using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Radix Sort (LSD, base 10): repeatedly runs a stable counting sort
/// keyed by one decimal digit at a time, starting from the units digit and
/// working up to the most significant digit of the largest value.
///
/// Each digit pass is structurally identical to <see cref="CountingSortSimulator"/>
/// — tally digit occurrences, turn them into cumulative positions, then place
/// every element directly into its slot for this pass — so it reuses the same
/// <see cref="StepType.CountTally"/>/<see cref="StepType.EndPass"/>/<see cref="StepType.CountPlace"/>
/// vocabulary and stays off the crane (<see cref="SortAlgorithmInfo.ShowCrane"/> = false)
/// for the same reason: there's no stable pair of positions being compared.
///
/// A pass only sorts by its own digit, so earlier passes' order can still be
/// disturbed by a later, more significant digit — <see cref="SortStep.SortedIndices"/>
/// stays empty until the very last <see cref="StepType.Completed"/> step, once
/// every digit has been processed.
///
/// <code>
///  1  void radix_sort(int a[], size_t n)
///  2  {
///  3      int max = a[0];
///  4      for (size_t i = 1; i &lt; n; i++) {
///  5          if (a[i] &gt; max) max = a[i];
///  6      }
///  7      for (int exp = 1; max / exp &gt; 0; exp *= 10) {
///  8          counting_sort_by_digit(a, n, exp);
///  9      }
/// 10  }
/// 11
/// 12  void counting_sort_by_digit(int a[], size_t n, int exp)
/// 13  {
/// 14      int output[n];
/// 15      int count[10] = {0};
/// 16      for (size_t i = 0; i &lt; n; i++) {
/// 17          count[(a[i] / exp) % 10]++;
/// 18      }
/// 19      for (int d = 1; d &lt; 10; d++) {
/// 20          count[d] += count[d - 1];
/// 21      }
/// 22      for (int i = (int)n - 1; i &gt;= 0; i--) {
/// 23          output[--count[(a[i] / exp) % 10]] = a[i];
/// 24      }
/// 25      for (size_t i = 0; i &lt; n; i++) {
/// 26          a[i] = output[i];
/// 27      }
/// 28  }
/// </code>
/// </summary>
public static class RadixSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">radix_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> max = a[0];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 1; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &gt; max) max = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> exp = 1; max / exp &gt; 0; exp *= 10) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">counting_sort_by_digit</span>(a, n, exp);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">counting_sort_by_digit</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n, <span class=\"tok-type\">int</span> exp)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> output[n];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> count[10] = {0};",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 0; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[(a[i] / exp) % 10]++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> d = 1; d &lt; 10; d++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[d] += count[d - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = (<span class=\"tok-type\">int</span>)n - 1; i &gt;= 0; i--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[(a[i] / exp) % 10]] = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 0; i &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i] = output[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.CSharp] =
    [
      "// Sorts non-negative integers.",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">void</span> <span class=\"tok-fn\">RadixSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a.Length &lt; 2) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> max = a[0];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">foreach</span> (<span class=\"tok-type\">int</span> value <span class=\"tok-kw\">in</span> a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &gt; max) max = value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">long</span> exp = 1; max / exp &gt; 0; exp *= 10) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">CountingSortByDigit</span>(a, exp);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">void</span> <span class=\"tok-fn\">CountingSortByDigit</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">long</span> exp)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] output = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[a.Length];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] count = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[10];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">foreach</span> (<span class=\"tok-type\">int</span> value <span class=\"tok-kw\">in</span> a) count[(<span class=\"tok-type\">int</span>)(value / exp % 10)]++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> d = 1; d &lt; 10; d++) count[d] += count[d - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = a.Length - 1; i &gt;= 0; i--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> digit = (<span class=\"tok-type\">int</span>)(a[i] / exp % 10);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[digit]] = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;output.<span class=\"tok-fn\">CopyTo</span>(a, 0);",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "// Sorts non-negative integers.",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">void</span> <span class=\"tok-fn\">radixSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a.length &lt; 2) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> max = a[0];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> value : a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &gt; max) max = value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">long</span> exp = 1; max / exp &gt; 0; exp *= 10) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">countingSortByDigit</span>(a, exp);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">void</span> <span class=\"tok-fn\">countingSortByDigit</span>(<span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">long</span> exp) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] output = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[a.length];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span>[] count = <span class=\"tok-kw\">new</span> <span class=\"tok-type\">int</span>[10];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> value : a) count[(<span class=\"tok-type\">int</span>)(value / exp % 10)]++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> d = 1; d &lt; 10; d++) count[d] += count[d - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = a.length - 1; i &gt;= 0; i--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> digit = (<span class=\"tok-type\">int</span>)(a[i] / exp % 10);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[digit]] = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;System.<span class=\"tok-fn\">arraycopy</span>(output, 0, a, 0, a.length);",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "# Sorts non-negative integers.",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">radix_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> <span class=\"tok-fn\">len</span>(a) &lt; 2:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span>",
      "&nbsp;&nbsp;&nbsp;&nbsp;maximum = <span class=\"tok-fn\">max</span>(a)",
      "&nbsp;&nbsp;&nbsp;&nbsp;exp = 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> maximum // exp &gt; 0:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">counting_sort_by_digit</span>(a, exp)",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;exp *= 10",
      "",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">counting_sort_by_digit</span>(a, exp):",
      "&nbsp;&nbsp;&nbsp;&nbsp;output = [0] * <span class=\"tok-fn\">len</span>(a)",
      "&nbsp;&nbsp;&nbsp;&nbsp;count = [0] * 10",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> value <span class=\"tok-kw\">in</span> a:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[value // exp % 10] += 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> d <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">range</span>(1, 10):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[d] += count[d - 1]",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> value <span class=\"tok-kw\">in</span> <span class=\"tok-fn\">reversed</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;digit = value // exp % 10",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[digit] -= 1",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[count[digit]] = value",
      "&nbsp;&nbsp;&nbsp;&nbsp;a[:] = output"
    ],
    [CodeLanguage.TypeScript] =
    [
      "// Sorts non-negative integers.",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">radixSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-kw\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a.length &lt; 2) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> max = a[0];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &gt; max) max = value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> exp = 1; Math.<span class=\"tok-fn\">floor</span>(max / exp) &gt; 0; exp *= 10) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">countingSortByDigit</span>(a, exp);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">countingSortByDigit</span>(a: <span class=\"tok-type\">number</span>[], exp: <span class=\"tok-type\">number</span>): <span class=\"tok-kw\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> output = <span class=\"tok-kw\">new</span> Array&lt;<span class=\"tok-type\">number</span>&gt;(a.length);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> count = <span class=\"tok-kw\">new</span> Array&lt;<span class=\"tok-type\">number</span>&gt;(10).<span class=\"tok-fn\">fill</span>(0);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) count[Math.<span class=\"tok-fn\">floor</span>(value / exp) % 10]++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> d = 1; d &lt; 10; d++) count[d] += count[d - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = a.length - 1; i &gt;= 0; i--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> digit = Math.<span class=\"tok-fn\">floor</span>(a[i] / exp) % 10;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[digit]] = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 0; i &lt; a.length; i++) a[i] = output[i];",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "// Sorts non-negative integers.",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">radixSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a.length &lt; 2) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> max = a[0];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &gt; max) max = value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> exp = 1; Math.<span class=\"tok-fn\">floor</span>(max / exp) &gt; 0; exp *= 10) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">countingSortByDigit</span>(a, exp);",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">countingSortByDigit</span>(a, exp) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> output = <span class=\"tok-kw\">new</span> <span class=\"tok-fn\">Array</span>(a.length);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> count = <span class=\"tok-kw\">new</span> <span class=\"tok-fn\">Array</span>(10).<span class=\"tok-fn\">fill</span>(0);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) count[Math.<span class=\"tok-fn\">floor</span>(value / exp) % 10]++;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> d = 1; d &lt; 10; d++) count[d] += count[d - 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = a.length - 1; i &gt;= 0; i--) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> digit = Math.<span class=\"tok-fn\">floor</span>(a[i] / exp) % 10;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[digit]] = a[i];",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 0; i &lt; a.length; i++) a[i] = output[i];",
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

    var tallyCount = 0;
    var writeCount = 0;

    if (n == 0)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.Completed,
          Snapshot = [],
          ActiveCodeLines = [1, 2],
          Caption = Res.Caption("Common_SortCompleted", language)
        }
      );

      return steps;
    }

    var max = a.Max();

    for (var exp = 1; max / exp > 0; exp *= 10)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = [.. a],
          CompareCount = tallyCount,
          SwapCount = writeCount,
          ActiveCodeLines = [7],
          SortedIndices = [.. sorted],
          Caption = Res.Caption("Radix_StartPass", language, DigitPlaceName(exp, language), exp)
        }
      );

      var count = new int[10];

      for (var i = 0; i < n; i++)
      {
        tallyCount++;
        var digit = (a[i] / exp) % 10;
        count[digit]++;

        steps.Add(
          new SortStep
          {
            Type = StepType.CountTally,
            Snapshot = [.. a],
            I = i,
            CompareCount = tallyCount,
            SwapCount = writeCount,
            LeftIndex = i,
            ActiveCodeLines = [16, 17],
            SortedIndices = [.. sorted],
            Caption = Res.Caption("Radix_Tally", language, i, a[i], digit, count[digit])
          }
        );
      }

      for (var d = 1; d < 10; d++)
      {
        count[d] += count[d - 1];
      }

      steps.Add(
        new SortStep
        {
          Type = StepType.EndPass,
          Snapshot = [.. a],
          CompareCount = tallyCount,
          SwapCount = writeCount,
          ActiveCodeLines = [19, 20],
          SortedIndices = [.. sorted],
          Caption = Res.Caption("Radix_Prefix", language)
        }
      );

      var output = new int[n];
      var placements = new List<(int SourceIndex, int Target, int Value)>();

      for (var i = n - 1; i >= 0; i--)
      {
        var digit = (a[i] / exp) % 10;
        var target = --count[digit];
        output[target] = a[i];
        placements.Add((i, target, a[i]));
      }

      foreach (var (sourceIndex, target, value) in placements)
      {
        writeCount++;

        steps.Add(
          new SortStep
          {
            Type = StepType.CountPlace,
            Snapshot = output,
            CompareCount = tallyCount,
            SwapCount = writeCount,
            RightIndex = target,
            ActiveCodeLines = [22, 23],
            SortedIndices = [.. sorted],
            Caption = Res.Caption("Radix_Place", language, value, sourceIndex, target)
          }
        );
      }

      a = output;
    }

    for (var k = 0; k < n; k++)
    {
      sorted.Add(k);
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.Completed,
        Snapshot = a,
        CompareCount = tallyCount,
        SwapCount = writeCount,
        ActiveCodeLines = [1, 2],
        SortedIndices = [.. sorted],
        Caption = Res.Caption("Common_SortCompleted", language)
      }
    );

    return steps;
  }

  private static string DigitPlaceName(
    int exp,
    UiLanguage language
  )
  {
    return exp switch
    {
      1 => Res.Caption("Radix_PlaceOnes", language),
      10 => Res.Caption("Radix_PlaceTens", language),
      100 => Res.Caption("Radix_PlaceHundreds", language),
      1000 => Res.Caption("Radix_PlaceThousands", language),
      _ => Res.Caption("Radix_PlacePowerOfTen", language, (int)Math.Log10(exp))
    };
  }
}
