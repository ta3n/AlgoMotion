using AlgoMotion.Services;

namespace AlgoMotion.Models;

/// <summary>
/// Everything the UI needs to display one array-search algorithm — mirrors
/// <see cref="TreeSearchAlgorithmInfo"/>. Adding a new algorithm means writing a simulator and adding
/// one entry to <see cref="SearchAlgorithms.All"/>; nothing else in the UI needs to change.
/// </summary>
public sealed class SearchAlgorithmInfo
{
  public required SearchAlgorithmKind Kind { get; init; }

  /// <summary>Big header title, e.g. "BINARY SEARCH" — the canonical English technical name regardless of UI language.</summary>
  public required string Name { get; init; }

  /// <summary>Key prefix for this algorithm's Subtitle/Hint text in Resources/SearchMeta.resx.</summary>
  public required string ResourceKey { get; init; }

  /// <summary>Code panel file name without extension, e.g. "binary_search".</summary>
  public required string BaseFileName { get; init; }

  public string CompareLabelKey { get; init; } = "Common_Compare";

  public string ActionLabelKey { get; init; } = "Common_Narrow";

  /// <summary>Pre-tokenized HTML source lines per language. Only C's lines line up with the recorded steps' <c>ActiveCodeLines</c>.</summary>
  public required IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage { get; init; }

  /// <summary>Records every step of searching the ascending-sorted array for <c>target</c>, narrated in the requested language.
  /// The input array is never modified.</summary>
  public required Func<int[], int, UiLanguage, List<SearchStep>> Record { get; init; }

  public string[] GetCodeLines(
    CodeLanguage language
  )
  {
    return CodeByLanguage.TryGetValue(language, out var lines) ? lines : CodeByLanguage[CodeLanguage.C];
  }

  public string GetFileName(
    CodeLanguage language
  )
  {
    return $"{BaseFileName}.{CodeLanguages.Extension(language)}";
  }

  public string GetSubtitle(
    UiLanguage language
  )
  {
    return Res.SearchMeta($"{ResourceKey}_Subtitle", language);
  }

  public string GetHintCaption(
    UiLanguage language
  )
  {
    return Res.SearchMeta($"{ResourceKey}_Hint", language);
  }

  public string GetCompareLabel(
    UiLanguage language
  )
  {
    return Res.SearchMeta(CompareLabelKey, language);
  }

  public string GetActionLabel(
    UiLanguage language
  )
  {
    return Res.SearchMeta(ActionLabelKey, language);
  }
}

/// <summary>The full registry of array-search algorithms offered in the picker, in display order.</summary>
public static class SearchAlgorithms
{
  public static readonly IReadOnlyList<SearchAlgorithmInfo> All =
  [
    new()
    {
      Kind = SearchAlgorithmKind.Linear,
      Name = "LINEAR SEARCH",
      ResourceKey = "Linear",
      BaseFileName = "linear_search",
      CodeByLanguage = LinearSearchSimulator.CodeByLanguage,
      Record = LinearSearchSimulator.Record
    },
    new()
    {
      Kind = SearchAlgorithmKind.Binary,
      Name = "BINARY SEARCH",
      ResourceKey = "Binary",
      BaseFileName = "binary_search",
      CodeByLanguage = BinarySearchSimulator.CodeByLanguage,
      Record = BinarySearchSimulator.Record
    },
    new()
    {
      Kind = SearchAlgorithmKind.Jump,
      Name = "JUMP SEARCH",
      ResourceKey = "Jump",
      BaseFileName = "jump_search",
      CodeByLanguage = JumpSearchSimulator.CodeByLanguage,
      Record = JumpSearchSimulator.Record
    },
    new()
    {
      Kind = SearchAlgorithmKind.Interpolation,
      Name = "INTERPOLATION SEARCH",
      ResourceKey = "Interpolation",
      BaseFileName = "interpolation_search",
      CodeByLanguage = InterpolationSearchSimulator.CodeByLanguage,
      Record = InterpolationSearchSimulator.Record
    }
  ];

  public static SearchAlgorithmInfo Get(
    SearchAlgorithmKind kind
  )
  {
    return All.First(a => a.Kind == kind);
  }
}
