using AlgoMotion.Services;

namespace AlgoMotion.Models;

/// <summary>
/// Everything the UI needs to display one algorithm — its recorder function
/// plus all the presentational bits (title, code listing, captions) that used
/// to be hardcoded for Bubble Sort alone. Adding a new algorithm to the app
/// means writing a simulator and adding one entry here; nothing else in the
/// UI needs to change.
/// </summary>
public sealed class SortAlgorithmInfo
{
  public required SortAlgorithmKind Kind { get; init; }

  /// <summary>Big header title, e.g. "BUBBLE SORT" — kept as the one canonical (English)
  /// technical name regardless of <see cref="UiLanguage"/>, same as how most non-English
  /// algorithm write-ups still name it "Bubble Sort"; the resx-backed subtitle/hint carry the
  /// translated description.</summary>
  public required string Name { get; init; }

  /// <summary>Key prefix used to look up this algorithm's Subtitle/Hint text in
  /// Resources/SortMeta.resx (e.g. "Bubble" → "Bubble_Subtitle", "Bubble_Hint").</summary>
  public required string ResourceKey { get; init; }

  /// <summary>File name shown in the code panel's title bar, without extension — e.g. "bubble_sort";
  /// <see cref="GetFileName"/> appends the extension for whichever <see cref="CodeLanguage"/> is selected.</summary>
  public required string BaseFileName { get; init; }

  /// <summary>SortMeta.resx key for the comparison counter label — "Common_Compare" for every
  /// algorithm here.</summary>
  public string CompareLabelKey { get; init; } = "Common_Compare";

  /// <summary>SortMeta.resx key for the mutation counter label — usually "Common_Swap"; Merge/
  /// Counting/Radix/Tree Sort use "Common_Write" instead.</summary>
  public string ActionLabelKey { get; init; } = "Common_Swap";

  /// <summary>Pre-tokenized HTML source lines per <see cref="CodeLanguage"/> shown in the code panel.
  /// Every simulator provides all of <see cref="CodeLanguages.All"/>; <see cref="GetCodeLines"/> falls
  /// back to C for safety but should never actually need to.</summary>
  public required IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage { get; init; }

  /// <summary>Runs the algorithm on a copy of the input and records every step, narrating each one
  /// in the requested <see cref="UiLanguage"/>.</summary>
  public required Func<int[], UiLanguage, List<SortStep>> Record { get; init; }

  /// <summary>True for algorithms whose steps compare/swap a simple adjacent-ish pair — these get the crane.
  /// Merge Sort's compare/write steps don't map onto a stable pair in the live array, so it skips the crane
  /// and relies on the bars' own highlight + range dimming instead.</summary>
  public bool ShowCrane { get; init; } = true;

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
    return Res.SortMeta($"{ResourceKey}_Subtitle", language);
  }

  public string GetHintCaption(
    UiLanguage language
  )
  {
    return Res.SortMeta($"{ResourceKey}_Hint", language);
  }

  public string GetCompareLabel(
    UiLanguage language
  )
  {
    return Res.SortMeta(CompareLabelKey, language);
  }

  public string GetActionLabel(
    UiLanguage language
  )
  {
    return Res.SortMeta(ActionLabelKey, language);
  }
}

/// <summary>The full registry of algorithms offered in the picker, in display order.</summary>
public static class SortAlgorithms
{
  public static readonly IReadOnlyList<SortAlgorithmInfo> All =
  [
    new()
    {
      Kind = SortAlgorithmKind.Bubble,
      Name = "BUBBLE SORT",
      ResourceKey = "Bubble",
      BaseFileName = "bubble_sort",
      CodeByLanguage = BubbleSortSimulator.CodeByLanguage,
      Record = BubbleSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Selection,
      Name = "SELECTION SORT",
      ResourceKey = "Selection",
      BaseFileName = "selection_sort",
      CodeByLanguage = SelectionSortSimulator.CodeByLanguage,
      Record = SelectionSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Insertion,
      Name = "INSERTION SORT",
      ResourceKey = "Insertion",
      BaseFileName = "insertion_sort",
      CodeByLanguage = InsertionSortSimulator.CodeByLanguage,
      Record = InsertionSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Quick,
      Name = "QUICK SORT",
      ResourceKey = "Quick",
      BaseFileName = "quick_sort",
      CodeByLanguage = QuickSortSimulator.CodeByLanguage,
      Record = QuickSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Merge,
      Name = "MERGE SORT",
      ResourceKey = "Merge",
      BaseFileName = "merge_sort",
      ActionLabelKey = "Common_Write",
      CodeByLanguage = MergeSortSimulator.CodeByLanguage,
      Record = MergeSortSimulator.Record,
      ShowCrane = false
    },
    new()
    {
      Kind = SortAlgorithmKind.Heap,
      Name = "HEAP SORT",
      ResourceKey = "Heap",
      BaseFileName = "heap_sort",
      CodeByLanguage = HeapSortSimulator.CodeByLanguage,
      Record = HeapSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Shell,
      Name = "SHELL SORT",
      ResourceKey = "Shell",
      BaseFileName = "shell_sort",
      CodeByLanguage = ShellSortSimulator.CodeByLanguage,
      Record = ShellSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.CocktailShaker,
      Name = "COCKTAIL SHAKER SORT",
      ResourceKey = "Cocktail",
      BaseFileName = "cocktail_sort",
      CodeByLanguage = CocktailShakerSortSimulator.CodeByLanguage,
      Record = CocktailShakerSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Counting,
      Name = "COUNTING SORT",
      ResourceKey = "Counting",
      BaseFileName = "counting_sort",
      CompareLabelKey = "Common_Count",
      ActionLabelKey = "Common_Write",
      CodeByLanguage = CountingSortSimulator.CodeByLanguage,
      Record = CountingSortSimulator.Record,
      ShowCrane = false
    },
    new()
    {
      Kind = SortAlgorithmKind.Radix,
      Name = "RADIX SORT",
      ResourceKey = "Radix",
      BaseFileName = "radix_sort",
      CompareLabelKey = "Common_Count",
      ActionLabelKey = "Common_Write",
      CodeByLanguage = RadixSortSimulator.CodeByLanguage,
      Record = RadixSortSimulator.Record,
      ShowCrane = false
    },
    new()
    {
      Kind = SortAlgorithmKind.Gnome,
      Name = "GNOME SORT",
      ResourceKey = "Gnome",
      BaseFileName = "gnome_sort",
      CodeByLanguage = GnomeSortSimulator.CodeByLanguage,
      Record = GnomeSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Cycle,
      Name = "CYCLE SORT",
      ResourceKey = "Cycle",
      BaseFileName = "cycle_sort",
      CodeByLanguage = CycleSortSimulator.CodeByLanguage,
      Record = CycleSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Pancake,
      Name = "PANCAKE SORT",
      ResourceKey = "Pancake",
      BaseFileName = "pancake_sort",
      CodeByLanguage = PancakeSortSimulator.CodeByLanguage,
      Record = PancakeSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Tree,
      Name = "TREE SORT",
      ResourceKey = "Tree",
      BaseFileName = "tree_sort",
      ActionLabelKey = "Common_Write",
      CodeByLanguage = TreeSortSimulator.CodeByLanguage,
      Record = TreeSortSimulator.Record
    }
  ];

  public static SortAlgorithmInfo Get(
    SortAlgorithmKind kind
  )
  {
    return All.First(a => a.Kind == kind);
  }
}
