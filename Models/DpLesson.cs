namespace AlgoMotion.Models;

public enum DpAlgorithm { Fibonacci, Knapsack, Lcs }

public sealed record DpStep(
  int Row,
  int Column,
  IReadOnlyList<int[]> Table,
  (int Row, int Column)[] Dependencies
);

public sealed record DpRecording(
  string[] Rows,
  string[] Columns,
  List<DpStep> Steps,
  int Result
);
