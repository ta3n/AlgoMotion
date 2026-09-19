using AlgoMotion.Models;

namespace AlgoMotion.Services;

public static class GraphSimulator
{
  public static bool TryParse(
    int count,
    int start,
    string text,
    out GraphInput input
  )
  {
    input = new(count, start, []);
    if (count is < 2 or > 12 || start < 0 || start >= count)
    {
      return false;
    }

    var edges = new List<GraphEdge>();
    var seen = new HashSet<(int, int)>();
    foreach (var line in text.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries))
    {
      var parts = line.Split([' ', ',', '\t'], StringSplitOptions.RemoveEmptyEntries);
      if (parts.Length != 3
        || !int.TryParse(parts[0], out var from)
        || !int.TryParse(parts[1], out var to)
        || !int.TryParse(parts[2], out var weight)
        || from < 0
        || from >= count
        || to < 0
        || to >= count
        || from == to
        || weight is < -99 or > 99
        || !seen.Add((Math.Min(from, to), Math.Max(from, to))))
      {
        return false;
      }

      edges.Add(new(from, to, weight));
      if (edges.Count > 40)
      {
        return false;
      }
    }

    input = new(count, start, edges);
    return true;
  }

  public static List<GraphStep> Record(
    GraphInput input,
    GraphAlgorithm algorithm
  )
  {
    var distances = Enumerable.Repeat(int.MaxValue, input.NodeCount).ToArray();
    var visited = new bool[input.NodeCount];
    var parents = Enumerable.Repeat(-1, input.NodeCount).ToArray();
    var selected = new List<int>();
    var frontier = new List<int> { input.Start };
    var steps = new List<GraphStep>();
    // Topological Sort seeds every zero-in-degree node, not just Start, so it must not
    // inherit the "Start already knows its own distance is 0" head start the other algorithms rely on.
    if (algorithm != GraphAlgorithm.TopologicalSort)
    {
      distances[input.Start] = 0;
    }

    void Save(
      string key,
      int? node = null,
      int? edge = null,
      int? round = null
    )
    {
      steps.Add(new(key, node, edge, [.. distances], [.. visited], [.. frontier], [.. selected], round));
    }

    Save("Initial");
    if (algorithm == GraphAlgorithm.Dfs)
    {
      frontier.Clear();
      VisitDepthFirst(input.Start);
      Save("Complete");
      return steps;
    }

    if (algorithm == GraphAlgorithm.TopologicalSort)
    {
      // Kahn's algorithm. Edges are read as directed From→To here (the parser still stores
      // each pair only once, so a graph edited for BFS/Dijkstra doubles as a DAG definition).
      frontier.Clear();
      var inDegree = new int[input.NodeCount];
      foreach (var edge in input.Edges)
      {
        inDegree[edge.To]++;
      }

      for (var node = 0; node < input.NodeCount; node++)
      {
        if (inDegree[node] == 0)
        {
          frontier.Add(node);
        }
      }

      frontier.Sort();
      var order = 0;
      while (frontier.Count > 0)
      {
        var node = frontier[0];
        frontier.RemoveAt(0);
        visited[node] = true;
        distances[node] = order++;
        Save("Visit", node);
        foreach (var (edge, index) in input.Edges.Select((edge, index) => (edge, index)))
        {
          if (edge.From != node)
          {
            continue;
          }

          inDegree[edge.To]--;
          Save("Relax", node, index);
          if (inDegree[edge.To] == 0)
          {
            frontier.Add(edge.To);
            frontier.Sort();
          }
        }
      }

      Save(order < input.NodeCount ? "CycleDetected" : "Complete");
      return steps;
    }

    if (algorithm == GraphAlgorithm.Kruskal)
    {
      // Restricted to the component reachable from Start so its MST weight is directly
      // comparable to Prim's — Prim can only ever grow a single tree from that same node.
      frontier.Clear();
      var reachable = new bool[input.NodeCount];
      var toVisit = new Stack<int>();
      toVisit.Push(input.Start);
      reachable[input.Start] = true;
      while (toVisit.Count > 0)
      {
        var node = toVisit.Pop();
        foreach (var edge in input.Edges)
        {
          int next;
          if (edge.From == node) next = edge.To;
          else if (edge.To == node) next = edge.From;
          else next = -1;
          if (next < 0 || reachable[next])
          {
            continue;
          }

          reachable[next] = true;
          toVisit.Push(next);
        }
      }

      var parent = Enumerable.Range(0, input.NodeCount).ToArray();
      int Find(
        int x
      )
      {
        if (parent[x] == x)
        {
          return x;
        }

        parent[x] = Find(parent[x]);
        return parent[x];
      }

      var orderedEdges = input.Edges.Select((edge, index) => (edge, index))
        .Where(e => reachable[e.edge.From] && reachable[e.edge.To])
        .OrderBy(e => e.edge.Weight)
        .ThenBy(e => e.index);
      foreach (var (edge, index) in orderedEdges)
      {
        var rootFrom = Find(edge.From);
        var rootTo = Find(edge.To);
        if (rootFrom == rootTo)
        {
          Save("RejectEdge", edge.From, index);
          continue;
        }

        parent[rootFrom] = rootTo;
        selected.Add(index);
        visited[edge.From] = true;
        visited[edge.To] = true;
        Save("SelectEdge", edge.From, index);
      }

      Save("Complete");
      return steps;
    }

    if (algorithm == GraphAlgorithm.BellmanFord)
    {
      // Edges are read as directed From→To, like Topological Sort — an undirected negative
      // edge would otherwise always form a trivial two-step negative cycle.
      frontier.Clear();
      var rounds = Math.Max(0, input.NodeCount - 1);
      for (var round = 1; round <= rounds; round++)
      {
        var improved = false;
        foreach (var (edge, index) in input.Edges.Select((edge, index) => (edge, index)))
        {
          if (distances[edge.From] == int.MaxValue)
          {
            continue;
          }

          var candidate = distances[edge.From] + edge.Weight;
          if (candidate >= distances[edge.To])
          {
            continue;
          }

          distances[edge.To] = candidate;
          parents[edge.To] = index;
          improved = true;
          Save("Relax", edge.To, index, round);
        }

        if (!improved)
        {
          break;
        }
      }

      var negativeCycle = input.Edges.Any(
        edge => distances[edge.From] != int.MaxValue && distances[edge.From] + edge.Weight < distances[edge.To]
      );
      for (var node = 0; node < input.NodeCount; node++)
      {
        visited[node] = distances[node] != int.MaxValue;
      }

      Save(negativeCycle ? "NegativeCycle" : "Complete");
      return steps;
    }

    void VisitDepthFirst(
      int node
    )
    {
      visited[node] = true;
      frontier.Add(node);
      Save("Visit", node);
      foreach (var (edge, index) in input.Edges.Select((edge, index) => (edge, index)))
      {
        if (edge.From != node && edge.To != node)
        {
          continue;
        }

        var next = edge.From == node ? edge.To : edge.From;
        if (visited[next])
        {
          continue;
        }

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
      if (parents[node] >= 0)
      {
        selected.Add(parents[node]);
      }

      Save(
        algorithm == GraphAlgorithm.Prim && parents[node] >= 0 ? "SelectEdge" : "Visit",
        node,
        parents[node] >= 0 ? parents[node] : null
      );
      var adjacent = input.Edges.Select((edge, index) => (edge, index))
        .Where(e => e.edge.From == node || e.edge.To == node)
        .ToList();
      foreach (var (edge, index) in adjacent)
      {
        var next = edge.From == node ? edge.To : edge.From;
        if (visited[next])
        {
          continue;
        }

        var candidate = algorithm switch
        {
          GraphAlgorithm.Prim => edge.Weight,
          GraphAlgorithm.Dijkstra => distances[node] + edge.Weight,
          _ => distances[node] + 1
        };
        var weighted = algorithm is GraphAlgorithm.Dijkstra or GraphAlgorithm.Prim;
        if (weighted ? candidate >= distances[next] : distances[next] != int.MaxValue)
        {
          continue;
        }

        distances[next] = candidate;
        parents[next] = index;
        if (!frontier.Contains(next))
        {
          frontier.Add(next);
        }

        Save("Relax", node, index);
      }
    }

    Save("Complete");
    return steps;
  }
}
