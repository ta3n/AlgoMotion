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

  /// <summary>Big header title, e.g. "BST SEARCH".</summary>
  public required string Name { get; init; }

  /// <summary>Spanish-style tagline under the title, matching the sorting page's convention.</summary>
  public required string Subtitle { get; init; }

  /// <summary>File name shown in the code panel's title bar, without extension — e.g. "bst_search";
  /// <see cref="GetFileName"/> appends the extension for whichever language is selected.</summary>
  public required string BaseFileName { get; init; }

  /// <summary>Small caption under the code panel describing the core idea.</summary>
  public required string HintCaption { get; init; }

  /// <summary>Label for the comparison counter — "COMPARA" for every algorithm here.</summary>
  public string CompareLabel { get; init; } = "COMPARA";

  /// <summary>Label for the visit counter — "VISITA" for every algorithm here.</summary>
  public string ActionLabel { get; init; } = "VISITA";

  /// <summary>Pre-tokenized HTML source lines per language shown in the code panel. See
  /// <see cref="SortAlgorithmInfo.CodeByLanguage"/> for why only C's lines up with the recorded
  /// steps' <c>ActiveCodeLines</c>.</summary>
  public required IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage { get; init; }

  /// <summary>Builds a tree from the given values and records every step of searching it for
  /// <c>target</c>.</summary>
  public required Func<int[], int, TreeSearchResult> Record { get; init; }

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
      Subtitle = "BÚSQUEDA POR ÁRBOL BINARIO",
      BaseFileName = "bst_search",
      HintCaption = "USA EL ORDEN DEL ÁRBOL PARA IR DIRECTO AL OBJETIVO",
      CodeByLanguage = BstSearchSimulator.CodeByLanguage,
      Record = BstSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.DfsPreorder,
      Name = "DFS PREORDER SEARCH",
      Subtitle = "BÚSQUEDA EN PROFUNDIDAD (PREORDEN)",
      BaseFileName = "preorder_search",
      HintCaption = "VISITA RAÍZ, LUEGO IZQUIERDA, LUEGO DERECHA",
      CodeByLanguage = DfsPreorderSearchSimulator.CodeByLanguage,
      Record = DfsPreorderSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.DfsInorder,
      Name = "DFS INORDER SEARCH",
      Subtitle = "BÚSQUEDA EN PROFUNDIDAD (INORDEN)",
      BaseFileName = "inorder_search",
      HintCaption = "VISITA IZQUIERDA, LUEGO RAÍZ, LUEGO DERECHA",
      CodeByLanguage = DfsInorderSearchSimulator.CodeByLanguage,
      Record = DfsInorderSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.DfsPostorder,
      Name = "DFS POSTORDER SEARCH",
      Subtitle = "BÚSQUEDA EN PROFUNDIDAD (POSTORDEN)",
      BaseFileName = "postorder_search",
      HintCaption = "VISITA IZQUIERDA, LUEGO DERECHA, LUEGO RAÍZ",
      CodeByLanguage = DfsPostorderSearchSimulator.CodeByLanguage,
      Record = DfsPostorderSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.Bfs,
      Name = "BFS SEARCH",
      Subtitle = "BÚSQUEDA EN ANCHURA",
      BaseFileName = "bfs_search",
      HintCaption = "VISITA NIVEL POR NIVEL USANDO UNA COLA",
      CodeByLanguage = BfsSearchSimulator.CodeByLanguage,
      Record = BfsSearchSimulator.Record
    },
    new()
    {
      Kind = TreeSearchAlgorithmKind.Avl,
      Name = "AVL SEARCH",
      Subtitle = "BÚSQUEDA POR ÁRBOL AVL",
      BaseFileName = "avl_search",
      HintCaption = "ÁRBOL AUTOBALANCEADO: PROFUNDIDAD SIEMPRE O(LOG N)",
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
