namespace AlgoMotion.Models;

public enum GraphAlgorithm { Bfs, Dfs, Dijkstra, Prim, Kruskal, BellmanFord, TopologicalSort }

public sealed record GraphEdge(
  int From,
  int To,
  int Weight
);

public sealed record GraphInput(
  int NodeCount,
  int Start,
  IReadOnlyList<GraphEdge> Edges
);

public sealed record GraphStep(
  string CaptionKey,
  int? Current,
  int? EdgeIndex,
  int[] Distances,
  bool[] Visited,
  int[] Frontier,
  int[] SelectedEdges,
  int? Round = null
);
