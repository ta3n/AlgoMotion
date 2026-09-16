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

  /// <summary>Big header title, e.g. "BUBBLE SORT".</summary>
  public required string Name { get; init; }

  /// <summary>Spanish-style tagline under the title, e.g. "ORDENACIÓN POR INTERCAMBIO".</summary>
  public required string Subtitle { get; init; }

  /// <summary>File name shown in the code panel's title bar, without extension — e.g. "bubble_sort";
  /// <see cref="GetFileName"/> appends the extension for whichever language is selected.</summary>
  public required string BaseFileName { get; init; }

  /// <summary>Small caption under the code panel describing the core idea.</summary>
  public required string HintCaption { get; init; }

  /// <summary>Label for the comparison counter — "COMPARA" for every algorithm here.</summary>
  public string CompareLabel { get; init; } = "COMPARA";

  /// <summary>Label for the mutation counter — usually "CAMBIA" (swaps); Merge Sort uses "ESCRIBE" (writes).</summary>
  public string ActionLabel { get; init; } = "CAMBIA";

  /// <summary>Pre-tokenized HTML source lines per language shown in the code panel. Every simulator
  /// provides all of <see cref="CodeLanguages.All"/>; <see cref="GetCodeLines"/> falls back to C for
  /// safety but should never actually need to.</summary>
  public required IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage { get; init; }

  /// <summary>Runs the algorithm on a copy of the input and records every step.</summary>
  public required Func<int[], List<SortStep>> Record { get; init; }

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
      Subtitle = "ORDENACIÓN POR INTERCAMBIO",
      BaseFileName = "bubble_sort",
      HintCaption = "SOLO COMPARA VECINOS, DE DOS EN DOS",
      CodeByLanguage = BubbleSortSimulator.CodeByLanguage,
      Record = BubbleSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Selection,
      Name = "SELECTION SORT",
      Subtitle = "ORDENACIÓN POR SELECCIÓN",
      BaseFileName = "selection_sort",
      HintCaption = "BUSCA EL MÍNIMO Y LO COLOCA AL FRENTE",
      CodeByLanguage = SelectionSortSimulator.CodeByLanguage,
      Record = SelectionSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Insertion,
      Name = "INSERTION SORT",
      Subtitle = "ORDENACIÓN POR INSERCIÓN",
      BaseFileName = "insertion_sort",
      HintCaption = "TOMA UNA CLAVE Y LA DESLIZA A SU LUGAR",
      CodeByLanguage = InsertionSortSimulator.CodeByLanguage,
      Record = InsertionSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Quick,
      Name = "QUICK SORT",
      Subtitle = "ORDENACIÓN RÁPIDA",
      BaseFileName = "quick_sort",
      HintCaption = "DIVIDE ALREDEDOR DE UN PIVOTE",
      CodeByLanguage = QuickSortSimulator.CodeByLanguage,
      Record = QuickSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Merge,
      Name = "MERGE SORT",
      Subtitle = "ORDENACIÓN POR MEZCLA",
      BaseFileName = "merge_sort",
      HintCaption = "DIVIDE, ORDENA Y MEZCLA",
      ActionLabel = "ESCRIBE",
      CodeByLanguage = MergeSortSimulator.CodeByLanguage,
      Record = MergeSortSimulator.Record,
      ShowCrane = false
    },
    new()
    {
      Kind = SortAlgorithmKind.Heap,
      Name = "HEAP SORT",
      Subtitle = "ORDENACIÓN POR MONTÍCULOS",
      BaseFileName = "heap_sort",
      HintCaption = "CONSTRUYE UN MONTÍCULO Y EXTRAE EL MÁXIMO",
      CodeByLanguage = HeapSortSimulator.CodeByLanguage,
      Record = HeapSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Shell,
      Name = "SHELL SORT",
      Subtitle = "ORDENACIÓN SHELL",
      BaseFileName = "shell_sort",
      HintCaption = "INSERCIÓN CON HUECOS QUE SE VAN REDUCIENDO",
      CodeByLanguage = ShellSortSimulator.CodeByLanguage,
      Record = ShellSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.CocktailShaker,
      Name = "COCKTAIL SHAKER SORT",
      Subtitle = "ORDENACIÓN COCTELERA",
      BaseFileName = "cocktail_sort",
      HintCaption = "RECORRE EL ARREGLO EN AMBAS DIRECCIONES",
      CodeByLanguage = CocktailShakerSortSimulator.CodeByLanguage,
      Record = CocktailShakerSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Counting,
      Name = "COUNTING SORT",
      Subtitle = "ORDENACIÓN POR CONTEO",
      BaseFileName = "counting_sort",
      HintCaption = "CUENTA FRECUENCIAS Y UBICA CADA VALOR",
      CompareLabel = "CUENTA",
      ActionLabel = "ESCRIBE",
      CodeByLanguage = CountingSortSimulator.CodeByLanguage,
      Record = CountingSortSimulator.Record,
      ShowCrane = false
    },
    new()
    {
      Kind = SortAlgorithmKind.Radix,
      Name = "RADIX SORT",
      Subtitle = "ORDENACIÓN POR RADIX",
      BaseFileName = "radix_sort",
      HintCaption = "ORDENA DÍGITO POR DÍGITO, DE MENOR A MAYOR PESO",
      CompareLabel = "CUENTA",
      ActionLabel = "ESCRIBE",
      CodeByLanguage = RadixSortSimulator.CodeByLanguage,
      Record = RadixSortSimulator.Record,
      ShowCrane = false
    },
    new()
    {
      Kind = SortAlgorithmKind.Gnome,
      Name = "GNOME SORT",
      Subtitle = "ORDENACIÓN DEL GNOMO",
      BaseFileName = "gnome_sort",
      HintCaption = "AVANZA O RETROCEDE UN PASO SEGÚN LA COMPARACIÓN",
      CodeByLanguage = GnomeSortSimulator.CodeByLanguage,
      Record = GnomeSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Cycle,
      Name = "CYCLE SORT",
      Subtitle = "ORDENACIÓN CÍCLICA",
      BaseFileName = "cycle_sort",
      HintCaption = "SIGUE CADA CICLO: EL MÍNIMO DE INTERCAMBIOS POSIBLE",
      CodeByLanguage = CycleSortSimulator.CodeByLanguage,
      Record = CycleSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Pancake,
      Name = "PANCAKE SORT",
      Subtitle = "ORDENACIÓN DE PANQUEQUES",
      BaseFileName = "pancake_sort",
      HintCaption = "VOLTEA UN SEGMENTO INICIAL COMO UNA PILA DE PANQUEQUES",
      CodeByLanguage = PancakeSortSimulator.CodeByLanguage,
      Record = PancakeSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Tree,
      Name = "TREE SORT",
      Subtitle = "ORDENACIÓN POR ÁRBOL BINARIO",
      BaseFileName = "tree_sort",
      HintCaption = "CONSTRUYE UN ÁRBOL BST Y LO RECORRE EN ORDEN",
      ActionLabel = "ESCRIBE",
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
