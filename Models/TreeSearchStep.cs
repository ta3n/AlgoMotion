namespace AlgoMotion.Models;

/// <summary>
/// The kind of event a single recorded tree-search step represents. Mirrors
/// <see cref="StepType"/>'s role for sorting: the UI uses this to decide
/// timing and which code lines to light up.
/// </summary>
public enum TreeSearchStepType
{
  /// <summary>Arrived at a node and compared it against the target.</summary>
  Visit,

  /// <summary>BST/AVL search: target is smaller — descending into the left subtree.</summary>
  GoLeft,

  /// <summary>BST/AVL search: target is bigger — descending into the right subtree.</summary>
  GoRight,

  /// <summary>Traversal search (DFS/BFS): this node isn't the target, keep going.</summary>
  Skip,

  /// <summary>Target found.</summary>
  Found,

  /// <summary>Search exhausted every reachable node without finding the target.</summary>
  NotFound
}

/// <summary>
/// One immutable frame of a tree-search animation. Mirrors <see cref="SortStep"/>'s
/// role for the sorting visualizer: each simulator runs its whole search up
/// front and records every frame as a <see cref="TreeSearchStep"/>; the UI
/// never re-implements the algorithm, it only plays this list back.
/// </summary>
public sealed class TreeSearchStep
{
  public TreeSearchStepType Type { get; set; }

  /// <summary>The node currently under the spotlight, or null once the search has ended without
  /// landing on a specific node (e.g. <see cref="TreeSearchStepType.NotFound"/>).</summary>
  public int? CurrentNodeId { get; set; }

  /// <summary>Every node visited so far, including this step's — dimmed/checked-off in the UI.</summary>
  public int[] VisitedNodeIds { get; set; } = [];

  /// <summary>BFS only: nodes currently waiting in the queue, not yet visited.</summary>
  public int[] QueuedNodeIds { get; set; } = [];

  public int CompareCount { get; set; }

  /// <summary>How many nodes have been visited so far (including this step's).</summary>
  public int VisitCount { get; set; }

  /// <summary>1-based source line numbers to highlight in the code panel.</summary>
  public int[] ActiveCodeLines { get; set; } = [];

  /// <summary>Human readable caption shown under the code panel.</summary>
  public string Caption { get; set; } = "";
}
