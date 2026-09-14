using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Heap Sort: build a max-heap in place, then repeatedly swap the
/// root (largest remaining element) with the last live slot and sift the new
/// root back down.
///
/// Reuses the same Compare/Swap visual language as Bubble/Selection/Insertion
/// Sort — every step still boils down to comparing or swapping a pair of live
/// array positions, so <see cref="SortAlgorithmInfo.ShowCrane"/> stays true.
/// <see cref="SortStep.PivotIndex"/> marks the subtree root currently being
/// sifted down (gold ring), and <see cref="SortStep.RangeEnd"/> tracks the
/// shrinking heap boundary so the already-extracted suffix dims out just like
/// Quick Sort's finished sub-ranges.
///
/// <code>
///  1  void heap_sort(int a[], size_t n)
///  2  {
///  3      for (int i = (int)n / 2 - 1; i &gt;= 0; i--) {
///  4          sift_down(a, n, i);
///  5      }
///  6      for (int end = (int)n - 1; end &gt; 0; end--) {
///  7          swap(&amp;a[0], &amp;a[end]);
///  8          sift_down(a, end, 0);
///  9      }
/// 10  }
/// 11
/// 12  void sift_down(int a[], size_t size, int root)
/// 13  {
/// 14      for (;;) {
/// 15          int largest = root;
/// 16          int left = 2 * root + 1, right = 2 * root + 2;
/// 17          if (left &lt; (int)size &amp;&amp; a[left] &gt; a[largest]) largest = left;
/// 18          if (right &lt; (int)size &amp;&amp; a[right] &gt; a[largest]) largest = right;
/// 19          if (largest == root) break;
/// 20          swap(&amp;a[root], &amp;a[largest]);
/// 21          root = largest;
/// 22      }
/// 23  }
/// </code>
/// </summary>
public static class HeapSortSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">heap_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = (<span class=\"tok-type\">int</span>)n / 2 - 1; i &gt;= 0; i--) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">sift_down</span>(a, n, i);",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> end = (<span class=\"tok-type\">int</span>)n - 1; end &gt; 0; end--) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swap(&amp;a[0], &amp;a[end]);",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">sift_down</span>(a, end, 0);",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "}",
    "",
    "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">sift_down</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> size, <span class=\"tok-type\">int</span> root)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (;;) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> largest = root;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> left = 2 * root + 1, right = 2 * root + 2;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (left &lt; (<span class=\"tok-type\">int</span>)size &amp;&amp; a[left] &gt; a[largest]) largest = left;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (right &lt; (<span class=\"tok-type\">int</span>)size &amp;&amp; a[right] &gt; a[largest]) largest = right;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (largest == root) <span class=\"tok-kw\">break</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swap(&amp;a[root], &amp;a[largest]);",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root = largest;",
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

    for (var i = (n / 2) - 1; i >= 0; i--)
    {
      SiftDown(a, n, i, steps, sorted, ref compareCount, ref swapCount);
    }

    for (var end = n - 1; end > 0; end--)
    {
      (a[0], a[end]) = (a[end], a[0]);
      swapCount++;
      sorted.Add(end);

      steps.Add(
        new SortStep
        {
          Type = StepType.Swap,
          Snapshot = [.. a],
          J = end,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = 0,
          RightIndex = end,
          RangeStart = 0,
          RangeEnd = end,
          ActiveCodeLines = [6, 7],
          SortedIndices = [.. sorted],
          Caption = $"Đưa đỉnh heap (lớn nhất còn lại) về đúng vị trí: đổi chỗ a[0] ↔ a[{end}]."
        }
      );

      SiftDown(a, end, 0, steps, sorted, ref compareCount, ref swapCount);
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
        ActiveCodeLines = [1, 2],
        SortedIndices = [.. sorted],
        Caption = "Hoàn tất! Dãy đã được sắp xếp."
      }
    );

    return steps;
  }

  /// <summary>Sifts the element at <paramref name="root"/> down into its correct place within the
  /// live heap of the given <paramref name="size"/>, swapping with the larger child until both
  /// children are smaller (or there are none left).</summary>
  private static void SiftDown(
    int[] a,
    int size,
    int root,
    List<SortStep> steps,
    SortedSet<int> sorted,
    ref int compareCount,
    ref int swapCount
  )
  {
    while (true)
    {
      var largest = root;
      var left = (2 * root) + 1;
      var right = (2 * root) + 2;

      if (left < size)
      {
        compareCount++;
        var leftBigger = a[left] > a[largest];

        steps.Add(
          new SortStep
          {
            Type = StepType.Compare,
            Snapshot = [.. a],
            I = root,
            J = left,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = largest,
            RightIndex = left,
            PivotIndex = root,
            RangeStart = 0,
            RangeEnd = size - 1,
            ActiveCodeLines = [17],
            SortedIndices = [.. sorted],
            Caption = $"So sánh con trái a[{left}] = {a[left]} với a[{largest}] = {a[largest]}"
              + (leftBigger ? "  →  con trái lớn hơn" : "  →  không lớn hơn")
          }
        );

        if (leftBigger)
        {
          largest = left;
        }
      }

      if (right < size)
      {
        compareCount++;
        var rightBigger = a[right] > a[largest];

        steps.Add(
          new SortStep
          {
            Type = StepType.Compare,
            Snapshot = [.. a],
            I = root,
            J = right,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = largest,
            RightIndex = right,
            PivotIndex = root,
            RangeStart = 0,
            RangeEnd = size - 1,
            ActiveCodeLines = [18],
            SortedIndices = [.. sorted],
            Caption = $"So sánh con phải a[{right}] = {a[right]} với a[{largest}] = {a[largest]}"
              + (rightBigger ? "  →  con phải lớn hơn" : "  →  không lớn hơn")
          }
        );

        if (rightBigger)
        {
          largest = right;
        }
      }

      if (largest == root)
      {
        steps.Add(
          new SortStep
          {
            Type = StepType.NoSwap,
            Snapshot = [.. a],
            I = root,
            CompareCount = compareCount,
            SwapCount = swapCount,
            PivotIndex = root,
            RangeStart = 0,
            RangeEnd = size - 1,
            ActiveCodeLines = [19],
            SortedIndices = [.. sorted],
            Caption = $"a[{root}] đã lớn hơn cả hai con — dừng dồn nhánh tại đây."
          }
        );

        return;
      }

      (a[root], a[largest]) = (a[largest], a[root]);
      swapCount++;

      steps.Add(
        new SortStep
        {
          Type = StepType.Swap,
          Snapshot = [.. a],
          I = root,
          J = largest,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = root,
          RightIndex = largest,
          PivotIndex = largest,
          RangeStart = 0,
          RangeEnd = size - 1,
          ActiveCodeLines = [20, 21],
          SortedIndices = [.. sorted],
          Caption = $"Đổi chỗ a[{root}] ↔ a[{largest}] để phần tử lớn nổi lên trên."
        }
      );

      root = largest;
    }
  }
}
