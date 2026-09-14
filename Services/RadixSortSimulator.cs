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
  public static readonly string[] CodeLines =
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
  ];

  public static List<SortStep> Record(
    IReadOnlyList<int> input
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
          Caption = "Hoàn tất! Dãy đã được sắp xếp."
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
          Caption = $"Sắp xếp theo {DigitPlaceName(exp)} (exp = {exp})."
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
            Caption = $"Đếm a[{i}] = {a[i]} (chữ số {digit}): count[{digit}] = {count[digit]}."
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
          Caption = "Cộng dồn count[] để biết vị trí cuối cùng của mỗi chữ số."
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
            Caption = $"Đặt {value} (từ a[{sourceIndex}]) vào vị trí a[{target}]."
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
        Caption = "Hoàn tất! Dãy đã được sắp xếp."
      }
    );

    return steps;
  }

  private static string DigitPlaceName(
    int exp
  )
  {
    return exp switch
    {
      1 => "hàng đơn vị",
      10 => "hàng chục",
      100 => "hàng trăm",
      1000 => "hàng nghìn",
      _ => $"hàng 10^{(int)Math.Log10(exp)}"
    };
  }
}
