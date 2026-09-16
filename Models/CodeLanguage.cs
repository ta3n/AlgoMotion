namespace AlgoMotion.Models;

public enum CodeLanguage
{
  C,
  CSharp,
  Java,
  Python,
  TypeScript,
  JavaScript
}

/// <summary>
/// Display metadata for <see cref="CodeLanguage"/> — label shown in the picker and file
/// extension used to build each algorithm's per-language file name.
///
/// The C snippet is the only one every <see cref="SortStep"/>/<see cref="TreeSearchStep"/>'s
/// <c>ActiveCodeLines</c> actually points into — every simulator's <c>Record</c> method was
/// written once, against the C reference's line numbers. Keeping the other five languages
/// perfectly line-synced with C would mean either contorting idiomatic C#/Java/TypeScript/
/// JavaScript into unnatural brace-per-line layouts, or padding Python with placeholder lines
/// just to preserve a line count it has no use for (it needs no closing-brace lines at all).
/// Neither is worth it — the UI simply shows the other languages without a highlighted line
/// instead of risking a highlight that lands on the wrong statement.
/// </summary>
public static class CodeLanguages
{
  public static readonly IReadOnlyList<CodeLanguage> All =
  [
    CodeLanguage.C,
    CodeLanguage.CSharp,
    CodeLanguage.Java,
    CodeLanguage.Python,
    CodeLanguage.TypeScript,
    CodeLanguage.JavaScript
  ];

  public static string Label(
    CodeLanguage language
  )
  {
    return language switch
    {
      CodeLanguage.C => "C",
      CodeLanguage.CSharp => "C#",
      CodeLanguage.Java => "Java",
      CodeLanguage.Python => "Python",
      CodeLanguage.TypeScript => "TypeScript",
      CodeLanguage.JavaScript => "JavaScript",
      _ => language.ToString()
    };
  }

  public static string Extension(
    CodeLanguage language
  )
  {
    return language switch
    {
      CodeLanguage.C => "c",
      CodeLanguage.CSharp => "cs",
      CodeLanguage.Java => "java",
      CodeLanguage.Python => "py",
      CodeLanguage.TypeScript => "ts",
      CodeLanguage.JavaScript => "js",
      _ => "txt"
    };
  }
}
