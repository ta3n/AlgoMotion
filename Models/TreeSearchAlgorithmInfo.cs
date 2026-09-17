using AlgoMotion.Services;

namespace AlgoMotion.Models;

/// <summary>The tree a search ran against, plus the recorded animation of the search itself.
/// The tree is rebuilt fresh by every <see cref="TreeSearchAlgorithmInfo.Record"/> call — unlike
/// the sorting page's array, which one algorithm's steps mutate in place, a tree-search algorithm
/// never changes the structure it searches, so the two are naturally returned together.</summary>
public sealed record TreeSearchResult(
  IReadOnlyList<TreeNode> Nodes,
  int? RootId,
  List<TreeSearchStep> Steps
);

/// <summary>
/// Everything the UI needs to display one tree-search algorithm — mirrors
/// <see cref="SortAlgorithmInfo"/>'s role for sorting. Adding a new algorithm
/// means writing a simulator and adding one entry here; nothing else in the
/// UI needs to change.
/// </summary>
public sealed class TreeSearchAlgorithmInfo
{
  public required TreeSearchAlgorithmKind Kind { get; init; }

  /// <summary>Big header title, e.g. "BST SEARCH" — kept as the one canonical (English) technical
  /// name regardless of <see cref="UiLanguage"/>; the resx-backed subtitle/hint carry the
  /// translated description.</summary>
  public required string Name { get; init; }

  /// <summary>Key prefix used to look up this algorithm's Subtitle/Hint text in
  /// Resources/TreeMeta.resx (e.g. "Bst" → "Bst_Subtitle", "Bst_Hint").</summary>
  public required string ResourceKey { get; init; }

  /// <summary>File name shown in the code panel's title bar, without extension — e.g. "bst_search";
  /// <see cref="GetFileName"/> appends the extension for whichever language is selected.</summary>
  public required string BaseFileName { get; init; }

  /// <summary>TreeMeta.resx key for the comparison counter label — "Common_Compare" for every
  /// algorithm here.</summary>
  public string CompareLabelKey { get; init; } = "Common_Compare";

  /// <summary>TreeMeta.resx key for the visit counter label — "Common_Visit" for every algorithm
  /// here.</summary>
  public string ActionLabelKey { get; init; } = "Common_Visit";

  /// <summary>Pre-tokenized HTML source lines per language shown in the code panel. See
  /// <see cref="SortAlgorithmInfo.CodeByLanguage"/> for why only C's lines up with the recorded
  /// steps' <c>ActiveCodeLines</c>.</summary>
  public required IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage { get; init; }

  /// <summary>Builds a tree from the given values and records every step of searching it for
  /// <c>target</c>, narrating each one in the requested <see cref="UiLanguage"/>.</summary>
  public required Func<int[], int, UiLanguage, TreeSearchResult> Record { get; init; }

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
    return Res.TreeMeta($"{ResourceKey}_Subtitle", language);
  }

  public string GetHintCaption(
    UiLanguage language
  )
  {
    return Res.TreeMeta($"{ResourceKey}_Hint", language);
  }

  public string GetCompareLabel(
    UiLanguage language
  )
  {
    return Res.TreeMeta(CompareLabelKey, language);
  }

  public string GetActionLabel(
    UiLanguage language
  )
  {
    return Res.TreeMeta(ActionLabelKey, language);
  }
}

/// <summary>The full registry of tree-search algorithms offered in the picker, in display order.</summary>
public static class TreeSearchAlgorithms
{
  public static readonly IReadOnlyList<TreeSearchAlgorithmInfo> All =
  [
    new()
    {
      Kind = TreeSearchAlgorithmKind.Bst,
      Name = "BST SEARCH",
      ResourceKey = "Bst",
      BaseFileName = "bst_search",
      CodeByLanguage = BstSearchSimulator.CodeByLanguage,
      Record = BstSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.DfsPreorder,
      Name = "DFS PREORDER SEARCH",
      ResourceKey = "DfsPreorder",
      BaseFileName = "preorder_search",
      CodeByLanguage = DfsPreorderSearchSimulator.CodeByLanguage,
      Record = DfsPreorderSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.DfsInorder,
      Name = "DFS INORDER SEARCH",
      ResourceKey = "DfsInorder",
      BaseFileName = "inorder_search",
      CodeByLanguage = DfsInorderSearchSimulator.CodeByLanguage,
      Record = DfsInorderSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.DfsPostorder,
      Name = "DFS POSTORDER SEARCH",
      ResourceKey = "DfsPostorder",
      BaseFileName = "postorder_search",
      CodeByLanguage = DfsPostorderSearchSimulator.CodeByLanguage,
      Record = DfsPostorderSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.Bfs,
      Name = "BFS SEARCH",
      ResourceKey = "Bfs",
      BaseFileName = "bfs_search",
      CodeByLanguage = BfsSearchSimulator.CodeByLanguage,
      Record = BfsSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.Avl,
      Name = "AVL SEARCH",
      ResourceKey = "Avl",
      BaseFileName = "avl_search",
      CodeByLanguage = AvlSearchSimulator.CodeByLanguage,
      Record = AvlSearchSimulator.Record
    }
  ];

  public static TreeSearchAlgorithmInfo Get(
    TreeSearchAlgorithmKind kind
  )
  {
    return All.First(a => a.Kind == kind);
  }
}
