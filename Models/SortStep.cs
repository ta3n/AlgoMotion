namespace AlgoMotion.Models;

/// <summary>
/// The kind of event a single recorded step represents. The UI uses this
/// to decide timing, captions and which code lines to light up.
///
/// The first block is shared by every algorithm (compare/swap around a pair
/// of adjacent indices). The blocks below add the extra vocabulary each
/// algorithm needs — a running best candidate for Selection Sort, a pivot
/// and sub-range for Quick Sort, a split/merge for Merge Sort — without
/// disturbing how Bubble Sort already uses the shared ones.
/// </summary>
public enum StepType
{
  StartPass,
  Compare,
  Swap,
  NoSwap,
  MarkSorted,
  EndPass,
  Completed,

  /// <summary>Selection Sort: a new running-minimum candidate was found.</summary>
  NewCandidate,

  /// <summary>Quick Sort: a pivot was chosen for the current sub-range.</summary>
  SetPivot,

  /// <summary>Quick/Merge Sort: the current sub-range is fully sorted/merged.</summary>
  RangeDone,

  /// <summary>Merge Sort: a range is being divided into two halves.</summary>
  SplitRange,

  /// <summary>Merge Sort: comparing the fronts of the two halves being merged.</summary>
  MergeCompare,

  /// <summary>Merge Sort: the smaller front value is written into its merged slot.</summary>
  MergeWrite
}

/// <summary>
/// One immutable frame of a sorting-algorithm animation.
///
/// Each simulator (see the classes in <c>Services</c>) runs its whole
/// algorithm up front and records every frame as a <see cref="SortStep"/>.
/// The UI never re-implements the algorithm: it simply plays this list back,
/// one step at a time, and lets CSS animate the difference between frames.
/// The type is shared across all algorithms; fields only one or two
/// algorithms need (like <see cref="PivotIndex"/>) are simply left null by
/// the others.
/// </summary>
public sealed class SortStep
{
  public StepType Type { get; set; }

  /// <summary>The full array state *after* this step is applied.</summary>
  public int[] Snapshot { get; set; } = [];

  /// <summary>Outer loop index (pass number), -1 when not applicable.</summary>
  public int I { get; set; } = -1;

  /// <summary>Inner loop index (comparison position), -1 when not applicable.</summary>
  public int J { get; set; } = -1;

  public int CompareCount { get; set; }

  public int SwapCount { get; set; }

  public bool Swapped { get; set; }

  /// <summary>Index of the left element of the pair being compared/swapped.</summary>
  public int? LeftIndex { get; set; }

  /// <summary>Index of the right element of the pair being compared/swapped.</summary>
  public int? RightIndex { get; set; }

  /// <summary>Selection Sort's running-minimum index, or Quick Sort's pivot index.</summary>
  public int? PivotIndex { get; set; }

  /// <summary>Quick/Merge Sort: start of the sub-array currently being worked on. Bars outside [Start,End] are dimmed.</summary>
  public int? RangeStart { get; set; }

  /// <summary>Quick/Merge Sort: end (inclusive) of the sub-array currently being worked on.</summary>
  public int? RangeEnd { get; set; }

  /// <summary>1-based source line numbers to highlight in the code panel.</summary>
  public int[] ActiveCodeLines { get; set; } = [];

  /// <summary>Indices that have already settled into their final, sorted position.</summary>
  public int[] SortedIndices { get; set; } = [];

  /// <summary>Human readable caption shown under the code panel.</summary>
  public string Caption { get; set; } = "";
}
