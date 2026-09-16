using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Pancake Sort: repeatedly finds the largest remaining element and
/// brings it to its final position using only "flips" — reversing a prefix
/// of the array, the way a short-order cook flips a stack of pancakes with a
/// single spatula pass instead of picking one pancake out directly.
///
/// Finding the max reuses Selection Sort's exact
/// <see cref="StepType.Compare"/>/<see cref="StepType.NewCandidate"/> pattern
/// (just tracking a maximum instead of a minimum). A flip is recorded as a
/// sequence of ordinary <see cref="StepType.Swap"/> steps between shrinking
/// (lo, hi) pairs — a real pairwise swap each time, so the crane stays on
/// (<see cref="SortAlgorithmInfo.ShowCrane"/> = true) unlike the write-based
/// algorithms. Two flips per outer step bring the max from wherever it was
/// found to the front, then from the front down to its final resting spot.
///
/// <code>
///  1  void pancake_sort(int a[], size_t n)
///  2  {
///  3      for (size_t size = n; size &gt; 1; size--) {
///  4          size_t max_idx = 0;
///  5          for (size_t i = 1; i &lt; size; i++) {
///  6              if (a[i] &gt; a[max_idx]) max_idx = i;
///  7          }
///  8          if (max_idx != size - 1) {
///  9              flip(a, max_idx);
/// 10              flip(a, size - 1);
/// 11          }
/// 12      }
/// 13  }
/// 14
/// 15  void flip(int a[], size_t k)
/// 16  {
/// 17      for (size_t lo = 0, hi = k; lo &lt; hi; lo++, hi--) {
/// 18          int tmp = a[lo];
/// 19          a[lo] = a[hi];
/// 20          a[hi] = tmp;
/// 21      }
/// 22  }
/// </code>
/// </summary>
public static class PancakeSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">pancake_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> size = n; size &gt; 1; size--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> max_idx = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 1; i &lt; size; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &gt; a[max_idx]) max_idx = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (max_idx != size - 1) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, max_idx);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, size - 1);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">flip</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> k)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> lo = 0, hi = k; lo &lt; hi; lo++, hi--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[lo];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[lo] = a[hi];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[hi] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">PancakeSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> size = a.Length; size &gt; 1; size--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> maxIndex = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = 1; i &lt; size; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &gt; a[maxIndex]) maxIndex = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (maxIndex != size - 1) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">Flip</span>(a, maxIndex);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">Flip</span>(a, size - 1);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">Flip</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> k)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> lo = 0, hi = k; lo &lt; hi; lo++, hi--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[lo], a[hi]) = (a[hi], a[lo]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">pancakeSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> size = a.length; size &gt; 1; size--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> maxIndex = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = 1; i &lt; size; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &gt; a[maxIndex]) maxIndex = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (maxIndex != size - 1) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, maxIndex);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, size - 1);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">flip</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> k) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> lo = 0, hi = k; lo &lt; hi; lo++, hi--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[lo];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[lo] = a[hi];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[hi] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">pancake_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> size <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">range</span>(<span class=\"tok-fn\">len</span>(a), 1, -1):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;max_index = 0",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> i <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">range</span>(1, size):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> a[i] &gt; a[max_index]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;max_index = i",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> max_index != size - 1:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, max_index)",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, size - 1)",
      "",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">flip</span>(a, k):",
      "&nbsp;&nbsp;&nbsp;&nbsp;lo, hi = 0, k",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> lo &lt; hi:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[lo], a[hi] = a[hi], a[lo]",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lo += 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;hi -= 1"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">pancakeSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> size = a.length; size &gt; 1; size--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> maxIndex = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 1; i &lt; size; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &gt; a[maxIndex]) maxIndex = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (maxIndex !== size - 1) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, maxIndex);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, size - 1);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">flip</span>(a: <span class=\"tok-type\">number</span>[], k: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> lo = 0, hi = k; lo &lt; hi; lo++, hi--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[lo], a[hi]] = [a[hi], a[lo]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">pancakeSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> size = a.length; size &gt; 1; size--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> maxIndex = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 1; i &lt; size; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &gt; a[maxIndex]) maxIndex = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (maxIndex !== size - 1) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, maxIndex);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">flip</span>(a, size - 1);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">flip</span>(a, k) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> lo = 0, hi = k; lo &lt; hi; lo++, hi--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[lo], a[hi]] = [a[hi], a[lo]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ]
  };
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

    for (var size = n; size > 1; size--)
    {
      var maxIdx = 0;

      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = [.. a],
          I = size - 1,
          CompareCount = compareCount,
          SwapCount = swapCount,
          PivotIndex = maxIdx,
          RangeStart = 0,
          RangeEnd = size - 1,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [3, 4],
          Caption = $"Đoạn [0..{size - 1}]: giả sử a[0] là lớn nhất, đi tìm phần tử lớn hơn."
        }
      );

      for (var i = 1; i < size; i++)
      {
        compareCount++;
        var bigger = a[i] > a[maxIdx];

        steps.Add(
          new SortStep
          {
            Type = StepType.Compare,
            Snapshot = [.. a],
            I = size - 1,
            J = i,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = maxIdx,
            RightIndex = i,
            PivotIndex = maxIdx,
            RangeStart = 0,
            RangeEnd = size - 1,
            SortedIndices = [.. sorted],
            ActiveCodeLines = [5, 6],
            Caption = $"So sánh a[{i}] = {a[i]} với a[max] = {a[maxIdx]}"
              + (bigger ? "  →  lớn hơn!" : "  →  không lớn hơn")
          }
        );

        if (bigger)
        {
          maxIdx = i;

          steps.Add(
            new SortStep
            {
              Type = StepType.NewCandidate,
              Snapshot = [.. a],
              I = size - 1,
              J = i,
              CompareCount = compareCount,
              SwapCount = swapCount,
              LeftIndex = 0,
              RightIndex = maxIdx,
              PivotIndex = maxIdx,
              RangeStart = 0,
              RangeEnd = size - 1,
              SortedIndices = [.. sorted],
              ActiveCodeLines = [6],
              Caption = $"a[{i}] = {a[i]} lớn hơn — max cập nhật thành {i}."
            }
          );
        }
      }

      if (maxIdx != size - 1)
      {
        if (maxIdx > 0)
        {
          Flip(a, maxIdx, "Lật 1 (đưa lớn nhất lên đầu đoạn)", steps, sorted, size, ref swapCount);
        }

        Flip(a, size - 1, "Lật 2 (đưa lớn nhất về cuối đoạn)", steps, sorted, size, ref swapCount);
      }
      else
      {
        steps.Add(
          new SortStep
          {
            Type = StepType.NoSwap,
            Snapshot = [.. a],
            I = size - 1,
            CompareCount = compareCount,
            SwapCount = swapCount,
            RangeStart = 0,
            RangeEnd = size - 1,
            SortedIndices = [.. sorted],
            ActiveCodeLines = [8],
            Caption = $"a[{size - 1}] đã là lớn nhất trong đoạn — không cần lật."
          }
        );
      }

      sorted.Add(size - 1);

      steps.Add(
        new SortStep
        {
          Type = StepType.MarkSorted,
          Snapshot = [.. a],
          I = size - 1,
          CompareCount = compareCount,
          SwapCount = swapCount,
          RightIndex = size - 1,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [10],
          Caption = $"a[{size - 1}] đã về đúng vị trí cuối cùng."
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

  /// <summary>Reverses a[0..k] one swap at a time, recording each as an ordinary Swap step.</summary>
  private static void Flip(
    int[] a,
    int k,
    string label,
    List<SortStep> steps,
    SortedSet<int> sorted,
    int size,
    ref int swapCount
  )
  {
    var lo = 0;
    var hi = k;

    while (lo < hi)
    {
      (a[lo], a[hi]) = (a[hi], a[lo]);
      swapCount++;

      steps.Add(
        new SortStep
        {
          Type = StepType.Swap,
          Snapshot = [.. a],
          SwapCount = swapCount,
          LeftIndex = lo,
          RightIndex = hi,
          RangeStart = 0,
          RangeEnd = size - 1,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [17, 18, 19, 20],
          Caption = $"{label}: đổi chỗ a[{lo}] ↔ a[{hi}]."
        }
      );

      lo++;
      hi--;
    }
  }
}
