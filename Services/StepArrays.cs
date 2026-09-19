namespace AlgoMotion.Services;

/// <summary>
/// Hands out immutable, shared <c>int[]</c> copies for recorded steps.
///
/// A simulator records one <see cref="Models.SortStep"/> per comparison/swap, and each step used to
/// carry its own fresh copy of the whole array and of the sorted-index set — for n = 200 that is tens
/// of thousands of steps × two ~800-byte arrays retained at once. Recording runs synchronously on
/// Blazor WASM's single UI thread, so that allocation and the GC work it triggers froze the page for
/// up to ~2 s when the algorithm or array size changed. Most consecutive steps do not change either
/// array (a compare, or a step that only moves a highlight), so they can safely point at the same one.
///
/// Sharing is safe because steps are immutable frames: nothing that reads a step (the pages,
/// <c>SortBars</c>, <c>ComparisonPanel</c>) ever writes to <c>Snapshot</c> or <c>SortedIndices</c>.
/// State is per-thread and matched by content or by source set, so one recording can never leak
/// into another.
/// </summary>
public static class StepArrays
{
  [ThreadStatic]
  private static int[]? _snapshot;

  [ThreadStatic]
  private static SortedSet<int>? _sortedSource;

  [ThreadStatic]
  private static int[]? _sorted;

  [ThreadStatic]
  private static int[]? _prefix;

  /// <summary>A read-only copy of <paramref name="live"/>, reusing the previous copy while the contents are unchanged.</summary>
  public static int[] Snapshot(
    int[] live
  )
  {
    var last = _snapshot;
    if (last is not null && last.AsSpan().SequenceEqual(live))
    {
      return last;
    }

    return _snapshot = [.. live];
  }

  /// <summary>
  /// The sorted indices as an array, reusing the previous copy while <paramref name="sorted"/> has not grown.
  /// Requires the set to be add-only between calls (every simulator only ever calls <c>Add</c> on it), so an
  /// unchanged <see cref="SortedSet{T}.Count"/> on the same instance means unchanged contents.
  /// </summary>
  public static int[] Sorted(
    SortedSet<int> sorted
  )
  {
    var last = _sorted;
    if (last is not null && ReferenceEquals(sorted, _sortedSource) && sorted.Count == last.Length)
    {
      return last;
    }

    _sortedSource = sorted;
    return _sorted = [.. sorted];
  }

  /// <summary>The indices <c>0..count-1</c>, reusing the previous array while <paramref name="count"/> is unchanged.</summary>
  public static int[] Prefix(
    int count
  )
  {
    var length = Math.Max(0, count);
    var last = _prefix;
    if (last is not null && last.Length == length)
    {
      return last;
    }

    var result = new int[length];
    for (var k = 0; k < result.Length; k++)
    {
      result[k] = k;
    }

    return _prefix = result;
  }
}
