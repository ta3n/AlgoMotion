using AlgoMotion.Models;

namespace AlgoMotion.Services;

public static class GraphSimulator
{
  public static bool TryParse(int count, int start, string text, out GraphInput input)
  {
    input = new(count, start, []);
    if (count is < 2 or > 12 || start < 0 || start >= count) return false;
    var edges = new List<GraphEdge>();
    var seen = new HashSet<(int, int)>();
    foreach (var line in text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
    {
      var parts = line.Split([' ', ',', '\t'], StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length != 3 || !int.TryParse(parts[0], out var from) || !int.TryParse(parts[1], out var to)
          || !int.TryParse(parts[2], out var weight) || from < 0 || from >= count || to < 0 || to >= count
          || from == to || weight is < 0 or > 99 || !seen.Add((Math.Min(from, to), Math.Max(from, to)))) return false;
      edges.Add(new(from, to, weight));
      if (edges.Count > 40) return false;
    }
    input = new(count, start, edges);
    return true;
  }

  public static List<GraphStep> Record(GraphInput input, GraphAlgorithm algorithm)
  {
    var distances = Enumerable.Repeat(int.MaxValue, input.NodeCount).ToArray();
    var visited = new bool[input.NodeCount];
    var parents = Enumerable.Repeat(-1, input.NodeCount).ToArray();
    var selected = new List<int>();
    var frontier = new List<int> { input.Start };
    var steps = new List<GraphStep>();
    distances[input.Start] = 0;
    void Save(string key, int? node = null, int? edge = null) =>
      steps.Add(new(key, node, edge, [.. distances], [.. visited], [.. frontier], [.. selected]));
    Save("Initial");
    if (algorithm == GraphAlgorithm.Dfs)
    {
      frontier.Clear();
      VisitDepthFirst(input.Start);
      Save("Complete");
      return steps;
    }

    void VisitDepthFirst(int node)
    {
      visited[node] = true;
      frontier.Add(node);
      Save("Visit", node);
      foreach (var (edge, index) in input.Edges.Select((edge, index) => (edge, index)))
      {
        if (edge.From != node && edge.To != node) continue;
        var next = edge.From == node ? edge.To : edge.From;
        if (visited[next]) continue;
        distances[next] = distances[node] + 1;
        selected.Add(index);
        Save("Relax", node, index);
        VisitDepthFirst(next);
      }
      frontier.RemoveAt(frontier.Count - 1);
    }

    while (frontier.Count > 0)
    {
      var position = algorithm switch
      {
        GraphAlgorithm.Dijkstra or GraphAlgorithm.Prim => frontier.IndexOf(frontier.MinBy(n => distances[n])),
        _ => 0
      };
      var node = frontier[position];
      frontier.RemoveAt(position);
      visited[node] = true;
      if (parents[node] >= 0) selected.Add(parents[node]);
      Save(algorithm == GraphAlgorithm.Prim && parents[node] >= 0 ? "SelectEdge" : "Visit", node, parents[node] >= 0 ? parents[node] : null);
      var adjacent = input.Edges.Select((edge, index) => (edge, index))
        .Where(e => e.edge.From == node || e.edge.To == node).ToList();
      foreach (var (edge, index) in adjacent)
      {
        var next = edge.From == node ? edge.To : edge.From;
        if (visited[next]) continue;
        var candidate = algorithm switch
        {
          GraphAlgorithm.Prim => edge.Weight,
          GraphAlgorithm.Dijkstra => distances[node] + edge.Weight,
          _ => distances[node] + 1
        };
        var weighted = algorithm is GraphAlgorithm.Dijkstra or GraphAlgorithm.Prim;
        if (weighted ? candidate >= distances[next] : distances[next] != int.MaxValue) continue;
        distances[next] = candidate;
        parents[next] = index;
        if (!frontier.Contains(next)) frontier.Add(next);
        Save("Relax", node, index);
      }
    }
    Save("Complete");
    return steps;
  }
}
