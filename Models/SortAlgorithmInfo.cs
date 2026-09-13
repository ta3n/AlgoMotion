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

  /// <summary>File name shown in the code panel's title bar, e.g. "bubble_sort.c".</summary>
  public required string FileName { get; init; }

  /// <summary>Small caption under the code panel describing the core idea.</summary>
  public required string HintCaption { get; init; }

  /// <summary>Label for the comparison counter — "COMPARA" for every algorithm here.</summary>
  public string CompareLabel { get; init; } = "COMPARA";

  /// <summary>Label for the mutation counter — usually "CAMBIA" (swaps); Merge Sort uses "ESCRIBE" (writes).</summary>
  public string ActionLabel { get; init; } = "CAMBIA";

  /// <summary>Pre-tokenized HTML source lines shown in the code panel, one entry per line.</summary>
  public required string[] CodeLines { get; init; }

  /// <summary>Runs the algorithm on a copy of the input and records every step.</summary>
  public required Func<int[], List<SortStep>> Record { get; init; }

  /// <summary>True for algorithms whose steps compare/swap a simple adjacent-ish pair — these get the crane.
  /// Merge Sort's compare/write steps don't map onto a stable pair in the live array, so it skips the crane
  /// and relies on the bars' own highlight + range dimming instead.</summary>
  public bool ShowCrane { get; init; } = true;
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
      FileName = "bubble_sort.c",
      HintCaption = "SOLO COMPARA VECINOS, DE DOS EN DOS",
      CodeLines = BubbleSortSimulator.CodeLines,
      Record = BubbleSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Selection,
      Name = "SELECTION SORT",
      Subtitle = "ORDENACIÓN POR SELECCIÓN",
      FileName = "selection_sort.c",
      HintCaption = "BUSCA EL MÍNIMO Y LO COLOCA AL FRENTE",
      CodeLines = SelectionSortSimulator.CodeLines,
      Record = SelectionSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Insertion,
      Name = "INSERTION SORT",
      Subtitle = "ORDENACIÓN POR INSERCIÓN",
      FileName = "insertion_sort.c",
      HintCaption = "TOMA UNA CLAVE Y LA DESLIZA A SU LUGAR",
      CodeLines = InsertionSortSimulator.CodeLines,
      Record = InsertionSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Quick,
      Name = "QUICK SORT",
      Subtitle = "ORDENACIÓN RÁPIDA",
      FileName = "quick_sort.c",
      HintCaption = "DIVIDE ALREDEDOR DE UN PIVOTE",
      CodeLines = QuickSortSimulator.CodeLines,
      Record = QuickSortSimulator.Record
    },
    new()
    {
      Kind = SortAlgorithmKind.Merge,
      Name = "MERGE SORT",
      Subtitle = "ORDENACIÓN POR MEZCLA",
      FileName = "merge_sort.c",
      HintCaption = "DIVIDE, ORDENA Y MEZCLA",
      ActionLabel = "ESCRIBE",
      CodeLines = MergeSortSimulator.CodeLines,
      Record = MergeSortSimulator.Record,
      ShowCrane = false
    }
  ];

  public static SortAlgorithmInfo Get(
    SortAlgorithmKind kind
  )
  {
    return All.First(a => a.Kind == kind);
  }
}
