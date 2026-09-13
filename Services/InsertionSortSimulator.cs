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
    public static readonly string[] CodeLines =
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
    ];

    public static List<SortStep> Record(IReadOnlyList<int> input)
    {
        var a = input.ToArray();
        var n = a.Length;
        var steps = new List<SortStep>();

        var compareCount = 0;
        var swapCount = 0;

        for (var i = 1; i < n; i++)
        {
            var j = i;

            steps.Add(new SortStep
            {
                Type = StepType.StartPass,
                Snapshot = [.. a],
                I = i,
                J = j,
                CompareCount = compareCount,
                SwapCount = swapCount,
                LeftIndex = i,
                ActiveCodeLines = [3, 4],
                SortedIndices = Range(0, i),
                Caption = $"Lấy a[{i}] = {a[i]} làm khoá, dịch dần về đúng chỗ trong đoạn đã sắp xếp."
            });

            while (j > 0)
            {
                compareCount++;
                var outOfOrder = a[j - 1] > a[j];

                steps.Add(new SortStep
                {
                    Type = StepType.Compare,
                    Snapshot = [.. a],
                    I = i,
                    J = j,
                    CompareCount = compareCount,
                    SwapCount = swapCount,
                    LeftIndex = j - 1,
                    RightIndex = j,
                    ActiveCodeLines = [5],
                    SortedIndices = Range(0, i),
                    Caption = $"So sánh a[{j - 1}] = {a[j - 1]} và a[{j}] = {a[j]}" +
                              (outOfOrder ? "  →  sai thứ tự" : "  →  đúng thứ tự, dừng")
                });

                if (!outOfOrder) break;

                (a[j - 1], a[j]) = (a[j], a[j - 1]);
                swapCount++;

                steps.Add(new SortStep
                {
                    Type = StepType.Swap,
                    Snapshot = [.. a],
                    I = i,
                    J = j,
                    CompareCount = compareCount,
                    SwapCount = swapCount,
                    LeftIndex = j - 1,
                    RightIndex = j,
                    ActiveCodeLines = [6, 7, 8],
                    SortedIndices = Range(0, i),
                    Caption = $"Đổi chỗ: a[{j - 1}] ↔ a[{j}]."
                });

                j--;
            }

            steps.Add(new SortStep
            {
                Type = StepType.EndPass,
                Snapshot = [.. a],
                I = i,
                CompareCount = compareCount,
                SwapCount = swapCount,
                ActiveCodeLines = [11],
                SortedIndices = Range(0, i + 1),
                Caption = $"Đoạn a[0..{i}] đã được sắp xếp."
            });
        }

        steps.Add(new SortStep
        {
            Type = StepType.Completed,
            Snapshot = [.. a],
            I = n - 1,
            CompareCount = compareCount,
            SwapCount = swapCount,
            ActiveCodeLines = [12],
            SortedIndices = Range(0, n),
            Caption = "Hoàn tất! Dãy đã được sắp xếp."
        });

        return steps;
    }

    private static int[] Range(int start, int endExclusive)
    {
        var result = new int[Math.Max(0, endExclusive - start)];
        for (var k = 0; k < result.Length; k++) result[k] = start + k;
        return result;
    }
}
