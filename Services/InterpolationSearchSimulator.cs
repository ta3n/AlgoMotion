using AlgoMotion.Models;
using static AlgoMotion.Services.SearchCode;

namespace AlgoMotion.Services;

/// <summary>
/// Records Interpolation Search: instead of always probing the middle, estimate where the target should
/// sit by assuming the values grow roughly evenly between a[lo] and a[hi] — like opening a phone book near
/// the back when looking for "W". On evenly spread data it needs about O(log log n) probes; on badly
/// skewed data it can degrade to O(n).
///
/// <code>
///  1  int interpolation_search(int a[], int n, int target)
///  2  {
///  3      int lo = 0, hi = n - 1;
///  4      while (lo &lt;= hi &amp;&amp; target &gt;= a[lo] &amp;&amp; target &lt;= a[hi]) {
///  5          int pos = lo;
///  6          if (a[hi] != a[lo]) {
///  7              pos = lo + (long)(target - a[lo]) * (hi - lo) / (a[hi] - a[lo]);
///  8          }
///  9          if (a[pos] == target) {
/// 10              return pos;
/// 11          }
/// 12          if (a[pos] &lt; target) {
/// 13              lo = pos + 1;
/// 14          } else {
/// 15              hi = pos - 1;
/// 16          }
/// 17      }
/// 18      return -1;
/// 19  }
/// </code>
/// </summary>
public static class InterpolationSearchSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        $"{Ty("int")} {Fn("interpolation_search")}({Ty("int")} a[], {Ty("int")} n, {Ty("int")} target)",
        "{",
        L(1, $"{Ty("int")} lo = 0, hi = n - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi &amp;&amp; target &gt;= a[lo] &amp;&amp; target &lt;= a[hi]) {{"),
        L(2, $"{Ty("int")} pos = lo;"),
        L(2, $"{Kw("if")} (a[hi] != a[lo]) {{"),
        L(3, $"pos = lo + ({Ty("long")})(target - a[lo]) * (hi - lo) / (a[hi] - a[lo]);"),
        L(2, "}"),
        L(2, $"{Kw("if")} (a[pos] == target) {{"),
        L(3, $"{Kw("return")} pos;"),
        L(2, "}"),
        L(2, $"{Kw("if")} (a[pos] &lt; target) {{"),
        L(3, "lo = pos + 1;"),
        L(2, $"}} {Kw("else")} {{"),
        L(3, "hi = pos - 1;"),
        L(2, "}"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        "// Requires an ascending-sorted array of roughly evenly spread values.",
        $"{Kw("static")} {Ty("int")} {Fn("InterpolationSearch")}({Ty("int")}[] a, {Ty("int")} target)",
        "{",
        L(1, $"{Ty("int")} lo = 0, hi = a.Length - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi &amp;&amp; target &gt;= a[lo] &amp;&amp; target &lt;= a[hi]) {{"),
        L(2, $"{Ty("int")} pos = lo;"),
        L(2, $"{Kw("if")} (a[hi] != a[lo])"),
        L(3, $"pos = lo + ({Ty("int")})(({Ty("long")})(target - a[lo]) * (hi - lo) / (a[hi] - a[lo]));"),
        L(2, $"{Kw("if")} (a[pos] == target) {Kw("return")} pos;"),
        L(2, $"{Kw("if")} (a[pos] &lt; target) lo = pos + 1;"),
        L(2, $"{Kw("else")} hi = pos - 1;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.Java] =
      [
        "// Requires an ascending-sorted array of roughly evenly spread values.",
        $"{Kw("static")} {Ty("int")} {Fn("interpolationSearch")}({Ty("int")}[] a, {Ty("int")} target) {{",
        L(1, $"{Ty("int")} lo = 0, hi = a.length - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi &amp;&amp; target &gt;= a[lo] &amp;&amp; target &lt;= a[hi]) {{"),
        L(2, $"{Ty("int")} pos = lo;"),
        L(2, $"{Kw("if")} (a[hi] != a[lo])"),
        L(3, $"pos = lo + ({Ty("int")}) (({Ty("long")}) (target - a[lo]) * (hi - lo) / (a[hi] - a[lo]));"),
        L(2, $"{Kw("if")} (a[pos] == target) {Kw("return")} pos;"),
        L(2, $"{Kw("if")} (a[pos] &lt; target) lo = pos + 1;"),
        L(2, $"{Kw("else")} hi = pos - 1;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.Python] =
      [
        "# Requires an ascending-sorted list of roughly evenly spread values.",
        $"{Kw("def")} {Fn("interpolation_search")}(a, target):",
        L(1, "lo, hi = 0, len(a) - 1"),
        L(1, $"{Kw("while")} lo &lt;= hi {Kw("and")} a[lo] &lt;= target &lt;= a[hi]:"),
        L(2, "pos = lo"),
        L(2, $"{Kw("if")} a[hi] != a[lo]:"),
        L(3, "pos = lo + (target - a[lo]) * (hi - lo) // (a[hi] - a[lo])"),
        L(2, $"{Kw("if")} a[pos] == target:"),
        L(3, $"{Kw("return")} pos"),
        L(2, $"{Kw("if")} a[pos] &lt; target:"),
        L(3, "lo = pos + 1"),
        L(2, $"{Kw("else")}:"),
        L(3, "hi = pos - 1"),
        L(1, $"{Kw("return")} -1")
      ],
      [CodeLanguage.TypeScript] =
      [
        "// Requires an ascending-sorted array of roughly evenly spread values.",
        $"{Kw("function")} {Fn("interpolationSearch")}(a: {Ty("number")}[], target: {Ty("number")}): {Ty("number")} {{",
        L(1, $"{Kw("let")} lo = 0, hi = a.length - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi &amp;&amp; target &gt;= a[lo] &amp;&amp; target &lt;= a[hi]) {{"),
        L(2, $"{Kw("let")} pos = lo;"),
        L(2, $"{Kw("if")} (a[hi] !== a[lo])"),
        L(3, "pos = lo + Math.floor((target - a[lo]) * (hi - lo) / (a[hi] - a[lo]));"),
        L(2, $"{Kw("if")} (a[pos] === target) {Kw("return")} pos;"),
        L(2, $"{Kw("if")} (a[pos] &lt; target) lo = pos + 1;"),
        L(2, $"{Kw("else")} hi = pos - 1;"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        "// Requires an ascending-sorted array of roughly evenly spread values.",
        $"{Kw("function")} {Fn("interpolationSearch")}(a, target) {{",
        L(1, $"{Kw("let")} lo = 0, hi = a.length - 1;"),
        L(1, $"{Kw("while")} (lo &lt;= hi &amp;&amp; target &gt;= a[lo] &amp;&amp; target &lt;= a[hi]) {{"),
        L(2, $"{Kw("let")} pos = lo;"),
        L(2, $"{Kw("if")} (a[hi] !== a[lo])"),
        L(3, "pos = lo + Math.floor((target - a[lo]) * (hi - lo) / (a[hi] - a[lo]));"),
        L(2, $"{Kw("if")} (a[pos] === target) {Kw("return")} pos;"),
        L(2, $"{Kw("if")} (a[pos] &lt; target) lo = pos + 1;"),
        L(2, $"{Kw("else")} hi = pos - 1;"),
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

    while (lo <= hi && target >= values[lo] && target <= values[hi])
    {
      // The target lies within [a[lo], a[hi]], so the estimate always lands inside [lo, hi].
      var pos = values[hi] == values[lo]
        ? lo
        : lo + (int)((long)(target - values[lo]) * (hi - lo) / (values[hi] - values[lo]));
      compareCount++;

      if (values[pos] == target)
      {
        steps.Add(
          new SearchStep
          {
            Type = SearchStepType.Found,
            Snapshot = snapshot,
            CheckedIndex = pos,
            RangeStart = lo,
            RangeEnd = hi,
            FoundIndex = pos,
            CompareCount = compareCount,
            NarrowCount = narrowCount,
            ActiveCodeLines = [9, 10],
            Caption = Res.Caption("Search_Found", language, pos, values[pos], target)
          }
        );

        return steps;
      }

      var goRight = values[pos] < target;
      if (goRight)
      {
        lo = pos + 1;
      }
      else
      {
        hi = pos - 1;
      }

      narrowCount++;
      steps.Add(
        new SearchStep
        {
          Type = goRight ? SearchStepType.NarrowRight : SearchStepType.NarrowLeft,
          Snapshot = snapshot,
          CheckedIndex = pos,
          RangeStart = lo,
          RangeEnd = hi,
          CompareCount = compareCount,
          NarrowCount = narrowCount,
          ActiveCodeLines = goRight ? [12, 13] : [14, 15],
          Caption = Res.Caption(
            goRight ? "Interpolation_NarrowRight" : "Interpolation_NarrowLeft",
            language,
            pos,
            values[pos],
            target,
            RangeText(lo, hi)
          )
        }
      );
    }

    // Either the range emptied, or the target fell outside [a[lo], a[hi]] and can't be in what's left.
    var outOfRange = lo <= hi;
    steps.Add(
      new SearchStep
      {
        Type = SearchStepType.NotFound,
        Snapshot = snapshot,
        // Empty range either way, so every bar is dimmed.
        RangeStart = hi + 1,
        RangeEnd = hi,
        CompareCount = compareCount,
        NarrowCount = narrowCount,
        ActiveCodeLines = [18],
        Caption = outOfRange
          ? Res.Caption("Interpolation_OutOfRange", language, target, values[lo], values[hi])
          : Res.Caption("Search_NotFound", language, target)
      }
    );

    return steps;
  }
}
