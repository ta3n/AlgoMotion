using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Counting Sort — the odd one out among these simulators: it never
/// compares two array elements to each other. Instead it tallies how many
/// times each value occurs, turns those tallies into cumulative positions,
/// then places every element directly into its final slot.
///
/// Because there's no stable "pair being compared" to point a crane at,
/// <see cref="SortAlgorithmInfo.ShowCrane"/> is false (same reasoning as
/// Merge Sort). <see cref="SortAlgorithmInfo.CompareLabel"/>/<see cref="SortAlgorithmInfo.ActionLabel"/>
/// are repurposed as "tally reads" / "writes" counters instead of
/// "comparisons" / "swaps".
///
/// The whole output array is computed first, then replayed one placement at
/// a time against that single, already-final snapshot — the same trick
/// <see cref="MergeSortSimulator"/> uses to keep every <see cref="SortStep.Snapshot"/>
/// a genuine permutation (required for Blazor's @key-based bar identity).
/// Placements are visited in the same back-to-front order the algorithm
/// actually uses (needed for stability), so which bar lights up next jumps
/// around rather than sweeping left to right — a deliberate contrast with
/// the comparison-based sorts.
///
/// <code>
///  1  void counting_sort(int a[], size_t n)
///  2  {
///  3      int min = a[0], max = a[0];
///  4      for (size_t i = 1; i &lt; n; i++) {
///  5          if (a[i] &lt; min) min = a[i];
///  6          if (a[i] &gt; max) max = a[i];
///  7      }
///  8
///  9      int range = max - min + 1;
/// 10      int count[range];
/// 11      memset(count, 0, sizeof(count));
/// 12      for (size_t i = 0; i &lt; n; i++) {
/// 13          count[a[i] - min]++;
/// 14      }
/// 15
/// 16      for (int v = 1; v &lt; range; v++) {
/// 17          count[v] += count[v - 1];
/// 18      }
/// 19
/// 20      int output[n];
/// 21      for (int i = (int)n - 1; i &gt;= 0; i--) {
/// 22          output[--count[a[i] - min]] = a[i];
/// 23      }
/// 24
/// 25      for (size_t i = 0; i &lt; n; i++) {
/// 26          a[i] = output[i];
/// 27      }
/// 28  }
/// </code>
/// </summary>
public static class CountingSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">counting_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> min = a[0], max = a[0];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 1; i &lt; n; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; min) min = a[i];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &gt; max) max = a[i];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> range = max - min + 1;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> count[range];",
        "&nbsp;&nbsp;&nbsp;&nbsp;memset(count, 0, <span class=\"tok-kw\">sizeof</span>(count));",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 0; i &lt; n; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[a[i] - min]++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> v = 1; v &lt; range; v++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[v] += count[v - 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> output[n];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = (<span class=\"tok-type\">int</span>)n - 1; i &gt;= 0; i--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[a[i] - min]] = a[i];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 0; i &lt; n; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i] = output[i];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">void</span> <span class=\"tok-fn\">CountingSort</span>(<span class=\"tok-type\">int</span>[] a)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a.Length &lt; 2) <span class=\"tok-kw\">return</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> min = a[0], max = a[0];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">foreach</span> (<span class=\"tok-type\">int</span> value <span class=\"tok-kw\">in</span> a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; min) min = value;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &gt; max) max = value;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] count = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[max - min + 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">foreach</span> (<span class=\"tok-type\">int</span> value <span class=\"tok-kw\">in</span> a) count[value - min]++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> v = 1; v &lt; count.Length; v++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[v] += count[v - 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] output = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[a.Length];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = a.Length - 1; i &gt;= 0; i--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[a[i] - min]] = a[i];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;output.<span class=\"tok-fn\">CopyTo</span>(a, 0);",
        "}"
      ],
      [CodeLanguage.Java] =
      [
        "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">void</span> <span class=\"tok-fn\">countingSort</span>(<span class=\"tok-type\">int</span>[] a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a.length &lt; 2) <span class=\"tok-kw\">return</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> min = a[0], max = a[0];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> value : a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; min) min = value;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &gt; max) max = value;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] count = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[max - min + 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> value : a) count[value - min]++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> v = 1; v &lt; count.length; v++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[v] += count[v - 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] output = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[a.length];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = a.length - 1; i &gt;= 0; i--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[a[i] - min]] = a[i];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;System.<span class=\"tok-fn\">arraycopy</span>(output, 0, a, 0, a.length);",
        "}"
      ],
      [CodeLanguage.Python] =
      [
        "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">counting_sort</span>(a):",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> <span class=\"tok-fn\">len</span>(a) &lt; 2:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span>",
        "&nbsp;&nbsp;&nbsp;&nbsp;low, high = <span class=\"tok-fn\">min</span>(a), <span class=\"tok-fn\">max</span>(a)",
        "&nbsp;&nbsp;&nbsp;&nbsp;count = [0] * (high - low + 1)",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> value <span class=\"tok-kw\">in</span> a:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[value - low] += 1",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> v <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">range</span>(1, <span class=\"tok-fn\">len</span>(count)):",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[v] += count[v - 1]",
        "&nbsp;&nbsp;&nbsp;&nbsp;output = [0] * <span class=\"tok-fn\">len</span>(a)",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> value <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">reversed</span>(a):",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[value - low] -= 1",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[count[value - low]] = value",
        "&nbsp;&nbsp;&nbsp;&nbsp;a[:] = output"
      ],
      [CodeLanguage.TypeScript] =
      [
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">countingSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-kw\">void</span> {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a.length &lt; 2) <span class=\"tok-kw\">return</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> min = a[0], max = a[0];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; min) min = value;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &gt; max) max = value;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> count = <span class=\"tok-kw\">new</span> Array&lt;<span class=\"tok-type\">number</span>&gt;(max - min + 1).<span class=\"tok-fn\">fill</span>(0);",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) count[value - min]++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> v = 1; v &lt; count.length; v++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[v] += count[v - 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> output = <span class=\"tok-kw\">new</span> Array&lt;<span class=\"tok-type\">number</span>&gt;(a.length);",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = a.length - 1; i &gt;= 0; i--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[a[i] - min]] = a[i];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 0; i &lt; a.length; i++) a[i] = output[i];",
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">countingSort</span>(a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a.length &lt; 2) <span class=\"tok-kw\">return</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> min = a[0], max = a[0];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; min) min = value;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &gt; max) max = value;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> count = <span class=\"tok-kw\">new</span> <span class=\"tok-fn\">Array</span>(max - min + 1).<span class=\"tok-fn\">fill</span>(0);",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) count[value - min]++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> v = 1; v &lt; count.length; v++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[v] += count[v - 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> output = <span class=\"tok-kw\">new</span> <span class=\"tok-fn\">Array</span>(a.length);",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = a.length - 1; i &gt;= 0; i--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[a[i] - min]] = a[i];",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 0; i &lt; a.length; i++) a[i] = output[i];",
        "}"
      ]
    };

  public static List<SortStep> Record(
    IReadOnlyList<int> input,
    UiLanguage language
  )
  {
    var a = input.ToArray();
    var n = a.Length;
    var steps = new List<SortStep>();
    var sorted = new SortedSet<int>();

    var tallyCount = 0;
    var writeCount = 0;

    if (n == 0)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.Completed,
          Snapshot = [],
          ActiveCodeLines = [1, 2],
          Caption = Res.Caption("Common_SortCompleted", language)
        }
      );

      return steps;
    }

    var min = a.Min();
    var max = a.Max();
    var range = max - min + 1;
    var count = new int[range];

    for (var i = 0; i < n; i++)
    {
      tallyCount++;
      count[a[i] - min]++;

      steps.Add(
        new SortStep
        {
          Type = StepType.CountTally,
          Snapshot = StepArrays.Snapshot(a),
          I = i,
          CompareCount = tallyCount,
          SwapCount = writeCount,
          LeftIndex = i,
          ActiveCodeLines = [12, 13],
          SortedIndices = StepArrays.Sorted(sorted),
          Caption = Res.Caption("Counting_Tally", language, i, a[i], a[i] - min, count[a[i] - min])
        }
      );
    }

    for (var v = 1; v < range; v++)
    {
      count[v] += count[v - 1];
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.EndPass,
        Snapshot = StepArrays.Snapshot(a),
        CompareCount = tallyCount,
        SwapCount = writeCount,
        ActiveCodeLines = [16, 17],
        SortedIndices = StepArrays.Sorted(sorted),
        Caption = Res.Caption("Counting_Prefix", language)
      }
    );

    var output = new int[n];
    var placements = new List<(int SourceIndex, int Target, int Value)>();

    for (var i = n - 1; i >= 0; i--)
    {
      var target = --count[a[i] - min];
      output[target] = a[i];
      placements.Add((i, target, a[i]));
    }

    foreach (var (sourceIndex, target, value) in placements)
    {
      writeCount++;
      sorted.Add(target);

      steps.Add(
        new SortStep
        {
          Type = StepType.CountPlace,
          Snapshot = output,
          CompareCount = tallyCount,
          SwapCount = writeCount,
          RightIndex = target,
          ActiveCodeLines = [21, 22],
          SortedIndices = StepArrays.Sorted(sorted),
          Caption = Res.Caption("Counting_Place", language, value, sourceIndex, target)
        }
      );
    }

    for (var k = 0; k < n; k++)
    {
      sorted.Add(k);
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.Completed,
        Snapshot = output,
        CompareCount = tallyCount,
        SwapCount = writeCount,
        ActiveCodeLines = [1, 2],
        SortedIndices = StepArrays.Sorted(sorted),
        Caption = Res.Caption("Common_SortCompleted", language)
      }
    );

    return steps;
  }
}
