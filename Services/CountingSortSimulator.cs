using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Counting Sort — the odd one out among these simulators: it never
/// compares two array elements to each other. Instead it tallies how many
/// times each value occurs, turns those tallies into cumulative positions,
/// then places every element directly into its final slot.
///
/// Because there's no stable "pair being compared" to point a crane at,
/// <see cref="SortAlgorithmInfo.ShowCrane"/> is false (same reasoning as
/// Merge Sort). <see cref="SortAlgorithmInfo.CompareLabel"/>/<see cref="SortAlgorithmInfo.ActionLabel"/>
/// are repurposed as "tally reads" / "writes" counters instead of
/// "comparisons" / "swaps".
///
/// The whole output array is computed first, then replayed one placement at
/// a time against that single, already-final snapshot — the same trick
/// <see cref="MergeSortSimulator"/> uses to keep every <see cref="SortStep.Snapshot"/>
/// a genuine permutation (required for Blazor's @key-based bar identity).
/// Placements are visited in the same back-to-front order the algorithm
/// actually uses (needed for stability), so which bar lights up next jumps
/// around rather than sweeping left to right — a deliberate contrast with
/// the comparison-based sorts.
///
/// <code>
///  1  void counting_sort(int a[], size_t n)
///  2  {
///  3      int min = a[0], max = a[0];
///  4      for (size_t i = 1; i &lt; n; i++) {
///  5          if (a[i] &lt; min) min = a[i];
///  6          if (a[i] &gt; max) max = a[i];
///  7      }
///  8
///  9      int range = max - min + 1;
/// 10      int count[range];
/// 11      memset(count, 0, sizeof(count));
/// 12      for (size_t i = 0; i &lt; n; i++) {
/// 13          count[a[i] - min]++;
/// 14      }
/// 15
/// 16      for (int v = 1; v &lt; range; v++) {
/// 17          count[v] += count[v - 1];
/// 18      }
/// 19
/// 20      int output[n];
/// 21      for (int i = (int)n - 1; i &gt;= 0; i--) {
/// 22          output[--count[a[i] - min]] = a[i];
/// 23      }
/// 24
/// 25      for (size_t i = 0; i &lt; n; i++) {
/// 26          a[i] = output[i];
/// 27      }
/// 28  }
/// </code>
/// </summary>
public static class CountingSortSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">counting_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> min = a[0], max = a[0];",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 1; i &lt; n; i++) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &lt; min) min = a[i];",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[i] &gt; max) max = a[i];",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> range = max - min + 1;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> count[range];",
    "&nbsp;&nbsp;&nbsp;&nbsp;memset(count, 0, <span class=\"tok-kw\">sizeof</span>(count));",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 0; i &lt; n; i++) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[a[i] - min]++;",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> v = 1; v &lt; range; v++) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;count[v] += count[v - 1];",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> output[n];",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = (<span class=\"tok-type\">int</span>)n - 1; i &gt;= 0; i--) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;output[--count[a[i] - min]] = a[i];",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "",
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

    var min = a.Min();
    var max = a.Max();
    var range = max - min + 1;
    var count = new int[range];

    for (var i = 0; i < n; i++)
    {
      tallyCount++;
      count[a[i] - min]++;

      steps.Add(
        new SortStep
        {
          Type = StepType.CountTally,
          Snapshot = [.. a],
          I = i,
          CompareCount = tallyCount,
          SwapCount = writeCount,
          LeftIndex = i,
          ActiveCodeLines = [12, 13],
          SortedIndices = [.. sorted],
          Caption = $"Đếm a[{i}] = {a[i]}: count[{a[i] - min}] = {count[a[i] - min]}."
        }
      );
    }

    for (var v = 1; v < range; v++)
    {
      count[v] += count[v - 1];
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.EndPass,
        Snapshot = [.. a],
        CompareCount = tallyCount,
        SwapCount = writeCount,
        ActiveCodeLines = [16, 17],
        SortedIndices = [.. sorted],
        Caption = "Cộng dồn count[] để biết vị trí cuối cùng của mỗi giá trị trong mảng kết quả."
      }
    );

    var output = new int[n];
    var placements = new List<(int SourceIndex, int Target, int Value)>();

    for (var i = n - 1; i >= 0; i--)
    {
      var target = --count[a[i] - min];
      output[target] = a[i];
      placements.Add((i, target, a[i]));
    }

    foreach (var (sourceIndex, target, value) in placements)
    {
      writeCount++;
      sorted.Add(target);

      steps.Add(
        new SortStep
        {
          Type = StepType.CountPlace,
          Snapshot = output,
          CompareCount = tallyCount,
          SwapCount = writeCount,
          RightIndex = target,
          ActiveCodeLines = [21, 22],
          SortedIndices = [.. sorted],
          Caption = $"Đặt {value} (từ a[{sourceIndex}] ban đầu) vào vị trí a[{target}]."
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
        Snapshot = output,
        CompareCount = tallyCount,
        SwapCount = writeCount,
        ActiveCodeLines = [1, 2],
        SortedIndices = [.. sorted],
        Caption = "Hoàn tất! Dãy đã được sắp xếp."
      }
    );

    return steps;
  }
}
