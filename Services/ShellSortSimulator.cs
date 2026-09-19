using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Shell Sort using the classic shrinking-gap sequence (n/2, n/4, ...,
/// 1). Each pass is exactly an Insertion Sort pass, except neighbours are
/// <c>gap</c> positions apart instead of adjacent — so this reuses the same
/// swap-based Compare/Swap steps as <see cref="InsertionSortSimulator"/>
/// almost line for line, just with <c>j - 1</c> replaced by <c>j - gap</c>.
///
/// Unlike Insertion Sort, a gapped pass doesn't leave a clean sorted prefix
/// behind (only the final gap = 1 pass does), so <see cref="SortStep.SortedIndices"/>
/// stays empty until the very last <see cref="StepType.Completed"/> step.
///
/// <code>
///  1  void shell_sort(int a[], size_t n)
///  2  {
///  3      for (size_t gap = n / 2; gap &gt; 0; gap /= 2) {
///  4          for (size_t i = gap; i &lt; n; i++) {
///  5              size_t j = i;
///  6              while (j &gt;= gap &amp;&amp; a[j - gap] &gt; a[j]) {
///  7                  int tmp = a[j - gap];
///  8                  a[j - gap] = a[j];
///  9                  a[j] = tmp;
/// 10                  j -= gap;
/// 11              }
/// 12          }
/// 13      }
/// 14  }
/// </code>
/// </summary>
public static class ShellSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">shell_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> gap = n / 2; gap &gt; 0; gap /= 2) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = gap; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt;= gap &amp;&amp; a[j - gap] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j - gap];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - gap] = a[j];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j -= gap;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">ShellSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.Length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> gap = n / 2; gap &gt; 0; gap /= 2) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = gap; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt;= gap &amp;&amp; a[j - gap] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[j - gap], a[j]) = (a[j], a[j - gap]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j -= gap;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">shellSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> gap = n / 2; gap &gt; 0; gap /= 2) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = gap; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt;= gap &amp;&amp; a[j - gap] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j - gap];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - gap] = a[j];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j -= gap;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">shell_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;n = len(a)",
      "&nbsp;&nbsp;&nbsp;&nbsp;gap = n // 2",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> gap &gt; 0:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> i <span class=\"tok-kw\">in</span> range(gap, n):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j = i",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> j &gt;= gap <span class=\"tok-kw\">and</span> a[j - gap] &gt; a[j]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - gap], a[j] = a[j], a[j - gap]",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j -= gap",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;gap //= 2"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">shellSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> gap = Math.floor(n / 2); gap &gt; 0; gap = Math.floor(gap / 2)) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = gap; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt;= gap &amp;&amp; a[j - gap] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j - gap], a[j]] = [a[j], a[j - gap]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j -= gap;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">shellSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> gap = Math.floor(n / 2); gap &gt; 0; gap = Math.floor(gap / 2)) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = gap; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> j = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (j &gt;= gap &amp;&amp; a[j - gap] &gt; a[j]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j - gap], a[j]] = [a[j], a[j - gap]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;j -= gap;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
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

    for (var gap = n / 2; gap > 0; gap /= 2)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = StepArrays.Snapshot(a),
          CompareCount = compareCount,
          SwapCount = swapCount,
          ActiveCodeLines = [3],
          Caption = Res.Caption("Shell_StartPass", language, gap)
        }
      );

      for (var i = gap; i < n; i++)
      {
        var j = i;

        while (j >= gap)
        {
          compareCount++;
          var outOfOrder = a[j - gap] > a[j];

          steps.Add(
            new SortStep
            {
              Type = StepType.Compare,
              Snapshot = StepArrays.Snapshot(a),
              I = i,
              J = j,
              CompareCount = compareCount,
              SwapCount = swapCount,
              LeftIndex = j - gap,
              RightIndex = j,
              ActiveCodeLines = [6],
              Caption = Res.Caption(
                outOfOrder ? "Shell_CompareOutOfOrder" : "Shell_CompareInOrder",
                language,
                j - gap, a[j - gap], j, a[j]
              )
            }
          );

          if (!outOfOrder)
          {
            break;
          }

          (a[j - gap], a[j]) = (a[j], a[j - gap]);
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
              LeftIndex = j - gap,
              RightIndex = j,
              ActiveCodeLines = [7, 8, 9],
              Caption = Res.Caption("Shell_Swap", language, j - gap, j)
            }
          );

          j -= gap;
        }
      }

      steps.Add(
        new SortStep
        {
          Type = StepType.EndPass,
          Snapshot = StepArrays.Snapshot(a),
          CompareCount = compareCount,
          SwapCount = swapCount,
          ActiveCodeLines = [12, 13],
          Caption = Res.Caption("Shell_EndPass", language, gap)
        }
      );
    }

    var allSorted = new int[n];
    for (var k = 0; k < n; k++)
    {
      allSorted[k] = k;
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.Completed,
        Snapshot = StepArrays.Snapshot(a),
        CompareCount = compareCount,
        SwapCount = swapCount,
        ActiveCodeLines = [1, 2],
        SortedIndices = allSorted,
        Caption = Res.Caption("Common_SortCompleted", language)
      }
    );

    return steps;
  }
}
