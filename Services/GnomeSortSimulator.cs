using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Gnome Sort — named after a garden gnome sorting flowerpots by
/// looking at the one behind him: if it's in order he steps forward, if not
/// he swaps and steps back to re-check. Reuses the exact same Compare/Swap
/// vocabulary as Bubble/Insertion Sort, but has no outer "pass" structure —
/// there's just a single position <c>i</c> walking forward and backward.
///
/// Unlike Bubble/Selection/Insertion Sort, no prefix or suffix is provably
/// settled at any point mid-run (a later backward walk can still touch
/// earlier positions), so <see cref="SortStep.SortedIndices"/> stays empty
/// until the final <see cref="StepType.Completed"/> step.
///
/// <code>
///  1  void gnome_sort(int a[], size_t n)
///  2  {
///  3      size_t i = 0;
///  4      while (i &lt; n) {
///  5          if (i == 0 || a[i - 1] &lt;= a[i]) {
///  6              i++;
///  7          } else {
///  8              int tmp = a[i - 1];
///  9              a[i - 1] = a[i];
/// 10              a[i] = tmp;
/// 11              i--;
/// 12          }
/// 13      }
/// 14  }
/// </code>
/// </summary>
public static class GnomeSortSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">gnome_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">size_t</span> i = 0;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (i &lt; n) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (i == 0 || a[i - 1] &lt;= a[i]) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i++;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;} <span class=\"tok-kw\">else</span> {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> tmp = a[i - 1];",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i - 1] = a[i];",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a[i] = tmp;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;i--;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
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

    var compareCount = 0;
    var swapCount = 0;

    var i = 0;
    while (i < n)
    {
      if (i == 0)
      {
        steps.Add(
          new SortStep
          {
            Type = StepType.NoSwap,
            Snapshot = [.. a],
            I = i,
            CompareCount = compareCount,
            SwapCount = swapCount,
            RightIndex = 0,
            ActiveCodeLines = [5, 6],
            Caption = "i = 0: chưa có phần tử phía trước để so sánh, tiến lên (i++)."
          }
        );

        i++;
        continue;
      }

      compareCount++;
      var inOrder = a[i - 1] <= a[i];

      steps.Add(
        new SortStep
        {
          Type = StepType.Compare,
          Snapshot = [.. a],
          I = i,
          CompareCount = compareCount,
          SwapCount = swapCount,
          LeftIndex = i - 1,
          RightIndex = i,
          ActiveCodeLines = [5],
          Caption = $"So sánh a[{i - 1}] = {a[i - 1]} và a[{i}] = {a[i]}"
            + (inOrder ? "  →  đúng thứ tự, tiến lên (i++)" : "  →  sai thứ tự, đổi chỗ rồi lùi lại (i--)")
        }
      );

      if (inOrder)
      {
        i++;
      }
      else
      {
        (a[i - 1], a[i]) = (a[i], a[i - 1]);
        swapCount++;

        steps.Add(
          new SortStep
          {
            Type = StepType.Swap,
            Snapshot = [.. a],
            I = i,
            CompareCount = compareCount,
            SwapCount = swapCount,
            LeftIndex = i - 1,
            RightIndex = i,
            ActiveCodeLines = [8, 9, 10, 11],
            Caption = $"Đổi chỗ: a[{i - 1}] ↔ a[{i}], lùi lại kiểm tra tiếp."
          }
        );

        i--;
      }
    }

    var allSorted = new int[n];
    for (var k = 0; k < n; k++)
    {
      allSorted[k] = k;
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.Completed,
        Snapshot = [.. a],
        CompareCount = compareCount,
        SwapCount = swapCount,
        ActiveCodeLines = [1, 2],
        SortedIndices = allSorted,
        Caption = "Hoàn tất! Dãy đã được sắp xếp."
      }
    );

    return steps;
  }
}
