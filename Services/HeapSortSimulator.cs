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
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
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
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">HeapSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.Length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = n / 2 - 1; i &gt;= 0; i--) <span class=\"tok-fn\">SiftDown</span>(a, n, i);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> end = n - 1; end &gt; 0; end--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[0], a[end]) = (a[end], a[0]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">SiftDown</span>(a, end, 0);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">SiftDown</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> size, <span class=\"tok-type\">int</span> root)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (<span class=\"tok-kw\">true</span>) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> largest = root, left = (2 * root) + 1, right = (2 * root) + 2;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (left &lt; size &amp;&amp; a[left] &gt; a[largest]) largest = left;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (right &lt; size &amp;&amp; a[right] &gt; a[largest]) largest = right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (largest == root) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[root], a[largest]) = (a[largest], a[root]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root = largest;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">heapSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = n / 2 - 1; i &gt;= 0; i--) <span class=\"tok-fn\">siftDown</span>(a, n, i);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> end = n - 1; end &gt; 0; end--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[0]; a[0] = a[end]; a[end] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">siftDown</span>(a, end, 0);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">siftDown</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> size, <span class=\"tok-type\">int</span> root) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (<span class=\"tok-kw\">true</span>) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> largest = root, left = 2 * root + 1, right = 2 * root + 2;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (left &lt; size &amp;&amp; a[left] &gt; a[largest]) largest = left;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (right &lt; size &amp;&amp; a[right] &gt; a[largest]) largest = right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (largest == root) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[root]; a[root] = a[largest]; a[largest] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root = largest;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">heap_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;n = len(a)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> i <span class=\"tok-kw\">in</span> range(n // 2 - 1, -1, -1):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">sift_down</span>(a, n, i)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> end <span class=\"tok-kw\">in</span> range(n - 1, 0, -1):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[0], a[end] = a[end], a[0]",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">sift_down</span>(a, end, 0)",
      "",
      "",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">sift_down</span>(a, size, root):",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while True</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;largest, left, right = root, 2 * root + 1, 2 * root + 2",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> left &lt; size <span class=\"tok-kw\">and</span> a[left] &gt; a[largest]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;largest = left",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> right &lt; size <span class=\"tok-kw\">and</span> a[right] &gt; a[largest]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;largest = right",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> largest == root:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span>",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[root], a[largest] = a[largest], a[root]",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root = largest"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">heapSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = Math.floor(n / 2) - 1; i &gt;= 0; i--) <span class=\"tok-fn\">siftDown</span>(a, n, i);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> end = n - 1; end &gt; 0; end--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[0], a[end]] = [a[end], a[0]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">siftDown</span>(a, end, 0);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">siftDown</span>(a: <span class=\"tok-type\">number</span>[], size: <span class=\"tok-type\">number</span>, root: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (<span class=\"tok-kw\">true</span>) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> largest = root;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> left = 2 * root + 1, right = 2 * root + 2;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (left &lt; size &amp;&amp; a[left] &gt; a[largest]) largest = left;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (right &lt; size &amp;&amp; a[right] &gt; a[largest]) largest = right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (largest === root) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[root], a[largest]] = [a[largest], a[root]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root = largest;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">heapSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = Math.floor(n / 2) - 1; i &gt;= 0; i--) <span class=\"tok-fn\">siftDown</span>(a, n, i);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> end = n - 1; end &gt; 0; end--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[0], a[end]] = [a[end], a[0]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">siftDown</span>(a, end, 0);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">siftDown</span>(a, size, root) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (<span class=\"tok-kw\">true</span>) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> largest = root;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> left = 2 * root + 1, right = 2 * root + 2;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (left &lt; size &amp;&amp; a[left] &gt; a[largest]) largest = left;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (right &lt; size &amp;&amp; a[right] &gt; a[largest]) largest = right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (largest === root) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[root], a[largest]] = [a[largest], a[root]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root = largest;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ]
  };

  public static List<SortStep> Record(
    IReadOnlyList<int> input,
    UiLanguage language
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
      SiftDown(a, n, i, steps, sorted, ref compareCount, ref swapCount, language);
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
          Snapshot = StepArrays.Snapshot(a),
          J = end,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = 0,
          RightIndex = end,
          RangeStart = 0,
          RangeEnd = end,
          ActiveCodeLines = [6, 7],
          SortedIndices = StepArrays.Sorted(sorted),
          Caption = Res.Caption("Heap_ExtractSwap", language, end)
        }
      );

      SiftDown(a, end, 0, steps, sorted, ref compareCount, ref swapCount, language);
    }

    for (var k = 0; k < n; k++)
    {
      sorted.Add(k);
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.Completed,
        Snapshot = StepArrays.Snapshot(a),
        CompareCount = compareCount,
        SwapCount = swapCount,
        ActiveCodeLines = [1, 2],
        SortedIndices = StepArrays.Sorted(sorted),
        Caption = Res.Caption("Common_SortCompleted", language)
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
    ref int swapCount,
    UiLanguage language
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
            Snapshot = StepArrays.Snapshot(a),
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
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption(
              leftBigger ? "Heap_CompareLeftBigger" : "Heap_CompareLeftNotBigger",
              language,
              left, a[left], largest, a[largest]
            )
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
            Snapshot = StepArrays.Snapshot(a),
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
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption(
              rightBigger ? "Heap_CompareRightBigger" : "Heap_CompareRightNotBigger",
              language,
              right, a[right], largest, a[largest]
            )
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
            Snapshot = StepArrays.Snapshot(a),
            I = root,
            CompareCount = compareCount,
            SwapCount = swapCount,
            PivotIndex = root,
            RangeStart = 0,
            RangeEnd = size - 1,
            ActiveCodeLines = [19],
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption("Heap_NoSwapStop", language, root)
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
          Snapshot = StepArrays.Snapshot(a),
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
          SortedIndices = StepArrays.Sorted(sorted),
          Caption = Res.Caption("Heap_SiftSwap", language, root, largest)
        }
      );

      root = largest;
    }
  }
}
