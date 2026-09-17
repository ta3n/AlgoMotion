namespace AlgoMotion.Models;

/// <summary>
/// The interface language — captions, labels, button text, everything the
/// user reads. Independent of <see cref="CodeLanguage"/>, which only picks
/// which programming language the code panel shows: a Vietnamese speaker
/// reading the app in VI can still study the Java listing, and an English
/// speaker in EN can study the Python one.
/// </summary>
public enum UiLanguage
{
  Vi,
  En
}

public static class UiLanguages
{
  public static readonly IReadOnlyList<UiLanguage> All = [UiLanguage.Vi, UiLanguage.En];

  public static string Label(
    UiLanguage language
  )
  {
    return language switch
    {
      UiLanguage.Vi => "Tiếng Việt",
      UiLanguage.En => "English",
      _ => language.ToString()
    };
  }

  /// <summary>Short code shown on the compact language-toggle button.</summary>
  public static string Code(
    UiLanguage language
  )
  {
    return language switch
    {
      UiLanguage.Vi => "VI",
      UiLanguage.En => "EN",
      _ => "?"
    };
  }
}
