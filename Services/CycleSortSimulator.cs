using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Cycle Sort — the algorithm that minimizes the number of swaps: it
/// follows each permutation cycle directly, moving every element to its
/// final resting place instead of bubbling elements past each other.
///
/// The textbook formulation holds the value being placed in a separate
/// <c>item</c> variable outside the array, leaving the origin slot stale
/// until the cycle closes — which means the array temporarily holds a
/// duplicate (and is missing the held value) between writes. That's
/// incompatible with this app's requirement that every <see cref="SortStep.Snapshot"/>
/// be a genuine permutation (Blazor's @key-based bar identity depends on it,
/// same constraint noted in <see cref="MergeSortSimulator"/>).
///
/// So this recorder uses the mathematically equivalent "decompose the cycle
/// into transpositions" formulation instead: always swap <c>a[start]</c>
/// directly with its computed target <c>a[pos]</c>, comparing against
/// <c>a[start]</c> itself rather than a floating value. A cycle of length L
/// still only takes L-1 real swaps this way (each element other than the one
/// already correctly placed moves exactly once), so the "very few operations"
/// point still holds, and every Snapshot stays a true permutation — which
/// also means a stable pair is swapped every time, so
/// <see cref="SortAlgorithmInfo.ShowCrane"/> stays true.
///
/// <code>
///  1  void cycle_sort(int a[], size_t n)
///  2  {
///  3      for (size_t start = 0; start + 1 &lt; n; start++) {
///  4          size_t pos = start;
///  5          for (size_t i = start + 1; i &lt; n; i++) {
///  6              if (a[i] &lt; a[start]) pos++;
///  7          }
///  8          if (pos == start) continue;
///  9          swap(&amp;a[start], &amp;a[pos]);
/// 10          while (pos != start) {
/// 11              pos = start;
/// 12              for (size_t i = start + 1; i &lt; n; i++) {
/// 13                  if (a[i] &lt; a[start]) pos++;
/// 14              }
/// 15              if (pos != start) {
/// 16                  swap(&amp;a[start], &amp;a[pos]);
/// 17              }
/// 18          }
/// 19      }
/// 20  }
/// </code>
/// </summary>
public static class CycleSortSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">cycle_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> start = 0; start + 1 &lt; n; start++) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> pos = start;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = start + 1; i &lt; n; i++) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; a[start]) pos++;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (pos == start) <span class=\"tok-kw\">continue</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swap(&amp;a[start], &amp;a[pos]);",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (pos != start) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;pos = start;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = start + 1; i &lt; n; i++) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; a[start]) pos++;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (pos != start) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swap(&amp;a[start], &amp;a[pos]);",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "}"
  ];

  public static List<SortStep> Record(
    IReadOnlyList<int> input
  )
  {
    var a = input.ToArray();
    var n = a.Length;
    var steps = new List<SortStep>();
    var sorted = new SortedSet<int>();

    var compareCount = 0;
    var swapCount = 0;

    for (var start = 0; start + 1 < n; start++)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = [.. a],
          I = start,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = start,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [3, 4],
          Caption = $"Bắt đầu chu trình tại vị trí {start}: tìm đúng vị trí cuối cùng của a[{start}] = {a[start]}."
        }
      );

      var pos = ScanForRank(a, start, steps, sorted, ref compareCount, swapCount, [5, 6]);

      if (pos == start)
      {
        sorted.Add(start);

        steps.Add(
          new SortStep
          {
            Type = StepType.NoSwap,
            Snapshot = [.. a],
            I = start,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = start,
            SortedIndices = [.. sorted],
            ActiveCodeLines = [8],
            Caption = $"a[{start}] đã đúng vị trí — bỏ qua chu trình này."
          }
        );

        continue;
      }

      (a[start], a[pos]) = (a[pos], a[start]);
      swapCount++;
      sorted.Add(pos);

      steps.Add(
        new SortStep
        {
          Type = StepType.Swap,
          Snapshot = [.. a],
          I = start,
          J = pos,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = start,
          RightIndex = pos,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [9],
          Caption = $"Đổi chỗ a[{start}] ↔ a[{pos}] — a[{pos}] = {a[pos]} đã về đúng vị trí cuối cùng."
        }
      );

      while (pos != start)
      {
        pos = ScanForRank(a, start, steps, sorted, ref compareCount, swapCount, [12, 13]);

        if (pos != start)
        {
          (a[start], a[pos]) = (a[pos], a[start]);
          swapCount++;
          sorted.Add(pos);

          steps.Add(
            new SortStep
            {
              Type = StepType.Swap,
              Snapshot = [.. a],
              I = start,
              J = pos,
              CompareCount = compareCount,
              SwapCount = swapCount,
              LeftIndex = start,
              RightIndex = pos,
              SortedIndices = [.. sorted],
              ActiveCodeLines = [15, 16],
              Caption = $"Đổi chỗ a[{start}] ↔ a[{pos}] — a[{pos}] = {a[pos]} đã về đúng vị trí cuối cùng."
            }
          );
        }
      }

      sorted.Add(start);

      steps.Add(
        new SortStep
        {
          Type = StepType.MarkSorted,
          Snapshot = [.. a],
          I = start,
          CompareCount = compareCount,
          SwapCount = swapCount,
          RightIndex = start,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [10],
          Caption = $"Chu trình khép lại — a[{start}] = {a[start]} cũng đã đúng vị trí."
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
        Snapshot = [.. a],
        CompareCount = compareCount,
        SwapCount = swapCount,
        SortedIndices = [.. sorted],
        ActiveCodeLines = [1, 2],
        Caption = "Hoàn tất! Dãy đã được sắp xếp."
      }
    );

    return steps;
  }

  /// <summary>Counts how many elements after <paramref name="start"/> are smaller than the
  /// current a[start], recording one Compare step per element scanned. The result is
  /// a[start]'s correct final rank within [start, n-1] given the array's current state.</summary>
  private static int ScanForRank(
    int[] a,
    int start,
    List<SortStep> steps,
    SortedSet<int> sorted,
    ref int compareCount,
    int swapCount,
    int[] activeCodeLines
  )
  {
    var pos = start;

    for (var i = start + 1; i < a.Length; i++)
    {
      compareCount++;
      var lessThanStart = a[i] < a[start];

      steps.Add(
        new SortStep
        {
          Type = StepType.Compare,
          Snapshot = [.. a],
          I = start,
          J = i,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = start,
          RightIndex = i,
          SortedIndices = [.. sorted],
          ActiveCodeLines = activeCodeLines,
          Caption = $"So sánh a[{i}] = {a[i]} với a[{start}] = {a[start]}"
            + (lessThanStart ? "  →  nhỏ hơn, tăng vị trí đích" : "  →  không nhỏ hơn")
        }
      );

      if (lessThanStart)
      {
        pos++;
      }
    }

    return pos;
  }
}
