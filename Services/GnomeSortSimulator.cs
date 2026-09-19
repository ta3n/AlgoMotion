using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Gnome Sort — named after a garden gnome sorting flowerpots by
/// looking at the one behind him: if it's in order he steps forward, if not
/// he swaps and steps back to re-check. Reuses the exact same Compare/Swap
/// vocabulary as Bubble/Insertion Sort, but has no outer "pass" structure —
/// there's just a single position <c>i</c> walking forward and backward.
///
/// Unlike Bubble/Selection/Insertion Sort, no prefix or suffix is provably
/// settled at any point mid-run (a later backward walk can still touch
/// earlier positions), so <see cref="SortStep.SortedIndices"/> stays empty
/// until the final <see cref="StepType.Completed"/> step.
///
/// <code>
///  1  void gnome_sort(int a[], size_t n)
///  2  {
///  3      size_t i = 0;
///  4      while (i &lt; n) {
///  5          if (i == 0 || a[i - 1] &lt;= a[i]) {
///  6              i++;
///  7          } else {
///  8              int tmp = a[i - 1];
///  9              a[i - 1] = a[i];
/// 10              a[i] = tmp;
/// 11              i--;
/// 12          }
/// 13      }
/// 14  }
/// </code>
/// </summary>
public static class GnomeSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">gnome_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> i = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt; n) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (i == 0 || a[i - 1] &lt;= a[i]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;} <span class=\"tok-kw\">else</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[i - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i - 1] = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">GnomeSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> i = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt; a.Length) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (i == 0 || a[i - 1] &lt;= a[i]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;} <span class=\"tok-kw\">else</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[i - 1], a[i]) = (a[i], a[i - 1]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">gnomeSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> i = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt; a.length) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (i == 0 || a[i - 1] &lt;= a[i]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;} <span class=\"tok-kw\">else</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[i - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i - 1] = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">gnome_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;i = 0",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> i &lt; <span class=\"tok-fn\">len</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> i == 0 <span class=\"tok-kw\">or</span> a[i - 1] &lt;= a[i]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i += 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">else</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i - 1], a[i] = a[i], a[i - 1]",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i -= 1"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">gnomeSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> i = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt; a.length) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (i === 0 || a[i - 1] &lt;= a[i]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;} <span class=\"tok-kw\">else</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[i - 1], a[i]] = [a[i], a[i - 1]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i--;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">gnomeSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> i = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt; a.length) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (i === 0 || a[i - 1] &lt;= a[i]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;} <span class=\"tok-kw\">else</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[i - 1], a[i]] = [a[i], a[i - 1]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i--;",
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

    var i = 0;
    while (i < n)
    {
      if (i == 0)
      {
        steps.Add(
          new SortStep
          {
            Type = StepType.NoSwap,
            Snapshot = StepArrays.Snapshot(a),
            I = i,
            CompareCount = compareCount,
            SwapCount = swapCount,
            RightIndex = 0,
            ActiveCodeLines = [5, 6],
            Caption = Res.Caption("Gnome_AtStart", language)
          }
        );

        i++;
        continue;
      }

      compareCount++;
      var inOrder = a[i - 1] <= a[i];

      steps.Add(
        new SortStep
        {
          Type = StepType.Compare,
          Snapshot = StepArrays.Snapshot(a),
          I = i,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = i - 1,
          RightIndex = i,
          ActiveCodeLines = [5],
          Caption = Res.Caption(
            inOrder ? "Gnome_CompareInOrder" : "Gnome_CompareOutOfOrder",
            language,
            i - 1, a[i - 1], i, a[i]
          )
        }
      );

      if (inOrder)
      {
        i++;
      }
      else
      {
        (a[i - 1], a[i]) = (a[i], a[i - 1]);
        swapCount++;

        steps.Add(
          new SortStep
          {
            Type = StepType.Swap,
            Snapshot = StepArrays.Snapshot(a),
            I = i,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = i - 1,
            RightIndex = i,
            ActiveCodeLines = [8, 9, 10, 11],
            Caption = Res.Caption("Gnome_Swap", language, i - 1, i)
          }
        );

        i--;
      }
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
