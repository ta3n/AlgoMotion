using AlgoMotion.Models;
using static AlgoMotion.Services.SearchCode;

namespace AlgoMotion.Services;

/// <summary>
/// Records Binary Search: probe the middle of the candidate range; because the array is sorted, a
/// smaller value means everything to its left is smaller still, and a bigger one rules out everything to
/// its right — so each probe throws away half the range, giving O(log n).
///
/// <code>
///  1  int binary_search(int a[], int n, int target)
///  2  {
///  3      int lo = 0, hi = n - 1;
///  4      while (lo &lt;= hi) {
///  5          int mid = lo + (hi - lo) / 2;
///  6          if (a[mid] == target) {
///  7              return mid;
///  8          }
///  9          if (a[mid] &lt; target) {
/// 10              lo = mid + 1;
/// 11          } else {
/// 12              hi = mid - 1;
/// 13          }
/// 14      }
/// 15      return -1;
/// 16  }
/// </code>
/// </summary>
public static class BinarySearchSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        $"{Ty("int")} {Fn("binary_search")}({Ty("int")} a[], {Ty("int")} n, {Ty("int")} target)",
        "{",
        L(1, $"{Ty("int")} lo = 0, hi = n - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi) {{"),
        L(2, $"{Ty("int")} mid = lo + (hi - lo) / 2;"),
        L(2, $"{Kw("if")} (a[mid] == target) {{"),
        L(3, $"{Kw("return")} mid;"),
        L(2, "}"),
        L(2, $"{Kw("if")} (a[mid] &lt; target) {{"),
        L(3, "lo = mid + 1;"),
        L(2, $"}} {Kw("else")} {{"),
        L(3, "hi = mid - 1;"),
        L(2, "}"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        "// Requires an ascending-sorted array.",
        $"{Kw("static")} {Ty("int")} {Fn("BinarySearch")}({Ty("int")}[] a, {Ty("int")} target)",
        "{",
        L(1, $"{Ty("int")} lo = 0, hi = a.Length - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi) {{"),
        L(2, $"{Ty("int")} mid = lo + (hi - lo) / 2;"),
        L(2, $"{Kw("if")} (a[mid] == target) {Kw("return")} mid;"),
        L(2, $"{Kw("if")} (a[mid] &lt; target) lo = mid + 1;"),
        L(2, $"{Kw("else")} hi = mid - 1;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.Java] =
      [
        "// Requires an ascending-sorted array.",
        $"{Kw("static")} {Ty("int")} {Fn("binarySearch")}({Ty("int")}[] a, {Ty("int")} target) {{",
        L(1, $"{Ty("int")} lo = 0, hi = a.length - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi) {{"),
        L(2, $"{Ty("int")} mid = lo + (hi - lo) / 2;"),
        L(2, $"{Kw("if")} (a[mid] == target) {Kw("return")} mid;"),
        L(2, $"{Kw("if")} (a[mid] &lt; target) lo = mid + 1;"),
        L(2, $"{Kw("else")} hi = mid - 1;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.Python] =
      [
        "# Requires an ascending-sorted list.",
        $"{Kw("def")} {Fn("binary_search")}(a, target):",
        L(1, "lo, hi = 0, len(a) - 1"),
        L(1, $"{Kw("while")} lo &lt;= hi:"),
        L(2, "mid = (lo + hi) // 2"),
        L(2, $"{Kw("if")} a[mid] == target:"),
        L(3, $"{Kw("return")} mid"),
        L(2, $"{Kw("if")} a[mid] &lt; target:"),
        L(3, "lo = mid + 1"),
        L(2, $"{Kw("else")}:"),
        L(3, "hi = mid - 1"),
        L(1, $"{Kw("return")} -1")
      ],
      [CodeLanguage.TypeScript] =
      [
        "// Requires an ascending-sorted array.",
        $"{Kw("function")} {Fn("binarySearch")}(a: {Ty("number")}[], target: {Ty("number")}): {Ty("number")} {{",
        L(1, $"{Kw("let")} lo = 0, hi = a.length - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi) {{"),
        L(2, $"{Kw("const")} mid = Math.floor((lo + hi) / 2);"),
        L(2, $"{Kw("if")} (a[mid] === target) {Kw("return")} mid;"),
        L(2, $"{Kw("if")} (a[mid] &lt; target) lo = mid + 1;"),
        L(2, $"{Kw("else")} hi = mid - 1;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        "// Requires an ascending-sorted array.",
        $"{Kw("function")} {Fn("binarySearch")}(a, target) {{",
        L(1, $"{Kw("let")} lo = 0, hi = a.length - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi) {{"),
        L(2, $"{Kw("const")} mid = Math.floor((lo + hi) / 2);"),
        L(2, $"{Kw("if")} (a[mid] === target) {Kw("return")} mid;"),
        L(2, $"{Kw("if")} (a[mid] &lt; target) lo = mid + 1;"),
        L(2, $"{Kw("else")} hi = mid - 1;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ]
    };

  public static List<SearchStep> Record(
    int[] values,
    int target,
    UiLanguage language
  )
  {
    var steps = new List<SearchStep>();
    var snapshot = StepArrays.Snapshot(values);
    var compareCount = 0;
    var narrowCount = 0;
    var lo = 0;
    var hi = values.Length - 1;

    while (lo <= hi)
    {
      var mid = lo + ((hi - lo) / 2);
      compareCount++;

      if (values[mid] == target)
      {
        steps.Add(
          new SearchStep
          {
            Type = SearchStepType.Found,
            Snapshot = snapshot,
            CheckedIndex = mid,
            RangeStart = lo,
            RangeEnd = hi,
            FoundIndex = mid,
            CompareCount = compareCount,
            NarrowCount = narrowCount,
            ActiveCodeLines = [6, 7],
            Caption = Res.Caption("Search_Found", language, mid, values[mid], target)
          }
        );

        return steps;
      }

      var goRight = values[mid] < target;
      if (goRight)
      {
        lo = mid + 1;
      }
      else
      {
        hi = mid - 1;
      }

      narrowCount++;
      steps.Add(
        new SearchStep
        {
          Type = goRight ? SearchStepType.NarrowRight : SearchStepType.NarrowLeft,
          Snapshot = snapshot,
          CheckedIndex = mid,
          RangeStart = lo,
          RangeEnd = hi,
          CompareCount = compareCount,
          NarrowCount = narrowCount,
          ActiveCodeLines = goRight ? [9, 10] : [11, 12],
          Caption = Res.Caption(
            goRight ? "Binary_NarrowRight" : "Binary_NarrowLeft",
            language,
            mid,
            values[mid],
            target,
            RangeText(lo, hi)
          )
        }
      );
    }

    steps.Add(
      new SearchStep
      {
        Type = SearchStepType.NotFound,
        Snapshot = snapshot,
        RangeStart = lo,
        RangeEnd = hi,
        CompareCount = compareCount,
        NarrowCount = narrowCount,
        ActiveCodeLines = [15],
        Caption = Res.Caption("Search_NotFound", language, target)
      }
    );

    return steps;
  }
}
