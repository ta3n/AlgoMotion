using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// The app's single source of truth for the current interface language, shared via DI so both
/// page components (Home/TreeSearch) and <c>MainLayout</c> — which sits outside any page and
/// therefore can't hold its own <see cref="UiLanguage"/> field — stay in sync. Changing the
/// language here fires <see cref="Changed"/> for every subscriber to re-render and (for the
/// pages) re-record their current algorithm's captions in the new language.
/// </summary>
public sealed class UiLanguageState
{
  public UiLanguage Current { get; private set; } = UiLanguage.Vi;

  public event Action? Changed;

  public void Set(
    UiLanguage language
  )
  {
    if (language == Current)
    {
      return;
    }

    Current = language;
    Changed?.Invoke();
  }
}
