using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Selection Sort: each pass scans the unsorted suffix for its
/// smallest element (tracked as <see cref="SortStep.PivotIndex"/>, drawn as a
/// gold ring) and swaps it into place at the front, once per pass.
///
/// <code>
///  1  void selection_sort(int a[], size_t n)
///  2  {
///  3      for (size_t i = 0; i + 1 &lt; n; i++) {
///  4          size_t min = i;
///  5          for (size_t j = i + 1; j &lt; n; j++) {
///  6              if (a[j] &lt; a[min]) {
///  7                  min = j;
///  8              }
///  9          }
/// 10          if (min != i) {
/// 11              int tmp = a[i];
/// 12              a[i] = a[min];
/// 13              a[min] = tmp;
/// 14          }
/// 15      }
/// 16  }
/// </code>
/// </summary>
public static class SelectionSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">selection_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> min = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> j = i + 1; j &lt; n; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &lt; a[min]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;min = j;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (min != i) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i] = a[min];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[min] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">SelectionSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.Length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> min = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = i + 1; j &lt; n; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &lt; a[min]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;min = j;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (min != i) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[i], a[min]) = (a[min], a[i]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">selectionSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> min = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = i + 1; j &lt; n; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &lt; a[min]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;min = j;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (min != i) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i] = a[min];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[min] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">selection_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;n = len(a)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> i <span class=\"tok-kw\">in</span> range(n - 1):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;min_idx = i",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> j <span class=\"tok-kw\">in</span> range(i + 1, n):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> a[j] &lt; a[min_idx]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;min_idx = j",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> min_idx != i:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i], a[min_idx] = a[min_idx], a[i]"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">selectionSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> min = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> j = i + 1; j &lt; n; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &lt; a[min]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;min = j;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (min !== i) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[i], a[min]] = [a[min], a[i]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">selectionSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> min = i;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> j = i + 1; j &lt; n; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &lt; a[min]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;min = j;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (min !== i) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[i], a[min]] = [a[min], a[i]];",
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
    var sorted = new SortedSet<int>();

    var compareCount = 0;
    var swapCount = 0;

    for (var i = 0; i + 1 < n; i++)
    {
      var min = i;

      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = StepArrays.Snapshot(a),
          I = i,
          J = -1,
          CompareCount = compareCount,
          SwapCount = swapCount,
          PivotIndex = min,
          ActiveCodeLines = [3, 4],
          SortedIndices = StepArrays.Sorted(sorted),
          Caption = Res.Caption("Selection_StartPass", language, i + 1, i)
        }
      );

      for (var j = i + 1; j < n; j++)
      {
        compareCount++;
        var smaller = a[j] < a[min];

        steps.Add(
          new SortStep
          {
            Type = StepType.Compare,
            Snapshot = StepArrays.Snapshot(a),
            I = i,
            J = j,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = min,
            RightIndex = j,
            PivotIndex = min,
            ActiveCodeLines = [5, 6],
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption(
              smaller ? "Selection_CompareSmaller" : "Selection_CompareNotSmaller",
              language,
              j, a[j], a[min]
            )
          }
        );

        if (smaller)
        {
          min = j;
          steps.Add(
            new SortStep
            {
              Type = StepType.NewCandidate,
              Snapshot = StepArrays.Snapshot(a),
              I = i,
              J = j,
              CompareCount = compareCount,
              SwapCount = swapCount,
              LeftIndex = i,
              RightIndex = min,
              PivotIndex = min,
              ActiveCodeLines = [7],
              SortedIndices = StepArrays.Sorted(sorted),
              Caption = Res.Caption("Selection_NewCandidate", language, j, a[j], j)
            }
          );
        }
      }

      if (min != i)
      {
        (a[i], a[min]) = (a[min], a[i]);
        swapCount++;

        steps.Add(
          new SortStep
          {
            Type = StepType.Swap,
            Snapshot = StepArrays.Snapshot(a),
            I = i,
            J = min,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = i,
            RightIndex = min,
            ActiveCodeLines = [10, 11, 12, 13],
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption("Selection_Swap", language, i, min)
          }
        );
      }
      else
      {
        steps.Add(
          new SortStep
          {
            Type = StepType.NoSwap,
            Snapshot = StepArrays.Snapshot(a),
            I = i,
            J = min,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = i,
            ActiveCodeLines = [10],
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption("Selection_NoSwap", language, i)
          }
        );
      }

      sorted.Add(i);

      steps.Add(
        new SortStep
        {
          Type = StepType.MarkSorted,
          Snapshot = StepArrays.Snapshot(a),
          I = i,
          CompareCount = compareCount,
          SwapCount = swapCount,
          RightIndex = i,
          ActiveCodeLines = [14, 15],
          SortedIndices = StepArrays.Sorted(sorted),
          Caption = Res.Caption("Selection_MarkSorted", language, i)
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
        Snapshot = StepArrays.Snapshot(a),
        I = n - 1,
        CompareCount = compareCount,
        SwapCount = swapCount,
        ActiveCodeLines = [16],
        SortedIndices = StepArrays.Sorted(sorted),
        Caption = Res.Caption("Common_SortCompleted", language)
      }
    );

    return steps;
  }
}
