using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Cocktail Shaker Sort — bidirectional Bubble Sort. Each round makes
/// a forward pass (bubbling the largest remaining element to the top of the
/// range) followed by a backward pass (bubbling the smallest remaining
/// element to the bottom), shrinking the active [lo, hi] window from both
/// ends every round instead of only from the top like plain Bubble Sort.
///
/// Reuses <see cref="BubbleSortSimulator"/>'s exact step vocabulary
/// (StartPass/Compare/Swap/NoSwap/MarkSorted/Completed) for both passes —
/// only the direction and which end gets marked sorted differ.
///
/// <code>
///  1  void cocktail_sort(int a[], size_t n)
///  2  {
///  3      size_t lo = 0, hi = n - 1;
///  4      while (lo &lt; hi) {
///  5          bool swapped = false;
///  6          for (size_t j = lo; j &lt; hi; j++) {
///  7              if (a[j] &gt; a[j + 1]) {
///  8                  int tmp = a[j];
///  9                  a[j] = a[j + 1];
/// 10                  a[j + 1] = tmp;
/// 11                  swapped = true;
/// 12              }
/// 13          }
/// 14          hi--;
/// 15          for (size_t j = hi; j &gt; lo; j--) {
/// 16              if (a[j - 1] &gt; a[j]) {
/// 17                  int tmp = a[j - 1];
/// 18                  a[j - 1] = a[j];
/// 19                  a[j] = tmp;
/// 20                  swapped = true;
/// 21              }
/// 22          }
/// 23          lo++;
/// 24          if (!swapped) break;
/// 25      }
/// 26  }
/// </code>
/// </summary>
public static class CocktailShakerSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">cocktail_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> lo = 0, hi = n - 1;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (lo &lt; hi) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">bool</span> swapped = <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> j = lo; j &lt; hi; j++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = a[j + 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j + 1] = tmp;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;hi--;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> j = hi; j &gt; lo; j--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j - 1] &gt; a[j]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j - 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - 1] = a[j];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = tmp;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lo++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">void</span> <span class=\"tok-fn\">CocktailSort</span>(<span class=\"tok-type\">int</span>[] a)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> lo = 0, hi = a.Length - 1;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (lo &lt; hi) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">bool</span> swapped = <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = lo; j &lt; hi; j++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[j], a[j + 1]) = (a[j + 1], a[j]);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;hi--;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = hi; j &gt; lo; j--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j - 1] &gt; a[j]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[j - 1], a[j]) = (a[j], a[j - 1]);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lo++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.Java] =
      [
        "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">void</span> <span class=\"tok-fn\">cocktailSort</span>(<span class=\"tok-type\">int</span>[] a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> lo = 0, hi = a.length - 1;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (lo &lt; hi) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">boolean</span> swapped = <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = lo; j &lt; hi; j++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = a[j + 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j + 1] = tmp;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;hi--;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = hi; j &gt; lo; j--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j - 1] &gt; a[j]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j - 1];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - 1] = a[j];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = tmp;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lo++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.Python] =
      [
        "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">cocktail_sort</span>(a):",
        "&nbsp;&nbsp;&nbsp;&nbsp;lo, hi = 0, <span class=\"tok-fn\">len</span>(a) - 1",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> lo &lt; hi:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">False</span>",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> j <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">range</span>(lo, hi):",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> a[j] &gt; a[j + 1]:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j], a[j + 1] = a[j + 1], a[j]",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">True</span>",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;hi -= 1",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> j <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">range</span>(hi, lo, -1):",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> a[j - 1] &gt; a[j]:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j - 1], a[j] = a[j], a[j - 1]",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">True</span>",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lo += 1",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> <span class=\"tok-kw\">not</span> swapped:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">break</span>"
      ],
      [CodeLanguage.TypeScript] =
      [
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">cocktailSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-kw\">void</span> {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> lo = 0, hi = a.length - 1;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (lo &lt; hi) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> swapped = <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> j = lo; j &lt; hi; j++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j], a[j + 1]] = [a[j + 1], a[j]];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;hi--;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> j = hi; j &gt; lo; j--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j - 1] &gt; a[j]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j - 1], a[j]] = [a[j], a[j - 1]];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lo++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">cocktailSort</span>(a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> lo = 0, hi = a.length - 1;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (lo &lt; hi) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> swapped = <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> j = lo; j &lt; hi; j++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j], a[j + 1]] = [a[j + 1], a[j]];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;hi--;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> j = hi; j &gt; lo; j--) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j - 1] &gt; a[j]) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j - 1], a[j]] = [a[j], a[j - 1]];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;lo++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
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

    var lo = 0;
    var hi = n - 1;

    while (lo < hi)
    {
      var swapped = false;

      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = StepArrays.Snapshot(a),
          I = lo,
          J = hi,
          CompareCount = compareCount,
          SwapCount = swapCount,
          ActiveCodeLines = [5, 6],
          SortedIndices = StepArrays.Sorted(sorted),
          Caption = Res.Caption("Cocktail_ForwardPass", language, lo, hi)
        }
      );

      for (var j = lo; j < hi; j++)
      {
        compareCount++;
        var willSwap = a[j] > a[j + 1];

        steps.Add(
          new SortStep
          {
            Type = StepType.Compare,
            Snapshot = StepArrays.Snapshot(a),
            I = lo,
            J = j,
            CompareCount = compareCount,
            SwapCount = swapCount,
            Swapped = swapped,
            LeftIndex = j,
            RightIndex = j + 1,
            ActiveCodeLines = [7],
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption(
              willSwap ? "Bubble_CompareOutOfOrder" : "Bubble_CompareInOrder",
              language,
              j,
              a[j],
              j + 1,
              a[j + 1]
            )
          }
        );

        if (willSwap)
        {
          (a[j], a[j + 1]) = (a[j + 1], a[j]);
          swapCount++;
          swapped = true;

          steps.Add(
            new SortStep
            {
              Type = StepType.Swap,
              Snapshot = StepArrays.Snapshot(a),
              I = lo,
              J = j,
              CompareCount = compareCount,
              SwapCount = swapCount,
              Swapped = true,
              LeftIndex = j,
              RightIndex = j + 1,
              ActiveCodeLines = [8, 9, 10, 11],
              SortedIndices = StepArrays.Sorted(sorted),
              Caption = Res.Caption("Bubble_Swap", language, j, j + 1)
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
              I = lo,
              J = j,
              CompareCount = compareCount,
              SwapCount = swapCount,
              Swapped = swapped,
              LeftIndex = j,
              RightIndex = j + 1,
              ActiveCodeLines = [7],
              SortedIndices = StepArrays.Sorted(sorted),
              Caption = Res.Caption("Bubble_NoSwap", language)
            }
          );
        }
      }

      sorted.Add(hi);

      steps.Add(
        new SortStep
        {
          Type = StepType.MarkSorted,
          Snapshot = StepArrays.Snapshot(a),
          I = lo,
          J = hi,
          CompareCount = compareCount,
          SwapCount = swapCount,
          Swapped = swapped,
          RightIndex = hi,
          ActiveCodeLines = [14],
          SortedIndices = StepArrays.Sorted(sorted),
          Caption = Res.Caption("Cocktail_MarkSortedMax", language, hi)
        }
      );

      hi--;

      if (lo < hi)
      {
        steps.Add(
          new SortStep
          {
            Type = StepType.StartPass,
            Snapshot = StepArrays.Snapshot(a),
            I = lo,
            J = hi,
            CompareCount = compareCount,
            SwapCount = swapCount,
            ActiveCodeLines = [15],
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption("Cocktail_BackwardPass", language, hi, lo)
          }
        );

        for (var j = hi; j > lo; j--)
        {
          compareCount++;
          var willSwap = a[j - 1] > a[j];

          steps.Add(
            new SortStep
            {
              Type = StepType.Compare,
              Snapshot = StepArrays.Snapshot(a),
              I = lo,
              J = j,
              CompareCount = compareCount,
              SwapCount = swapCount,
              Swapped = swapped,
              LeftIndex = j - 1,
              RightIndex = j,
              ActiveCodeLines = [16],
              SortedIndices = StepArrays.Sorted(sorted),
              Caption = Res.Caption(
                willSwap ? "Bubble_CompareOutOfOrder" : "Bubble_CompareInOrder",
                language,
                j - 1,
                a[j - 1],
                j,
                a[j]
              )
            }
          );

          if (willSwap)
          {
            (a[j - 1], a[j]) = (a[j], a[j - 1]);
            swapCount++;
            swapped = true;

            steps.Add(
              new SortStep
              {
                Type = StepType.Swap,
                Snapshot = StepArrays.Snapshot(a),
                I = lo,
                J = j,
                CompareCount = compareCount,
                SwapCount = swapCount,
                Swapped = true,
                LeftIndex = j - 1,
                RightIndex = j,
                ActiveCodeLines = [17, 18, 19, 20],
                SortedIndices = StepArrays.Sorted(sorted),
                Caption = Res.Caption("Bubble_Swap", language, j - 1, j)
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
                I = lo,
                J = j,
                CompareCount = compareCount,
                SwapCount = swapCount,
                Swapped = swapped,
                LeftIndex = j - 1,
                RightIndex = j,
                ActiveCodeLines = [16],
                SortedIndices = StepArrays.Sorted(sorted),
                Caption = Res.Caption("Bubble_NoSwap", language)
              }
            );
          }
        }

        sorted.Add(lo);

        steps.Add(
          new SortStep
          {
            Type = StepType.MarkSorted,
            Snapshot = StepArrays.Snapshot(a),
            I = lo,
            J = hi,
            CompareCount = compareCount,
            SwapCount = swapCount,
            Swapped = swapped,
            LeftIndex = lo,
            ActiveCodeLines = [23],
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption("Cocktail_MarkSortedMin", language, lo)
          }
        );

        lo++;
      }

      if (!swapped)
      {
        for (var k = lo; k <= hi; k++)
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
            ActiveCodeLines = [24, 26],
            SortedIndices = StepArrays.Sorted(sorted),
            Caption = Res.Caption("Bubble_EarlyStop", language)
          }
        );

        return steps;
      }
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
        ActiveCodeLines = [26],
        SortedIndices = StepArrays.Sorted(sorted),
        Caption = Res.Caption("Common_SortCompleted", language)
      }
    );

    return steps;
  }
}
