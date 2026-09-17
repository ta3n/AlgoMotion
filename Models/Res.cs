using System.Reflection;
using System.Resources;

namespace AlgoMotion.Models;

/// <summary>
/// Central lookup for every resx-backed localized string in the app: static UI chrome
/// (<see cref="UiText"/>), per-algorithm metadata (subtitle/hint/counter labels), and per-step
/// captions narrated inside each simulator's <c>Record</c> method.
///
/// Each concern is a pair of files under Resources/ — e.g. <c>UiText.resx</c> (Vietnamese,
/// no suffix) and <c>UiText.English.resx</c>. "English" is deliberately not a real
/// <see cref="CultureInfo"/> name (unlike "en" or "en-US"), so the SDK's resx tooling does not
/// treat it as a culture-specific satellite resource — both files compile as ordinary embedded
/// resources straight into the main assembly. That means both languages are always resident in
/// memory and switching <see cref="UiLanguage"/> is a plain dictionary-style lookup, not the
/// official <c>IStringLocalizer</c>/<c>CurrentUICulture</c> pattern (which requires a page reload
/// in standalone Blazor WebAssembly to fetch the right satellite assembly). Adding a third
/// language later is: add <c>&lt;Name&gt;.&lt;Language&gt;.resx</c> next to the existing ones,
/// register its <see cref="ResourceManager"/> below, and extend <see cref="UiLanguage"/> — no
/// other file needs to change.
/// </summary>
public static class Res
{
  private static readonly Assembly Assembly = typeof(Res).Assembly;

  private static ResourceManager Rm(
    string baseName
  )
  {
    return new ResourceManager($"AlgoMotion.Resources.{baseName}", Assembly);
  }

  private static readonly ResourceManager UiTextVi = Rm("UiText");
  private static readonly ResourceManager UiTextEn = Rm("UiText.English");
  private static readonly ResourceManager SortMetaVi = Rm("SortMeta");
  private static readonly ResourceManager SortMetaEn = Rm("SortMeta.English");
  private static readonly ResourceManager TreeMetaVi = Rm("TreeMeta");
  private static readonly ResourceManager TreeMetaEn = Rm("TreeMeta.English");
  private static readonly ResourceManager CaptionsVi = Rm("Captions");
  private static readonly ResourceManager CaptionsEn = Rm("Captions.English");

  private static string Lookup(
    ResourceManager vi,
    ResourceManager en,
    string key,
    UiLanguage language
  )
  {
    var resourceManager = language == UiLanguage.En ? en : vi;
    return resourceManager.GetString(key) ?? vi.GetString(key) ?? key;
  }

  public static string UiText(
    UiTextKey key,
    UiLanguage language
  )
  {
    return Lookup(UiTextVi, UiTextEn, key.ToString(), language);
  }

  public static string SortMeta(
    string key,
    UiLanguage language
  )
  {
    return Lookup(SortMetaVi, SortMetaEn, key, language);
  }

  public static string TreeMeta(
    string key,
    UiLanguage language
  )
  {
    return Lookup(TreeMetaVi, TreeMetaEn, key, language);
  }

  /// <summary>Looks up a per-step caption template and formats it with the given arguments, e.g.
  /// a key whose Vietnamese value is <c>"So sánh a[{0}] và a[{1}]"</c> and English value is
  /// <c>"Compare a[{0}] and a[{1}]"</c>.</summary>
  public static string Caption(
    string key,
    UiLanguage language,
    params object[] args
  )
  {
    var template = Lookup(CaptionsVi, CaptionsEn, key, language);
    return args.Length == 0 ? template : string.Format(template, args);
  }
}
