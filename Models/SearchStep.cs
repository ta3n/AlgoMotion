namespace AlgoMotion.Models;

/// <summary>
/// The kind of event a single recorded array-search step represents. The player uses this to decide
/// how long a frame dwells on screen.
/// </summary>
public enum SearchStepType
{
  /// <summary>Compared one element against the target without narrowing anything (linear scan, or the block check in Jump Search).</summary>
  Check,

  /// <summary>Probed an element that is bigger than the target — the upper part of the range is discarded.</summary>
  NarrowLeft,

  /// <summary>Probed an element that is smaller than the target — the lower part of the range is discarded.</summary>
  NarrowRight,

  /// <summary>Jump Search: the end of a whole block is still smaller than the target, so the block is skipped.</summary>
  JumpBlock,

  /// <summary>Target found.</summary>
  Found,

  /// <summary>Search exhausted the candidate range without finding the target.</summary>
  NotFound
}

/// <summary>
/// One immutable frame of an array-search animation — the searching counterpart of
/// <see cref="SortStep"/> / <see cref="TreeSearchStep"/>. Each simulator runs the whole search up front
/// and records every frame; the UI only plays the list back.
/// </summary>
public sealed class SearchStep
{
  public SearchStepType Type { get; init; }

  /// <summary>The searched array. A search never modifies it, so every step of one recording shares the same instance.</summary>
  public int[] Snapshot { get; init; } = [];

  /// <summary>The index being compared in this frame, or null when the frame doesn't point at one.</summary>
  public int? CheckedIndex { get; init; }

  /// <summary>Inclusive bounds of the candidate range still worth searching after this frame. Bars outside
  /// it are dimmed; <c>RangeStart &gt; RangeEnd</c> means nothing is left. Null on the initial frame.</summary>
  public int? RangeStart { get; init; }

  public int? RangeEnd { get; init; }

  /// <summary>Index of the match, set only on <see cref="SearchStepType.Found"/>.</summary>
  public int? FoundIndex { get; init; }

  public int CompareCount { get; init; }

  /// <summary>How many times the candidate range has been narrowed so far (elements ruled out for Linear Search,
  /// halvings for Binary, skipped blocks for Jump, probes for Interpolation).</summary>
  public int NarrowCount { get; init; }

  /// <summary>1-based source line numbers of the C reference to highlight in the code panel.</summary>
  public int[] ActiveCodeLines { get; init; } = [];

  public string Caption { get; init; } = "";
}
