using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Runs the optimized Bubble Sort (with the "swapped" early-exit flag) on a
/// copy of the input and records every comparison/swap as a <see cref="SortStep"/>.
///
/// This mirrors, line for line, the C reference shown in the code panel:
///
/// <code>
///  1  void bubble_sort(int a[], size_t n)
///  2  {
///  3      for (size_t i = 0; i + 1 &lt; n; i++) {
///  4          bool swapped = false;
///  5          for (size_t j = 0; j + 1 &lt; n - i; j++) {
///  6              if (a[j] &gt; a[j + 1]) {
///  7                  int tmp = a[j];
///  8                  a[j] = a[j + 1];
///  9                  a[j + 1] = tmp;
/// 10                  swapped = true;
/// 11              }
/// 12          }
/// 13          if (!swapped) break;
/// 14      }
/// 15  }
/// </code>
///
/// The UI never runs this loop itself — it only plays back the recorded
/// <see cref="SortStep"/> list, which keeps simulation and animation fully
/// decoupled.
/// </summary>
public static class BubbleSortSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">bubble_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">bool</span> swapped = <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> j = 0; j + 1 &lt; n - i; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = a[j + 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j + 1] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">BubbleSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.Length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">bool</span> swapped = <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = 0; j + 1 &lt; n - i; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(a[j], a[j + 1]) = (a[j + 1], a[j]);",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static void</span> <span class=\"tok-fn\">bubbleSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">boolean</span> swapped = <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> j = 0; j + 1 &lt; n - i; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[j];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j] = a[j + 1];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j + 1] = tmp;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">bubble_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;n = len(a)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> i <span class=\"tok-kw\">in</span> range(n - 1):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">False</span>",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> j <span class=\"tok-kw\">in</span> range(n - 1 - i):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> a[j] &gt; a[j + 1]:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[j], a[j + 1] = a[j + 1], a[j]",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">True</span>",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if not</span> swapped:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">break</span>"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">bubbleSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> swapped = <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> j = 0; j + 1 &lt; n - i; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j], a[j + 1]] = [a[j + 1], a[j]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">bubbleSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> n = a.length;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> i = 0; i + 1 &lt; n; i++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> swapped = <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> j = 0; j + 1 &lt; n - i; j++) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (a[j] &gt; a[j + 1]) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[a[j], a[j + 1]] = [a[j + 1], a[j]];",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;swapped = <span class=\"tok-kw\">true</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (!swapped) <span class=\"tok-kw\">break</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}"
    ]
  };

  public static List<SortStep> Record(
    IReadOnlyList<int> input
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
      var swapped = false;

      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = [.. a],
          I = i,
          J = -1,
          CompareCount = compareCount,
          SwapCount = swapCount,
          Swapped = false,
          ActiveCodeLines = [3, 4],
          SortedIndices = [.. sorted],
          Caption = $"Lượt {i + 1}: quét từ đầu dãy, so sánh từng cặp liền kề."
        }
      );

      for (var j = 0; j + 1 < n - i; j++)
      {
        compareCount++;
        var willSwap = a[j] > a[j + 1];

        steps.Add(
          new SortStep
          {
            Type = StepType.Compare,
            Snapshot = [.. a],
            I = i,
            J = j,
            CompareCount = compareCount,
            SwapCount = swapCount,
            Swapped = swapped,
            LeftIndex = j,
            RightIndex = j + 1,
            ActiveCodeLines = [5, 6],
            SortedIndices = [.. sorted],
            Caption = $"So sánh a[{j}] = {a[j]} và a[{j + 1}] = {a[j + 1]}"
              + (willSwap ? "  →  sai thứ tự" : "  →  đúng thứ tự")
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
              Snapshot = [.. a],
              I = i,
              J = j,
              CompareCount = compareCount,
              SwapCount = swapCount,
              Swapped = swapped,
              LeftIndex = j,
              RightIndex = j + 1,
              ActiveCodeLines = [7, 8, 9, 10],
              SortedIndices = [.. sorted],
              Caption = $"Đổi chỗ: a[{j}] ↔ a[{j + 1}]"
            }
          );
        }
        else
        {
          steps.Add(
            new SortStep
            {
              Type = StepType.NoSwap,
              Snapshot = [.. a],
              I = i,
              J = j,
              CompareCount = compareCount,
              SwapCount = swapCount,
              Swapped = swapped,
              LeftIndex = j,
              RightIndex = j + 1,
              ActiveCodeLines = [6],
              SortedIndices = [.. sorted],
              Caption = "Đã đúng thứ tự, giữ nguyên vị trí."
            }
          );
        }
      }

      var settledIndex = n - 1 - i;
      sorted.Add(settledIndex);

      steps.Add(
        new SortStep
        {
          Type = StepType.MarkSorted,
          Snapshot = [.. a],
          I = i,
          J = settledIndex,
          CompareCount = compareCount,
          SwapCount = swapCount,
          Swapped = swapped,
          RightIndex = settledIndex,
          ActiveCodeLines = [12, 13],
          SortedIndices = [.. sorted],
          Caption = $"Phần tử lớn nhất của đoạn còn lại đã về đúng vị trí a[{settledIndex}]."
        }
      );

      if (!swapped)
      {
        for (var k = 0; k < settledIndex; k++)
        {
          sorted.Add(k);
        }

        steps.Add(
          new SortStep
          {
            Type = StepType.Completed,
            Snapshot = [.. a],
            I = i,
            J = -1,
            CompareCount = compareCount,
            SwapCount = swapCount,
            Swapped = false,
            ActiveCodeLines = [13, 15],
            SortedIndices = [.. sorted],
            Caption = "Không có đổi chỗ nào trong lượt này — dừng sớm, dãy đã được sắp xếp!"
          }
        );
        return steps;
      }

      steps.Add(
        new SortStep
        {
          Type = StepType.EndPass,
          Snapshot = [.. a],
          I = i,
          J = -1,
          CompareCount = compareCount,
          SwapCount = swapCount,
          Swapped = swapped,
          ActiveCodeLines = [14],
          SortedIndices = [.. sorted],
          Caption = $"Kết thúc lượt {i + 1}."
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
        Snapshot = [.. a],
        I = n - 1,
        J = -1,
        CompareCount = compareCount,
        SwapCount = swapCount,
        Swapped = false,
        ActiveCodeLines = [15],
        SortedIndices = [.. sorted],
        Caption = "Hoàn tất! Dãy đã được sắp xếp."
      }
    );

    return steps;
  }
}
