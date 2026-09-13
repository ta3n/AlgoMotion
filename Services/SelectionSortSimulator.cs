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
    public static readonly string[] CodeLines =
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
    ];

    public static List<SortStep> Record(IReadOnlyList<int> input)
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

            steps.Add(new SortStep
            {
                Type = StepType.StartPass,
                Snapshot = [.. a],
                I = i,
                J = -1,
                CompareCount = compareCount,
                SwapCount = swapCount,
                PivotIndex = min,
                ActiveCodeLines = [3, 4],
                SortedIndices = [.. sorted],
                Caption = $"Lượt {i + 1}: giả sử a[{i}] là nhỏ nhất, đi tìm phần tử nhỏ hơn."
            });

            for (var j = i + 1; j < n; j++)
            {
                compareCount++;
                var smaller = a[j] < a[min];

                steps.Add(new SortStep
                {
                    Type = StepType.Compare,
                    Snapshot = [.. a],
                    I = i,
                    J = j,
                    CompareCount = compareCount,
                    SwapCount = swapCount,
                    LeftIndex = min,
                    RightIndex = j,
                    PivotIndex = min,
                    ActiveCodeLines = [5, 6],
                    SortedIndices = [.. sorted],
                    Caption = $"So sánh a[{j}] = {a[j]} với a[min] = {a[min]}" +
                              (smaller ? "  →  nhỏ hơn!" : "  →  không nhỏ hơn")
                });

                if (smaller)
                {
                    min = j;
                    steps.Add(new SortStep
                    {
                        Type = StepType.NewCandidate,
                        Snapshot = [.. a],
                        I = i,
                        J = j,
                        CompareCount = compareCount,
                        SwapCount = swapCount,
                        LeftIndex = i,
                        RightIndex = min,
                        PivotIndex = min,
                        ActiveCodeLines = [7],
                        SortedIndices = [.. sorted],
                        Caption = $"a[{j}] = {a[j]} nhỏ hơn — min cập nhật thành {j}."
                    });
                }
            }

            if (min != i)
            {
                (a[i], a[min]) = (a[min], a[i]);
                swapCount++;

                steps.Add(new SortStep
                {
                    Type = StepType.Swap,
                    Snapshot = [.. a],
                    I = i,
                    J = min,
                    CompareCount = compareCount,
                    SwapCount = swapCount,
                    LeftIndex = i,
                    RightIndex = min,
                    ActiveCodeLines = [10, 11, 12, 13],
                    SortedIndices = [.. sorted],
                    Caption = $"Đổi chỗ a[{i}] và a[{min}] — phần tử nhỏ nhất về đầu."
                });
            }
            else
            {
                steps.Add(new SortStep
                {
                    Type = StepType.NoSwap,
                    Snapshot = [.. a],
                    I = i,
                    J = min,
                    CompareCount = compareCount,
                    SwapCount = swapCount,
                    LeftIndex = i,
                    ActiveCodeLines = [10],
                    SortedIndices = [.. sorted],
                    Caption = $"a[{i}] đã là nhỏ nhất, không cần đổi chỗ."
                });
            }

            sorted.Add(i);

            steps.Add(new SortStep
            {
                Type = StepType.MarkSorted,
                Snapshot = [.. a],
                I = i,
                CompareCount = compareCount,
                SwapCount = swapCount,
                RightIndex = i,
                ActiveCodeLines = [14, 15],
                SortedIndices = [.. sorted],
                Caption = $"a[{i}] đã về đúng vị trí cuối cùng."
            });
        }

        for (var k = 0; k < n; k++) sorted.Add(k);

        steps.Add(new SortStep
        {
            Type = StepType.Completed,
            Snapshot = [.. a],
            I = n - 1,
            CompareCount = compareCount,
            SwapCount = swapCount,
            ActiveCodeLines = [16],
            SortedIndices = [.. sorted],
            Caption = "Hoàn tất! Dãy đã được sắp xếp."
        });

        return steps;
    }
}
