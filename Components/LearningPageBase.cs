using AlgoMotion.Models;
using AlgoMotion.Services;
using Microsoft.AspNetCore.Components;

namespace AlgoMotion.Components;

public abstract class LearningPageBase : ComponentBase, IDisposable
{
  [Inject] protected UiLanguageState LanguageState { get; set; } = null!;
  protected UiLanguage Language => LanguageState.Current;
  protected string L(string key) => LearningText.Get(key, Language);
  protected override void OnInitialized() => LanguageState.Changed += LanguageChanged;
  private void LanguageChanged() => _ = InvokeAsync(StateHasChanged);
  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }
  protected virtual void Dispose(bool disposing)
  {
    if (disposing) LanguageState.Changed -= LanguageChanged;
  }
}
