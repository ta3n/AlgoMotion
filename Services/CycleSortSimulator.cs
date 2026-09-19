using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Cycle Sort — the algorithm that minimizes the number of swaps: it
/// follows each permutation cycle directly, moving every element to its
/// final resting place instead of bubbling elements past each other.
///
/// The textbook formulation holds the value being placed in a separate
/// <c>item</c> variable outside the array, leaving the origin slot stale
/// until the cycle closes — which means the array temporarily holds a
/// duplicate (and is missing the held value) between writes. That's
/// incompatible with this app's requirement that every <see cref="SortStep.Snapshot"/>
/// be a genuine permutation (Blazor's @key-based bar identity depends on it,
/// same constraint noted in <see cref="MergeSortSimulator"/>).
///
/// So this recorder uses the mathematically equivalent "decompose the cycle
/// into transpositions" formulation instead: always swap <c>a[start]</c>
/// directly with its computed target <c>a[pos]</c>, comparing against
/// <c>a[start]</c> itself rather than a floating value. A cycle of length L
/// still only takes L-1 real swaps this way (each element other than the one
/// already correctly placed moves exactly once), so the "very few operations"
/// point still holds, and every Snapshot stays a true permutation — which
/// also means a stable pair is swapped every time, so
/// <see cref="SortAlgorithmInfo.ShowCrane"/> stays true.
///
/// <code>
///  1  void cycle_sort(int a[], size_t n)
///  2  {
///  3      for (size_t start = 0; start + 1 &lt; n; start++) {
///  4          size_t pos = start;
///  5          for (size_t i = start + 1; i &lt; n; i++) {
///  6              if (a[i] &lt; a[start]) pos++;
///  7          }
///  8          if (pos == start) continue;
///  9          while (a[pos] == a[start]) pos++; swap(&amp;a[start], &amp;a[pos]);
/// 10          while (pos != start) {
/// 11              pos = start;
/// 12              for (size_t i = start + 1; i &lt; n; i++) {
/// 13                  if (a[i] &lt; a[start]) pos++;
/// 14              }
/// 15              if (pos != start) {
/// 16                  while (a[pos] == a[start]) pos++; swap(&amp;a[start], &amp;a[pos]);
/// 17              }
/// 18          }
/// 19      }
/// 20  }
/// </code>
/// </summary>
public static class CycleSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">cycle_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> start = 0; start + 1 &lt; n; start++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> pos = start;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = start + 1; i &lt; n; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (pos == start) <span class=\"tok-kw\">continue</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;while (a[pos] == a[start]) pos++; swap(&amp;a[start], &amp;a[pos]);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (pos != start) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;pos = start;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = start + 1; i &lt; n; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (pos != start) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;while (a[pos] == a[start]) pos++; swap(&amp;a[start], &amp;a[pos]);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">CycleSort</span>(<span class=\"tok-type\">int</span>[] a)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> start = 0; start + 1 &lt; a.Length; start++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (<span class=\"tok-kw\">true</span>) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> pos = start;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = start + 1; i &lt; a.Length; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (pos == start) <span class=\"tok-kw\">break</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (a[pos] == a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[start], a[pos]) = (a[pos], a[start]);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.Java] =
      [
        "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">cycleSort</span>(<span class=\"tok-type\">int</span>[] a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> start = 0; start + 1 &lt; a.length; start++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (<span class=\"tok-kw\">true</span>) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> pos = start;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = start + 1; i &lt; a.length; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (pos == start) <span class=\"tok-kw\">break</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (a[pos] == a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[start];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[start] = a[pos];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[pos] = tmp;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.Python] =
      [
        "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">cycle_sort</span>(a):",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> start <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">range</span>(<span class=\"tok-fn\">len</span>(a) - 1):",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> <span class=\"tok-kw\">True</span>:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;pos = start",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> i <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">range</span>(start + 1, <span class=\"tok-fn\">len</span>(a)):",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> a[i] &lt; a[start]:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;pos += 1",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> pos == start:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">break</span>",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> a[pos] == a[start]:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;pos += 1",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[start], a[pos] = a[pos], a[start]"
      ],
      [CodeLanguage.TypeScript] =
      [
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">cycleSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> start = 0; start + 1 &lt; a.length; start++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (<span class=\"tok-kw\">true</span>) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> pos = start;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = start + 1; i &lt; a.length; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (pos === start) <span class=\"tok-kw\">break</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (a[pos] === a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[start], a[pos]] = [a[pos], a[start]];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">cycleSort</span>(a) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> start = 0; start + 1 &lt; a.length; start++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (<span class=\"tok-kw\">true</span>) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> pos = start;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = start + 1; i &lt; a.length; i++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (pos === start) <span class=\"tok-kw\">break</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (a[pos] === a[start]) pos++;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[start], a[pos]] = [a[pos], a[start]];",
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

    for (var start = 0; start + 1 < n; start++)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = StepArrays.Snapshot(a),
          I = start,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = start,
          SortedIndices = StepArrays.Sorted(sorted),
          ActiveCodeLines = [3, 4],
          Caption = Res.Caption("Cycle_StartPass", language, start, start, a[start])
        }
      );

      var pos = ScanForRank(a, start, steps, sorted, ref compareCount, swapCount, [5, 6], language);

      if (pos == start)
      {
        sorted.Add(start);

        steps.Add(
          new SortStep
          {
            Type = StepType.NoSwap,
            Snapshot = StepArrays.Snapshot(a),
            I = start,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = start,
            SortedIndices = StepArrays.Sorted(sorted),
            ActiveCodeLines = [8],
            Caption = Res.Caption("Cycle_AlreadyPlaced", language, start)
          }
        );

        continue;
      }

      while (a[pos] == a[start])
      {
        pos++;
      }

      (a[start], a[pos]) = (a[pos], a[start]);
      swapCount++;
      sorted.Add(pos);

      steps.Add(
        new SortStep
        {
          Type = StepType.Swap,
          Snapshot = StepArrays.Snapshot(a),
          I = start,
          J = pos,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = start,
          RightIndex = pos,
          SortedIndices = StepArrays.Sorted(sorted),
          ActiveCodeLines = [9],
          Caption = Res.Caption("Cycle_SwapFinal", language, start, pos, a[pos])
        }
      );

      while (pos != start)
      {
        pos = ScanForRank(a, start, steps, sorted, ref compareCount, swapCount, [12, 13], language);

        if (pos != start)
        {
          while (a[pos] == a[start])
          {
            pos++;
          }

          (a[start], a[pos]) = (a[pos], a[start]);
          swapCount++;
          sorted.Add(pos);

          steps.Add(
            new SortStep
            {
              Type = StepType.Swap,
              Snapshot = StepArrays.Snapshot(a),
              I = start,
              J = pos,
              CompareCount = compareCount,
              SwapCount = swapCount,
              LeftIndex = start,
              RightIndex = pos,
              SortedIndices = StepArrays.Sorted(sorted),
              ActiveCodeLines = [15, 16],
              Caption = Res.Caption("Cycle_SwapFinal", language, start, pos, a[pos])
            }
          );
        }
      }

      sorted.Add(start);

      steps.Add(
        new SortStep
        {
          Type = StepType.MarkSorted,
          Snapshot = StepArrays.Snapshot(a),
          I = start,
          CompareCount = compareCount,
          SwapCount = swapCount,
          RightIndex = start,
          SortedIndices = StepArrays.Sorted(sorted),
          ActiveCodeLines = [10],
          Caption = Res.Caption("Cycle_CycleClosed", language, start, a[start])
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
        CompareCount = compareCount,
        SwapCount = swapCount,
        SortedIndices = StepArrays.Sorted(sorted),
        ActiveCodeLines = [1, 2],
        Caption = Res.Caption("Common_SortCompleted", language)
      }
    );

    return steps;
  }

  /// <summary>Counts how many elements after <paramref name="start"/> are smaller than the
  /// current a[start], recording one Compare step per element scanned. The result is
  /// a[start]'s correct final rank within [start, n-1] given the array's current state.</summary>
  private static int ScanForRank(
    int[] a,
    int start,
    List<SortStep> steps,
    SortedSet<int> sorted,
    ref int compareCount,
    int swapCount,
    int[] activeCodeLines,
    UiLanguage language
  )
  {
    var pos = start;

    for (var i = start + 1; i < a.Length; i++)
    {
      compareCount++;
      var lessThanStart = a[i] < a[start];

      steps.Add(
        new SortStep
        {
          Type = StepType.Compare,
          Snapshot = StepArrays.Snapshot(a),
          I = start,
          J = i,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = start,
          RightIndex = i,
          SortedIndices = StepArrays.Sorted(sorted),
          ActiveCodeLines = activeCodeLines,
          Caption = Res.Caption(
            lessThanStart ? "Cycle_ScanLess" : "Cycle_ScanNotLess",
            language,
            i,
            a[i],
            start,
            a[start]
          )
        }
      );

      if (lessThanStart)
      {
        pos++;
      }
    }

    return pos;
  }
}
