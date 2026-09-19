namespace AlgoMotion.Services;

/// <summary>Small helpers shared by the array-search simulators: tokenized-HTML snippets for the code panel
/// (same <c>tok-*</c> classes the other simulators hand-write) and a range formatter for captions.</summary>
internal static class SearchCode
{
  private const string Indent = "&nbsp;&nbsp;&nbsp;&nbsp;";

  public static string Kw(
    string text
  )
  {
    return $"<span class=\"tok-kw\">{text}</span>";
  }

  public static string Ty(
    string text
  )
  {
    return $"<span class=\"tok-type\">{text}</span>";
  }

  public static string Fn(
    string text
  )
  {
    return $"<span class=\"tok-fn\">{text}</span>";
  }

  /// <summary>A code line indented by <paramref name="level"/> four-space steps.</summary>
  public static string L(
    int level,
    string text
  )
  {
    return string.Concat(Enumerable.Repeat(Indent, level)) + text;
  }

  /// <summary>"[lo, hi]" for a caption, or "∅" once the range is empty.</summary>
  public static string RangeText(
    int lo,
    int hi
  )
  {
    return lo > hi ? "∅" : $"[{lo}, {hi}]";
  }
}
