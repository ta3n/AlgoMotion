using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Quick Sort (Lomuto partition scheme, pivot = last element of the
/// sub-range). Implemented iteratively with an explicit stack of (lo, hi)
/// ranges instead of real recursion, so the recorder stays a flat loop like
/// every other simulator — the recursion tree is just visited depth-first via
/// the stack, giving the same left-to-right visual order a recursive call
/// would produce.
///
/// Bars outside the current [lo, hi] range are dimmed by the UI (via
/// <see cref="SortStep.RangeStart"/>/<see cref="SortStep.RangeEnd"/>) to keep
/// focus on the sub-array actually being partitioned; the pivot gets a gold
/// ring via <see cref="SortStep.PivotIndex"/>.
///
/// <code>
///  1  void quick_sort(int a[], int lo, int hi)
///  2  {
///  3      if (lo &gt;= hi) return;
///  4      int pivot = a[hi];
///  5      int i = lo - 1;
///  6      for (int j = lo; j &lt; hi; j++) {
///  7          if (a[j] &lt; pivot) {
///  8              i++;
///  9              swap(&amp;a[i], &amp;a[j]);
/// 10          }
/// 11      }
/// 12      swap(&amp;a[i + 1], &amp;a[hi]);
/// 13      int p = i + 1;
/// 14      quick_sort(a, lo, p - 1);
/// 15      quick_sort(a, p + 1, hi);
/// 16  }
/// </code>
/// </summary>
public static class QuickSortSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">quick_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">int</span> lo, <span class=\"tok-type\">int</span> hi)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (lo &gt;= hi) <span class=\"tok-kw\">return</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> pivot = a[hi];",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> i = lo - 1;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = lo; j &lt; hi; j++) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &lt; pivot) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i++;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swap(&amp;a[i], &amp;a[j]);",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;swap(&amp;a[i + 1], &amp;a[hi]);",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> p = i + 1;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">quick_sort</span>(a, lo, p - 1);",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">quick_sort</span>(a, p + 1, hi);",
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

    var stack = new Stack<(int lo, int hi)>();
    if (n > 0)
    {
      stack.Push((0, n - 1));
    }

    while (stack.Count > 0)
    {
      var (lo, hi) = stack.Pop();

      if (lo >= hi)
      {
        if (lo == hi)
        {
          sorted.Add(lo);
        }

        continue;
      }

      var pivotValue = a[hi];

      steps.Add(
        new SortStep
        {
          Type = StepType.SetPivot,
          Snapshot = [.. a],
          I = lo,
          J = hi,
          CompareCount = compareCount,
          SwapCount = swapCount,
          PivotIndex = hi,
          RangeStart = lo,
          RangeEnd = hi,
          ActiveCodeLines = [3, 4, 5],
          SortedIndices = [.. sorted],
          Caption = $"Vùng [{lo}..{hi}]: chọn pivot = a[{hi}] = {pivotValue}."
        }
      );

      var i = lo - 1;

      for (var j = lo; j < hi; j++)
      {
        compareCount++;
        var smaller = a[j] < pivotValue;

        steps.Add(
          new SortStep
          {
            Type = StepType.Compare,
            Snapshot = [.. a],
            I = lo,
            J = j,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = j,
            RightIndex = hi,
            PivotIndex = hi,
            RangeStart = lo,
            RangeEnd = hi,
            ActiveCodeLines = [6, 7],
            SortedIndices = [.. sorted],
            Caption = $"So sánh a[{j}] = {a[j]} với pivot = {pivotValue}"
              + (smaller ? "  →  nhỏ hơn" : "  →  không nhỏ hơn")
          }
        );

        if (smaller)
        {
          i++;
          if (i != j)
          {
            (a[i], a[j]) = (a[j], a[i]);
            swapCount++;

            steps.Add(
              new SortStep
              {
                Type = StepType.Swap,
                Snapshot = [.. a],
                I = lo,
                J = j,
                CompareCount = compareCount,
                SwapCount = swapCount,
                LeftIndex = i,
                RightIndex = j,
                PivotIndex = hi,
                RangeStart = lo,
                RangeEnd = hi,
                ActiveCodeLines = [8, 9],
                SortedIndices = [.. sorted],
                Caption = $"Đưa a[{j}] về vùng nhỏ hơn pivot: đổi chỗ a[{i}] ↔ a[{j}]."
              }
            );
          }
          else
          {
            steps.Add(
              new SortStep
              {
                Type = StepType.NoSwap,
                Snapshot = [.. a],
                I = lo,
                J = j,
                CompareCount = compareCount,
                SwapCount = swapCount,
                LeftIndex = i,
                RightIndex = j,
                PivotIndex = hi,
                RangeStart = lo,
                RangeEnd = hi,
                ActiveCodeLines = [8, 9],
                SortedIndices = [.. sorted],
                Caption = $"a[{j}] đã đúng vùng, không cần đổi chỗ."
              }
            );
          }
        }
      }

      (a[i + 1], a[hi]) = (a[hi], a[i + 1]);
      swapCount++;
      var p = i + 1;

      steps.Add(
        new SortStep
        {
          Type = StepType.Swap,
          Snapshot = [.. a],
          I = lo,
          J = hi,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = p,
          RightIndex = hi,
          PivotIndex = p,
          RangeStart = lo,
          RangeEnd = hi,
          ActiveCodeLines = [12, 13],
          SortedIndices = [.. sorted],
          Caption = $"Đặt pivot vào đúng vị trí: đổi chỗ a[{p}] ↔ a[{hi}]."
        }
      );

      sorted.Add(p);

      steps.Add(
        new SortStep
        {
          Type = StepType.RangeDone,
          Snapshot = [.. a],
          I = lo,
          J = hi,
          CompareCount = compareCount,
          SwapCount = swapCount,
          PivotIndex = p,
          RangeStart = lo,
          RangeEnd = hi,
          ActiveCodeLines = [14, 15],
          SortedIndices = [.. sorted],
          Caption = $"a[{p}] cố định — chia thành vùng [{lo}..{p - 1}] và [{p + 1}..{hi}]."
        }
      );

      // Push right first so the left sub-range is processed first (matches recursive call order).
      stack.Push((p + 1, hi));
      stack.Push((lo, p - 1));
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
}
