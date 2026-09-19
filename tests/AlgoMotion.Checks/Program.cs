using AlgoMotion.Models;
using AlgoMotion.Services;

var assertions = 0;
void Check(bool condition, string name)
{
  assertions++;
  if (!condition) throw new InvalidOperationException(name);
}

#pragma warning disable S2245 // Seeded test data must be reproducible; not a security use.
var random = new Random(1778);
#pragma warning restore S2245
int[][] cases = [[], [1], [2, 1], [3, 3, 1, 2], [10000, 1, 10000, 9],
  .. Enumerable.Range(0, 35).Select(n => Enumerable.Range(0, n).Select(_ => random.Next(1, 40)).ToArray()),
  Enumerable.Range(1, 200).Reverse().ToArray()];
foreach (var algorithm in SortAlgorithms.All)
{
  foreach (var input in cases)
  {
    var original = input.ToArray();
    var steps = algorithm.Record(input, UiLanguage.En);
    Check(input.SequenceEqual(original), algorithm.Name + " mutated input");
    Check(steps.Count > 0, algorithm.Name + " empty recording");
    Check(steps[^1].Snapshot.SequenceEqual(input.Order()), algorithm.Name + " incorrect output");
    Check(steps[^1].Type == StepType.Completed, algorithm.Name + " incomplete");
    Check(steps.All(s => s.Snapshot.Length == input.Length), algorithm.Name + " snapshot length");
    Check(steps.All(s => s.SortedIndices.All(i => i >= 0 && i < input.Length)), algorithm.Name + " sorted index");
  }
}
foreach (var algorithm in TreeSearchAlgorithms.All)
{
  foreach (var input in cases.Take(40))
  {
    foreach (var target in new[] { 1, 3, 99999 })
    {
      var original = input.ToArray();
      var result = algorithm.Record(input, target, UiLanguage.En);
      Check(input.SequenceEqual(original), algorithm.Name + " mutated input");
      Check(result.Steps.Count > 0, algorithm.Name + " empty recording");
      Check((result.Steps[^1].Type == TreeSearchStepType.Found) == input.Contains(target), algorithm.Name + " result");
      Check(result.Steps.All(s => s.VisitedNodeIds.All(id => id >= 0 && id < result.Nodes.Count)), algorithm.Name + " invalid node");
    }
  }
}
Check(CustomInput.TryParse("5, 2; 2\n10000", 4, 200, out var parsed) && parsed.SequenceEqual(new[] {5,2,2,10000}), "parse delimiters");
foreach (var input in new[] { "", "1,2", "0,1,2,3", "-1,2,3,4", "10001,2,3,4", "2147483648,2,3,4", "x,2,3,4" })
  Check(!CustomInput.TryParse(input, 4, 200, out _), "reject " + input);

using var sort = new AnimationPlayer();
using var tree = new TreeAnimationPlayer();
sort.SeekTo(100);
tree.SeekTo(100);
Check(sort.State.CurrentIndex == -1 && tree.State.CurrentIndex == -1, "empty seek");
sort.Load([new(), new(), new()]);
tree.Load([new(), new(), new()]);
sort.SeekTo(100); tree.SeekTo(100);
Check(sort.IsAtEnd && tree.IsAtEnd, "end clamping");
sort.StepBack(); tree.StepBack();
Check(sort.State.CurrentIndex == 1 && tree.State.CurrentIndex == 1, "rewind");
sort.Reset(); tree.Reset(); sort.StepBack(); tree.StepBack();
Check(sort.Current == null && tree.Current == null, "initial boundary");
sort.Play(); tree.Play(); sort.SeekTo(1); tree.SeekTo(1);
await Task.Delay(800);
Check(!sort.State.IsPlaying && !tree.State.IsPlaying && sort.State.CurrentIndex == 1 && tree.State.CurrentIndex == 1, "seek cancels timers");

var quiz = new QuizAttempt();
quiz.Next();
Check(quiz.Index == 0, "quiz cannot skip unanswered");
foreach (var question in QuizBank.All)
{
  Check(quiz.Answer(question.Answer), "quiz accepts answer");
  Check(!quiz.Answer(question.Answer), "quiz blocks double score");
  foreach (var language in UiLanguages.All)
  {
    var key = question.Id + "Question";
    Check(LearningText.Get(key, language) != key, "localized quiz question");
  }
  quiz.Next();
}
Check(quiz.Finished && quiz.Score == QuizBank.All.Count, "quiz score");
quiz.Reset();
Check(quiz.Index == 0 && quiz.Score == 0 && quiz.Selected == null, "quiz reset");
Check(ComplexityExperiment.Measure(SortAlgorithmKind.Selection, 8, "Descending").Comparisons == 28, "selection comparisons");
Check(ComplexityExperiment.Input(32, "Random").SequenceEqual(ComplexityExperiment.Input(32, "Random")), "reproducible inputs");

Check(GraphSimulator.TryParse(4, 0, "0 1 4\n0 2 1\n2 1 2\n1 3 1\n2 3 5", out var graph), "valid graph");
var shortest = GraphSimulator.Record(graph, GraphAlgorithm.Dijkstra);
Check(shortest[^1].Distances.SequenceEqual(new[] {0, 3, 1, 4}), "Dijkstra known paths");
var mst = GraphSimulator.Record(graph, GraphAlgorithm.Prim)[^1];
Check(mst.SelectedEdges.Sum(i => graph.Edges[i].Weight) == 4 && mst.SelectedEdges.Length == 3, "Prim known MST");
Check(GraphSimulator.Record(graph, GraphAlgorithm.Bfs)[^1].Distances.SequenceEqual(new[] {0, 1, 1, 2}), "BFS distances");
Check(GraphSimulator.Record(graph, GraphAlgorithm.Dfs)[^1].Visited.All(v => v), "DFS reaches graph");
Check(shortest[0].Visited.All(v => !v) && shortest[0].Distances[1] == int.MaxValue, "immutable graph snapshots");
foreach (var invalid in new[] { "0 0 1", "0 4 1", "0 1 -1", "0 1 1\n1 0 2", "0 1", "x 1 2" })
  Check(!GraphSimulator.TryParse(4, 0, invalid, out _), "invalid graph " + invalid);
Check(GraphSimulator.TryParse(3, 0, "0 1 0", out var disconnected), "zero-weight graph");
foreach (var algorithm in Enum.GetValues<GraphAlgorithm>())
{
  var last = GraphSimulator.Record(disconnected, algorithm)[^1];
  Check(last.Visited.SequenceEqual(new[] {true, true, false}) && last.Distances[2] == int.MaxValue, "disconnected " + algorithm);
}
// Independent Floyd-Warshall oracle on seeded weighted graphs.
for (var iteration = 0; iteration < 30; iteration++)
{
  const int n = 6;
  var edges = new List<GraphEdge>();
  var distance = new int[n, n];
  for (var i = 0; i < n; i++)
    for (var j = 0; j < n; j++) distance[i, j] = i == j ? 0 : 100000;
  for (var i = 0; i < n; i++)
    for (var j = i + 1; j < n; j++)
      if (random.Next(3) != 0)
      {
        var w = random.Next(10);
        edges.Add(new(i, j, w));
        distance[i, j] = distance[j, i] = w;
      }
  for (var k = 0; k < n; k++)
    for (var i = 0; i < n; i++)
      for (var j = 0; j < n; j++) distance[i, j] = Math.Min(distance[i, j], distance[i, k] + distance[k, j]);
  for (var start = 0; start < n; start++)
  {
    var recorded = GraphSimulator.Record(new(n, start, edges), GraphAlgorithm.Dijkstra);
    Check(recorded[^1].Distances.SequenceEqual(Enumerable.Range(0, n).Select(i => distance[start, i] == 100000 ? int.MaxValue : distance[start, i])), "Dijkstra oracle");
    // Kruskal oracle restricted to the reachable component.
    var components = Enumerable.Range(0, n).ToArray();
    var weight = 0;
    foreach (var edge in edges.OrderBy(e => e.Weight))
    {
      if (distance[start, edge.From] == 100000 || components[edge.From] == components[edge.To]) continue;
      var old = components[edge.To];
      var replacement = components[edge.From];
      for (var i = 0; i < n; i++) if (components[i] == old) components[i] = replacement;
      weight += edge.Weight;
    }
    var prim = GraphSimulator.Record(new(n, start, edges), GraphAlgorithm.Prim)[^1];
    Check(prim.SelectedEdges.Sum(i => edges[i].Weight) == weight, "Prim vs Kruskal");
  }
}
Check(DpSimulator.Fibonacci(0).Result == 0 && DpSimulator.Fibonacci(1).Result == 1 && DpSimulator.Fibonacci(30).Result == 832040, "Fibonacci boundaries");
Check(DpSimulator.Knapsack([1,3,4,5], [1,4,5,7], 7).Result == 9, "knapsack known result");
Check(DpSimulator.Knapsack([2], [3], 4).Result == 3, "0/1 prevents reuse");
Check(DpSimulator.Lcs("ABCBDAB", "BDCABA").Result == 4 && DpSimulator.Lcs("", "ABC").Result == 0, "LCS known results");
for (var iteration = 0; iteration < 40; iteration++)
{
  var weights = Enumerable.Range(0, 6).Select(_ => random.Next(1, 8)).ToArray();
  var values = Enumerable.Range(0, 6).Select(_ => random.Next(1, 20)).ToArray();
  var best = 0;
  for (var mask = 0; mask < 64; mask++)
  {
    var w = 0; var v = 0;
    for (var i = 0; i < 6; i++) if ((mask & (1 << i)) != 0) { w += weights[i]; v += values[i]; }
    if (w <= 10) best = Math.Max(best, v);
  }
  var result = DpSimulator.Knapsack(weights, values, 10);
  Check(result.Result == best, "knapsack brute-force oracle");
  Check(result.Steps[0].Table.All(row => row.All(v => v == 0)), "DP snapshot isolation");
  var first = new string(Enumerable.Range(0, 6).Select(_ => (char)('A' + random.Next(3))).ToArray());
  var second = new string(Enumerable.Range(0, 6).Select(_ => (char)('A' + random.Next(3))).ToArray());
  var longest = 0;
  for (var mask = 0; mask < 64; mask++)
  {
    var candidate = first.Where((_, i) => (mask & (1 << i)) != 0).ToArray();
    var matched = 0;
    foreach (var c in second) if (matched < candidate.Length && c == candidate[matched]) matched++;
    if (matched == candidate.Length) longest = Math.Max(longest, matched);
  }
  Check(DpSimulator.Lcs(first, second).Result == longest, "LCS brute-force oracle");
}
sort.Reset(); tree.Reset();
sort.BreakAtIndex = 0; tree.BreakAtIndex = 0;
sort.Play(); tree.Play();
Check(!sort.State.IsPlaying && !tree.State.IsPlaying && sort.State.CurrentIndex == 0 && tree.State.CurrentIndex == 0, "breakpoints pause both players");
// Recording must stay cheap: it runs synchronously on Blazor WASM's single UI thread, so steps share
// unchanged arrays instead of each carrying its own copy (this once froze the page for up to ~2 s).
{
  var live = new[] { 3, 1, 2 };
  var first = StepArrays.Snapshot(live);
  Check(ReferenceEquals(first, StepArrays.Snapshot(live)), "unchanged snapshot is shared");
  live[0] = 9;
  var second = StepArrays.Snapshot(live);
  Check(!ReferenceEquals(first, second) && first.SequenceEqual(new[] { 3, 1, 2 }) && second.SequenceEqual(new[] { 9, 1, 2 }), "changed snapshot is copied and the earlier one is untouched");
  var set = new SortedSet<int> { 4 };
  var one = StepArrays.Sorted(set);
  Check(ReferenceEquals(one, StepArrays.Sorted(set)), "unchanged sorted set is shared");
  set.Add(2);
  Check(StepArrays.Sorted(set).SequenceEqual(new[] { 2, 4 }) && one.SequenceEqual(new[] { 4 }), "grown sorted set is copied");
  Check(StepArrays.Sorted(new SortedSet<int> { 4 }) is { Length: 1 } other && !ReferenceEquals(other, one) && StepArrays.Sorted(new SortedSet<int> { 7 })[0] == 7, "a different set never reuses another set's array");
  Check(StepArrays.Prefix(3).SequenceEqual(new[] { 0, 1, 2 }) && StepArrays.Prefix(0).Length == 0 && StepArrays.Prefix(-2).Length == 0, "prefix indices");
  foreach (var algorithm in SortAlgorithms.All)
  {
    var big = Enumerable.Range(1, 200).OrderBy(_ => random.Next()).ToArray();
    var recorded = algorithm.Record(big, UiLanguage.En);
    Check(recorded.All(s => s.Snapshot.Order().SequenceEqual(big.Order())), algorithm.Name + " snapshots stay permutations of the input");
    Check(recorded.All(s => s.SortedIndices.SequenceEqual(s.SortedIndices.Distinct().Order())), algorithm.Name + " sorted indices stay ascending and unique");
    var distinctSnapshots = recorded.Select(s => s.Snapshot).Distinct(ReferenceEqualityComparer.Instance).Count();
    Check(recorded.Count < 400 || distinctSnapshots < recorded.Count * 0.9, algorithm.Name + " shares snapshots between steps");
  }
}
Check(GraphSimulator.TryParse(3, 0, "0 1 1\n0 2 1\n1 2 1", out var triangle), "triangle");
Check(GraphSimulator.Record(triangle, GraphAlgorithm.Dfs)[^1].Distances.SequenceEqual(new[] {0,1,2}), "DFS tree depth");
Console.WriteLine($"PASS: {assertions} assertions across all simulator families, input, playback, experiments and quiz.");
