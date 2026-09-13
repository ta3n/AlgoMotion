namespace AlgoMotion.Models;

/// <summary>One historical caption entry — what the visualizer said, and when (wall-clock) it said it.</summary>
public sealed record CaptionLogEntry(
  DateTime Time,
  int StepIndex,
  string Caption
);
