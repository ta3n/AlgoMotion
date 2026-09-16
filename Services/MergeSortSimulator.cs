using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Merge Sort (top-down, recursive split + merge with a temp buffer).
///
/// Unlike the other simulators, a merge's two "pointers" don't correspond to
/// a stable pair of positions in the live array — once values start
/// interleaving from both halves, the original index of the next unconsumed
/// element no longer reflects what's actually still there. So this
/// algorithm sets <see cref="SortAlgorithmInfo.ShowCrane"/> to false and
/// leans on <see cref="SortStep.RangeStart"/>/<see cref="SortStep.RangeEnd"/>
/// (dims everything outside the active range) plus a single-bar highlight on
/// <see cref="SortStep.RightIndex"/> for the slot just written.
///
/// A position is only marked "sorted" (green) once the *top-level* merge
/// (covering the whole array) writes it — a sub-merge's result can still be
/// reshuffled by a later, larger merge, so marking it green any earlier
/// would be a lie.
///
/// <code>
///  1  void merge_sort(int a[], int lo, int hi)
///  2  {
///  3      if (lo &gt;= hi) return;
///  4      int mid = lo + (hi - lo) / 2;
///  5      merge_sort(a, lo, mid);
///  6      merge_sort(a, mid + 1, hi);
///  7      merge(a, lo, mid, hi);
///  8  }
///  9
/// 10  void merge(int a[], int lo, int mid, int hi)
/// 11  {
/// 12      int tmp[hi - lo + 1];
/// 13      int i = lo, j = mid + 1, k = 0;
/// 14      while (i &lt;= mid &amp;&amp; j &lt;= hi) {
/// 15          tmp[k++] = (a[i] &lt;= a[j]) ? a[i++] : a[j++];
/// 16      }
/// 17      while (i &lt;= mid) tmp[k++] = a[i++];
/// 18      while (j &lt;= hi)  tmp[k++] = a[j++];
/// 19      for (int x = 0; x &lt; k; x++) a[lo + x] = tmp[x];
/// 20  }
/// </code>
/// </summary>
public static class MergeSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">merge_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">int</span> lo, <span class=\"tok-type\">int</span> hi)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (lo &gt;= hi) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> mid = lo + (hi - lo) / 2;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge_sort</span>(a, lo, mid);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge_sort</span>(a, mid + 1, hi);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge</span>(a, lo, mid, hi);",
      "}",
      "",
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">merge</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">int</span> lo, <span class=\"tok-type\">int</span> mid, <span class=\"tok-type\">int</span> hi)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp[hi - lo + 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> i = lo, j = mid + 1, k = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid &amp;&amp; j &lt;= hi) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;tmp[k++] = (a[i] &lt;= a[j]) ? a[i++] : a[j++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid) tmp[k++] = a[i++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &lt;= hi)&nbsp;&nbsp;tmp[k++] = a[j++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> x = 0; x &lt; k; x++) a[lo + x] = tmp[x];",
      "}"
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">MergeSort</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> lo, <span class=\"tok-type\">int</span> hi)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (lo &gt;= hi) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> mid = lo + (hi - lo) / 2;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">MergeSort</span>(a, lo, mid);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">MergeSort</span>(a, mid + 1, hi);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">Merge</span>(a, lo, mid, hi);",
      "}",
      "",
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">Merge</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> lo, <span class=\"tok-type\">int</span> mid, <span class=\"tok-type\">int</span> hi)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">var</span> tmp = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[hi - lo + 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> i = lo, j = mid + 1, k = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid &amp;&amp; j &lt;= hi)",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;tmp[k++] = a[i] &lt;= a[j] ? a[i++] : a[j++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid) tmp[k++] = a[i++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &lt;= hi) tmp[k++] = a[j++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> x = 0; x &lt; k; x++) a[lo + x] = tmp[x];",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">mergeSort</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> lo, <span class=\"tok-type\">int</span> hi) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (lo &gt;= hi) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> mid = lo + (hi - lo) / 2;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">mergeSort</span>(a, lo, mid);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">mergeSort</span>(a, mid + 1, hi);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge</span>(a, lo, mid, hi);",
      "}",
      "",
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">merge</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> lo, <span class=\"tok-type\">int</span> mid, <span class=\"tok-type\">int</span> hi) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] tmp = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[hi - lo + 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> i = lo, j = mid + 1, k = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid &amp;&amp; j &lt;= hi) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;tmp[k++] = (a[i] &lt;= a[j]) ? a[i++] : a[j++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid) tmp[k++] = a[i++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &lt;= hi) tmp[k++] = a[j++];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> x = 0; x &lt; k; x++) a[lo + x] = tmp[x];",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">merge_sort</span>(a, lo, hi):",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> lo &gt;= hi:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span>",
      "&nbsp;&nbsp;&nbsp;&nbsp;mid = lo + (hi - lo) // 2",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge_sort</span>(a, lo, mid)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge_sort</span>(a, mid + 1, hi)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge</span>(a, lo, mid, hi)",
      "",
      "",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">merge</span>(a, lo, mid, hi):",
      "&nbsp;&nbsp;&nbsp;&nbsp;tmp = []",
      "&nbsp;&nbsp;&nbsp;&nbsp;i, j = lo, mid + 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> i &lt;= mid <span class=\"tok-kw\">and</span> j &lt;= hi:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> a[i] &lt;= a[j]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;tmp.append(a[i]); i += 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">else</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;tmp.append(a[j]); j += 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> i &lt;= mid:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;tmp.append(a[i]); i += 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> j &lt;= hi:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;tmp.append(a[j]); j += 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;a[lo:hi + 1] = tmp"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">mergeSort</span>(a: <span class=\"tok-type\">number</span>[], lo: <span class=\"tok-type\">number</span>, hi: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (lo &gt;= hi) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> mid = lo + Math.floor((hi - lo) / 2);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">mergeSort</span>(a, lo, mid);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">mergeSort</span>(a, mid + 1, hi);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge</span>(a, lo, mid, hi);",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">merge</span>(a: <span class=\"tok-type\">number</span>[], lo: <span class=\"tok-type\">number</span>, mid: <span class=\"tok-type\">number</span>, hi: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> tmp: <span class=\"tok-type\">number</span>[] = [];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> i = lo, j = mid + 1;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid &amp;&amp; j &lt;= hi) tmp.push(a[i] &lt;= a[j] ? a[i++] : a[j++]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid) tmp.push(a[i++]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &lt;= hi) tmp.push(a[j++]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> x = 0; x &lt; tmp.length; x++) a[lo + x] = tmp[x];",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">mergeSort</span>(a, lo, hi) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (lo &gt;= hi) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> mid = lo + Math.floor((hi - lo) / 2);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">mergeSort</span>(a, lo, mid);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">mergeSort</span>(a, mid + 1, hi);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">merge</span>(a, lo, mid, hi);",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">merge</span>(a, lo, mid, hi) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> tmp = [];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> i = lo, j = mid + 1;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid &amp;&amp; j &lt;= hi) tmp.push(a[i] &lt;= a[j] ? a[i++] : a[j++]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt;= mid) tmp.push(a[i++]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &lt;= hi) tmp.push(a[j++]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> x = 0; x &lt; tmp.length; x++) a[lo + x] = tmp[x];",
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
    var writeCount = 0;

    if (n > 1)
    {
      Split(a, 0, n - 1, n, steps, sorted, ref compareCount, ref writeCount);
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
        SwapCount = writeCount,
        ActiveCodeLines = [1, 2],
        SortedIndices = [.. sorted],
        Caption = "Hoàn tất! Dãy đã được sắp xếp."
      }
    );

    return steps;
  }

  private static void Split(
    int[] a,
    int lo,
    int hi,
    int n,
    List<SortStep> steps,
    SortedSet<int> sorted,
    ref int compareCount,
    ref int writeCount
  )
  {
    if (lo >= hi)
    {
      return;
    }

    var mid = lo + ((hi - lo) / 2);

    steps.Add(
      new SortStep
      {
        Type = StepType.SplitRange,
        Snapshot = [.. a],
        CompareCount = compareCount,
        SwapCount = writeCount,
        RangeStart = lo,
        RangeEnd = hi,
        ActiveCodeLines = [4, 5, 6],
        SortedIndices = [.. sorted],
        Caption = $"Chia [{lo}..{hi}] thành [{lo}..{mid}] và [{mid + 1}..{hi}]."
      }
    );

    Split(a, lo, mid, n, steps, sorted, ref compareCount, ref writeCount);
    Split(a, mid + 1, hi, n, steps, sorted, ref compareCount, ref writeCount);
    Merge(a, lo, mid, hi, n, steps, sorted, ref compareCount, ref writeCount);
  }

  private static void Merge(
    int[] a,
    int lo,
    int mid,
    int hi,
    int n,
    List<SortStep> steps,
    SortedSet<int> sorted,
    ref int compareCount,
    ref int writeCount
  )
  {
    var isTopLevel = lo == 0 && hi == n - 1;
    var tmp = new int[hi - lo + 1];
    int i = lo, j = mid + 1, k = 0;

    while (i <= mid && j <= hi)
    {
      compareCount++;
      var takeLeft = a[i] <= a[j];

      steps.Add(
        new SortStep
        {
          Type = StepType.MergeCompare,
          Snapshot = [.. a],
          CompareCount = compareCount,
          SwapCount = writeCount,
          LeftIndex = i,
          RightIndex = j,
          RangeStart = lo,
          RangeEnd = hi,
          ActiveCodeLines = [14, 15],
          SortedIndices = [.. sorted],
          Caption = $"So sánh a[{i}] = {a[i]} và a[{j}] = {a[j]}"
            + (takeLeft ? "  →  lấy bên trái trước" : "  →  lấy bên phải trước")
        }
      );

      tmp[k++] = takeLeft ? a[i++] : a[j++];
    }

    while (i <= mid)
    {
      tmp[k++] = a[i++];
    }

    while (j <= hi)
    {
      tmp[k++] = a[j++];
    }

    // Apply the whole merge to `a` atomically before recording any write steps.
    // Writing progressively while snapshotting the *whole* array would, for a
    // moment, show two positions holding the same value — the slot just written
    // and that value's still-unwritten former home elsewhere in [lo,hi] — which
    // breaks the @key-based bar identity (Blazor requires unique keys per render).
    // Every Snapshot handed to the UI must always be a genuine permutation.
    for (var x = 0; x < k; x++)
    {
      a[lo + x] = tmp[x];
    }

    var mergedSnapshot = a.ToArray();

    for (var x = 0; x < k; x++)
    {
      writeCount++;
      if (isTopLevel)
      {
        sorted.Add(lo + x);
      }

      steps.Add(
        new SortStep
        {
          Type = StepType.MergeWrite,
          Snapshot = mergedSnapshot,
          CompareCount = compareCount,
          SwapCount = writeCount,
          RightIndex = lo + x,
          RangeStart = lo,
          RangeEnd = hi,
          ActiveCodeLines = [19],
          SortedIndices = [.. sorted],
          Caption = $"Đã đặt {tmp[x]} vào a[{lo + x}]."
        }
      );
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.RangeDone,
        Snapshot = [.. a],
        CompareCount = compareCount,
        SwapCount = writeCount,
        RangeStart = lo,
        RangeEnd = hi,
        ActiveCodeLines = [7],
        SortedIndices = [.. sorted],
        Caption = $"Đã gộp xong [{lo}..{hi}]."
      }
    );
  }
}
