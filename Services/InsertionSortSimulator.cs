using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Insertion Sort using its swap-based formulation: take a[i] as the
/// key and bubble it left, one adjacent swap at a time, until it meets an
/// element that's no bigger. This is behaviorally identical to the classic
/// shift-based version (same comparisons, same final order) but reuses the
/// same Compare/Swap visual language as Bubble and Selection Sort instead of
/// needing a distinct "shift" animation.
///
/// <code>
///  1  void insertion_sort(int a[], size_t n)
///  2  {
///  3      for (size_t i = 1; i &lt; n; i++) {
///  4          size_t j = i;
///  5          while (j &gt; 0 &amp;&amp; a[j - 1] &gt; a[j]) {
///  6              int tmp = a[j - 1];
///  7              a[j - 1] = a[j];
///  8              a[j] = tmp;
///  9              j--;
/// 10          }
/// 11      }
/// 12  }
/// </code>
/// </summary>
public static class InsertionSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">insertion_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 1; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt; 0 &amp;&amp; a[j - 1] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - 1] = a[j];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">InsertionSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.Length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = 1; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt; 0 &amp;&amp; a[j - 1] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[j - 1], a[j]) = (a[j], a[j - 1]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">insertionSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = 1; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt; 0 &amp;&amp; a[j - 1] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - 1] = a[j];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">insertion_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;n = len(a)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> i <span class=\"tok-kw\">in</span> range(1, n):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j = i",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> j &gt; 0 <span class=\"tok-kw\">and</span> a[j - 1] &gt; a[j]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - 1], a[j] = a[j], a[j - 1]",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j -= 1"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">insertionSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 1; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt; 0 &amp;&amp; a[j - 1] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j - 1], a[j]] = [a[j], a[j - 1]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">insertionSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 1; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt; 0 &amp;&amp; a[j - 1] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j - 1], a[j]] = [a[j], a[j - 1]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
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

    var compareCount = 0;
    var swapCount = 0;

    for (var i = 1; i < n; i++)
    {
      var j = i;

      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = StepArrays.Snapshot(a),
          I = i,
          J = j,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = i,
          ActiveCodeLines = [3, 4],
          SortedIndices = StepArrays.Prefix(i),
          Caption = Res.Caption("Insertion_StartPass", language, i, a[i])
        }
      );

      while (j > 0)
      {
        compareCount++;
        var outOfOrder = a[j - 1] > a[j];

        steps.Add(
          new SortStep
          {
            Type = StepType.Compare,
            Snapshot = StepArrays.Snapshot(a),
            I = i,
            J = j,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = j - 1,
            RightIndex = j,
            ActiveCodeLines = [5],
            SortedIndices = StepArrays.Prefix(i),
            Caption = Res.Caption(
              outOfOrder ? "Insertion_CompareOutOfOrder" : "Insertion_CompareInOrder",
              language,
              j - 1, a[j - 1], j, a[j]
            )
          }
        );

        if (!outOfOrder)
        {
          break;
        }

        (a[j - 1], a[j]) = (a[j], a[j - 1]);
        swapCount++;

        steps.Add(
          new SortStep
          {
            Type = StepType.Swap,
            Snapshot = StepArrays.Snapshot(a),
            I = i,
            J = j,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = j - 1,
            RightIndex = j,
            ActiveCodeLines = [6, 7, 8],
            SortedIndices = StepArrays.Prefix(i),
            Caption = Res.Caption("Insertion_Swap", language, j - 1, j)
          }
        );

        j--;
      }

      steps.Add(
        new SortStep
        {
          Type = StepType.EndPass,
          Snapshot = StepArrays.Snapshot(a),
          I = i,
          CompareCount = compareCount,
          SwapCount = swapCount,
          ActiveCodeLines = [11],
          SortedIndices = StepArrays.Prefix(i + 1),
          Caption = Res.Caption("Insertion_EndPass", language, i)
        }
      );
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.Completed,
        Snapshot = StepArrays.Snapshot(a),
        I = n - 1,
        CompareCount = compareCount,
        SwapCount = swapCount,
        ActiveCodeLines = [12],
        SortedIndices = StepArrays.Prefix(n),
        Caption = Res.Caption("Common_SortCompleted", language)
      }
    );

    return steps;
  }
}
