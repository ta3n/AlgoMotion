using AlgoMotion.Models;
using static AlgoMotion.Services.SearchCode;

namespace AlgoMotion.Services;

/// <summary>
/// Records Linear Search: look at every element in order until the target turns up. It needs no
/// ordering at all, which is exactly why it is the O(n) baseline the other searches improve on — the
/// visualizer still feeds it the same ascending array so the four algorithms can be compared on one input.
///
/// <code>
///  1  int linear_search(int a[], int n, int target)
///  2  {
///  3      for (int i = 0; i &lt; n; i++) {
///  4          if (a[i] == target) {
///  5              return i;
///  6          }
///  7      }
///  8      return -1;
///  9  }
/// </code>
/// </summary>
public static class LinearSearchSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        $"{Ty("int")} {Fn("linear_search")}({Ty("int")} a[], {Ty("int")} n, {Ty("int")} target)",
        "{",
        L(1, $"{Kw("for")} ({Ty("int")} i = 0; i &lt; n; i++) {{"),
        L(2, $"{Kw("if")} (a[i] == target) {{"),
        L(3, $"{Kw("return")} i;"),
        L(2, "}"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        $"{Kw("static")} {Ty("int")} {Fn("LinearSearch")}({Ty("int")}[] a, {Ty("int")} target)",
        "{",
        L(1, $"{Kw("for")} ({Kw("var")} i = 0; i &lt; a.Length; i++) {{"),
        L(2, $"{Kw("if")} (a[i] == target) {Kw("return")} i;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.Java] =
      [
        $"{Kw("static")} {Ty("int")} {Fn("linearSearch")}({Ty("int")}[] a, {Ty("int")} target) {{",
        L(1, $"{Kw("for")} ({Ty("int")} i = 0; i &lt; a.length; i++) {{"),
        L(2, $"{Kw("if")} (a[i] == target) {Kw("return")} i;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.Python] =
      [
        $"{Kw("def")} {Fn("linear_search")}(a, target):",
        L(1, $"{Kw("for")} i, value {Kw("in")} enumerate(a):"),
        L(2, $"{Kw("if")} value == target:"),
        L(3, $"{Kw("return")} i"),
        L(1, $"{Kw("return")} -1")
      ],
      [CodeLanguage.TypeScript] =
      [
        $"{Kw("function")} {Fn("linearSearch")}(a: {Ty("number")}[], target: {Ty("number")}): {Ty("number")} {{",
        L(1, $"{Kw("for")} ({Kw("let")} i = 0; i &lt; a.length; i++) {{"),
        L(2, $"{Kw("if")} (a[i] === target) {Kw("return")} i;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        $"{Kw("function")} {Fn("linearSearch")}(a, target) {{",
        L(1, $"{Kw("for")} ({Kw("let")} i = 0; i &lt; a.length; i++) {{"),
        L(2, $"{Kw("if")} (a[i] === target) {Kw("return")} i;"),
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
    var n = values.Length;
    var compareCount = 0;

    for (var i = 0; i < n; i++)
    {
      compareCount++;
      var isMatch = values[i] == target;
      steps.Add(
        new SearchStep
        {
          Type = isMatch ? SearchStepType.Found : SearchStepType.Check,
          Snapshot = snapshot,
          CheckedIndex = i,
          // Everything before i has already been ruled out, so the candidate range is [i, n - 1].
          RangeStart = i,
          RangeEnd = n - 1,
          FoundIndex = isMatch ? i : null,
          CompareCount = compareCount,
          NarrowCount = i,
          ActiveCodeLines = isMatch ? [4, 5] : [3, 4],
          Caption = Res.Caption(isMatch ? "Search_Found" : "Linear_Check", language, i, values[i], target)
        }
      );

      if (isMatch)
      {
        return steps;
      }
    }

    steps.Add(
      new SearchStep
      {
        Type = SearchStepType.NotFound,
        Snapshot = snapshot,
        RangeStart = n,
        RangeEnd = n - 1,
        CompareCount = compareCount,
        NarrowCount = n,
        ActiveCodeLines = [8],
        Caption = Res.Caption("Search_NotFound", language, target)
      }
    );

    return steps;
  }
}
