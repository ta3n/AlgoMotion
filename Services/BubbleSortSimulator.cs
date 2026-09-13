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
    public static List<SortStep> Record(IReadOnlyList<int> input)
    {
        var a = input.ToArray();
        int n = a.Length;
        var steps = new List<SortStep>();
        var sorted = new SortedSet<int>();

        int compareCount = 0;
        int swapCount = 0;

        for (int i = 0; i + 1 < n; i++)
        {
            bool swapped = false;

            steps.Add(new SortStep
            {
                Type = StepType.StartPass,
                Snapshot = a.ToArray(),
                I = i,
                J = -1,
                CompareCount = compareCount,
                SwapCount = swapCount,
                Swapped = false,
                ActiveCodeLines = [3, 4],
                SortedIndices = sorted.ToArray(),
                Caption = $"Lượt {i + 1}: quét từ đầu dãy, so sánh từng cặp liền kề."
            });

            for (int j = 0; j + 1 < n - i; j++)
            {
                compareCount++;
                bool willSwap = a[j] > a[j + 1];

                steps.Add(new SortStep
                {
                    Type = StepType.Compare,
                    Snapshot = a.ToArray(),
                    I = i,
                    J = j,
                    CompareCount = compareCount,
                    SwapCount = swapCount,
                    Swapped = swapped,
                    LeftIndex = j,
                    RightIndex = j + 1,
                    ActiveCodeLines = [5, 6],
                    SortedIndices = sorted.ToArray(),
                    Caption = $"So sánh a[{j}] = {a[j]} và a[{j + 1}] = {a[j + 1]}" +
                              (willSwap ? "  →  sai thứ tự" : "  →  đúng thứ tự")
                });

                if (willSwap)
                {
                    (a[j], a[j + 1]) = (a[j + 1], a[j]);
                    swapCount++;
                    swapped = true;

                    steps.Add(new SortStep
                    {
                        Type = StepType.Swap,
                        Snapshot = a.ToArray(),
                        I = i,
                        J = j,
                        CompareCount = compareCount,
                        SwapCount = swapCount,
                        Swapped = swapped,
                        LeftIndex = j,
                        RightIndex = j + 1,
                        ActiveCodeLines = [7, 8, 9, 10],
                        SortedIndices = sorted.ToArray(),
                        Caption = $"Đổi chỗ: a[{j}] ↔ a[{j + 1}]"
                    });
                }
                else
                {
                    steps.Add(new SortStep
                    {
                        Type = StepType.NoSwap,
                        Snapshot = a.ToArray(),
                        I = i,
                        J = j,
                        CompareCount = compareCount,
                        SwapCount = swapCount,
                        Swapped = swapped,
                        LeftIndex = j,
                        RightIndex = j + 1,
                        ActiveCodeLines = [6],
                        SortedIndices = sorted.ToArray(),
                        Caption = "Đã đúng thứ tự, giữ nguyên vị trí."
                    });
                }
            }

            int settledIndex = n - 1 - i;
            sorted.Add(settledIndex);

            steps.Add(new SortStep
            {
                Type = StepType.MarkSorted,
                Snapshot = a.ToArray(),
                I = i,
                J = settledIndex,
                CompareCount = compareCount,
                SwapCount = swapCount,
                Swapped = swapped,
                RightIndex = settledIndex,
                ActiveCodeLines = [12, 13],
                SortedIndices = sorted.ToArray(),
                Caption = $"Phần tử lớn nhất của đoạn còn lại đã về đúng vị trí a[{settledIndex}]."
            });

            if (!swapped)
            {
                for (int k = 0; k < settledIndex; k++) sorted.Add(k);

                steps.Add(new SortStep
                {
                    Type = StepType.Completed,
                    Snapshot = a.ToArray(),
                    I = i,
                    J = -1,
                    CompareCount = compareCount,
                    SwapCount = swapCount,
                    Swapped = false,
                    ActiveCodeLines = [13, 15],
                    SortedIndices = sorted.ToArray(),
                    Caption = "Không có đổi chỗ nào trong lượt này — dừng sớm, dãy đã được sắp xếp!"
                });
                return steps;
            }

            steps.Add(new SortStep
            {
                Type = StepType.EndPass,
                Snapshot = a.ToArray(),
                I = i,
                J = -1,
                CompareCount = compareCount,
                SwapCount = swapCount,
                Swapped = swapped,
                ActiveCodeLines = [14],
                SortedIndices = sorted.ToArray(),
                Caption = $"Kết thúc lượt {i + 1}."
            });
        }

        for (int k = 0; k < n; k++) sorted.Add(k);

        steps.Add(new SortStep
        {
            Type = StepType.Completed,
            Snapshot = a.ToArray(),
            I = n - 1,
            J = -1,
            CompareCount = compareCount,
            SwapCount = swapCount,
            Swapped = false,
            ActiveCodeLines = [15],
            SortedIndices = sorted.ToArray(),
            Caption = "Hoàn tất! Dãy đã được sắp xếp."
        });

        return steps;
    }
}
