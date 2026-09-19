using AlgoMotion.Models;
using static AlgoMotion.Services.SearchCode;

namespace AlgoMotion.Services;

/// <summary>
/// Records Jump Search: hop through the sorted array in blocks of √n, comparing only the LAST element of
/// each block. The first block whose last element is &gt;= the target is the only place the target can be, so
/// a plain linear scan inside that one block finishes the job — O(√n) comparisons overall.
///
/// <code>
///  1  int jump_search(int a[], int n, int target)
///  2  {
///  3      int step = (int)sqrt(n);
///  4      int prev = 0;
///  5      while (prev &lt; n &amp;&amp; a[min(prev + step, n) - 1] &lt; target) {
///  6          prev += step;
///  7      }
///  8      int end = min(prev + step, n);
///  9      for (int i = prev; i &lt; end; i++) {
/// 10          if (a[i] == target) {
/// 11              return i;
/// 12          }
/// 13      }
/// 14      return -1;
/// 15  }
/// </code>
/// </summary>
public static class JumpSearchSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        $"{Ty("int")} {Fn("jump_search")}({Ty("int")} a[], {Ty("int")} n, {Ty("int")} target)",
        "{",
        L(1, $"{Ty("int")} step = ({Ty("int")})sqrt(n);"),
        L(1, $"{Ty("int")} prev = 0;"),
        L(1, $"{Kw("while")} (prev &lt; n &amp;&amp; a[min(prev + step, n) - 1] &lt; target) {{"),
        L(2, "prev += step;"),
        L(1, "}"),
        L(1, $"{Ty("int")} end = min(prev + step, n);"),
        L(1, $"{Kw("for")} ({Ty("int")} i = prev; i &lt; end; i++) {{"),
        L(2, $"{Kw("if")} (a[i] == target) {{"),
        L(3, $"{Kw("return")} i;"),
        L(2, "}"),
        L(1, "}"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        "// Requires an ascending-sorted array.",
        $"{Kw("static")} {Ty("int")} {Fn("JumpSearch")}({Ty("int")}[] a, {Ty("int")} target)",
        "{",
        L(1, $"{Ty("int")} n = a.Length;"),
        L(1, $"{Ty("int")} step = ({Ty("int")})Math.Sqrt(n);"),
        L(1, $"{Ty("int")} prev = 0;"),
        L(1, $"{Kw("while")} (prev &lt; n &amp;&amp; a[Math.Min(prev + step, n) - 1] &lt; target)"),
        L(2, "prev += step;"),
        L(1, $"{Ty("int")} end = Math.Min(prev + step, n);"),
        L(1, $"{Kw("for")} ({Ty("int")} i = prev; i &lt; end; i++)"),
        L(2, $"{Kw("if")} (a[i] == target) {Kw("return")} i;"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.Java] =
      [
        "// Requires an ascending-sorted array.",
        $"{Kw("static")} {Ty("int")} {Fn("jumpSearch")}({Ty("int")}[] a, {Ty("int")} target) {{",
        L(1, $"{Ty("int")} n = a.length;"),
        L(1, $"{Ty("int")} step = ({Ty("int")}) Math.sqrt(n);"),
        L(1, $"{Ty("int")} prev = 0;"),
        L(1, $"{Kw("while")} (prev &lt; n &amp;&amp; a[Math.min(prev + step, n) - 1] &lt; target)"),
        L(2, "prev += step;"),
        L(1, $"{Ty("int")} end = Math.min(prev + step, n);"),
        L(1, $"{Kw("for")} ({Ty("int")} i = prev; i &lt; end; i++)"),
        L(2, $"{Kw("if")} (a[i] == target) {Kw("return")} i;"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.Python] =
      [
        "# Requires an ascending-sorted list.",
        $"{Kw("import")} math",
        "",
        $"{Kw("def")} {Fn("jump_search")}(a, target):",
        L(1, "n = len(a)"),
        L(1, "step = int(math.sqrt(n))"),
        L(1, "prev = 0"),
        L(1, $"{Kw("while")} prev &lt; n {Kw("and")} a[min(prev + step, n) - 1] &lt; target:"),
        L(2, "prev += step"),
        L(1, $"{Kw("for")} i {Kw("in")} range(prev, min(prev + step, n)):"),
        L(2, $"{Kw("if")} a[i] == target:"),
        L(3, $"{Kw("return")} i"),
        L(1, $"{Kw("return")} -1")
      ],
      [CodeLanguage.TypeScript] =
      [
        "// Requires an ascending-sorted array.",
        $"{Kw("function")} {Fn("jumpSearch")}(a: {Ty("number")}[], target: {Ty("number")}): {Ty("number")} {{",
        L(1, $"{Kw("const")} n = a.length;"),
        L(1, $"{Kw("const")} step = Math.floor(Math.sqrt(n));"),
        L(1, $"{Kw("let")} prev = 0;"),
        L(1, $"{Kw("while")} (prev &lt; n &amp;&amp; a[Math.min(prev + step, n) - 1] &lt; target)"),
        L(2, "prev += step;"),
        L(1, $"{Kw("const")} end = Math.min(prev + step, n);"),
        L(1, $"{Kw("for")} ({Kw("let")} i = prev; i &lt; end; i++)"),
        L(2, $"{Kw("if")} (a[i] === target) {Kw("return")} i;"),
        L(1, $"{Kw("return")} -1;"),
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        "// Requires an ascending-sorted array.",
        $"{Kw("function")} {Fn("jumpSearch")}(a, target) {{",
        L(1, $"{Kw("const")} n = a.length;"),
        L(1, $"{Kw("const")} step = Math.floor(Math.sqrt(n));"),
        L(1, $"{Kw("let")} prev = 0;"),
        L(1, $"{Kw("while")} (prev &lt; n &amp;&amp; a[Math.min(prev + step, n) - 1] &lt; target)"),
        L(2, "prev += step;"),
        L(1, $"{Kw("const")} end = Math.min(prev + step, n);"),
        L(1, $"{Kw("for")} ({Kw("let")} i = prev; i &lt; end; i++)"),
        L(2, $"{Kw("if")} (a[i] === target) {Kw("return")} i;"),
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
    var narrowCount = 0;

    SearchStep Frame(
      SearchStepType type,
      int? checkedIndex,
      int rangeStart,
      int rangeEnd,
      int? foundIndex,
      int[] lines,
      string caption
    )
    {
      return new SearchStep
      {
        Type = type,
        Snapshot = snapshot,
        CheckedIndex = checkedIndex,
        RangeStart = rangeStart,
        RangeEnd = rangeEnd,
        FoundIndex = foundIndex,
        CompareCount = compareCount,
        NarrowCount = narrowCount,
        ActiveCodeLines = lines,
        Caption = caption
      };
    }

    if (n > 0)
    {
      var step = Math.Max(1, (int)Math.Sqrt(n));
      var prev = 0;
      var exhausted = false;

      // Phase 1: compare only the last element of each block until one reaches the target.
      while (true)
      {
        var last = Math.Min(prev + step, n) - 1;
        compareCount++;
        if (values[last] >= target)
        {
          break;
        }

        prev += step;
        narrowCount++;
        exhausted = prev >= n;
        var nextEnd = Math.Min(prev + step, n) - 1;
        steps.Add(
          Frame(
            SearchStepType.JumpBlock,
            last,
            exhausted ? n : prev,
            exhausted ? n - 1 : nextEnd,
            null,
            [5, 6],
            Res.Caption("Jump_Skip", language, last, values[last], target, exhausted ? "∅" : RangeText(prev, nextEnd))
          )
        );

        if (exhausted)
        {
          break;
        }
      }

      if (!exhausted)
      {
        var end = Math.Min(prev + step, n);
        steps.Add(
          Frame(
            SearchStepType.Check,
            end - 1,
            prev,
            end - 1,
            null,
            [5, 8],
            Res.Caption("Jump_BlockFound", language, end - 1, values[end - 1], target, RangeText(prev, end - 1))
          )
        );

        // Phase 2: linear scan inside the one block that can hold the target.
        for (var i = prev; i < end; i++)
        {
          compareCount++;
          var isMatch = values[i] == target;
          steps.Add(
            Frame(
              isMatch ? SearchStepType.Found : SearchStepType.Check,
              i,
              i,
              end - 1,
              isMatch ? i : null,
              isMatch ? [10, 11] : [9, 10],
              Res.Caption(isMatch ? "Search_Found" : "Jump_Scan", language, i, values[i], target)
            )
          );

          if (isMatch)
          {
            return steps;
          }
        }
      }
    }

    steps.Add(Frame(SearchStepType.NotFound, null, n, n - 1, null, [14], Res.Caption("Search_NotFound", language, target)));
    return steps;
  }
}
