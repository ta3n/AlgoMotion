namespace AlgoMotion.Models;

/// <summary>
/// One node in a fixed binary tree structure, built once before a search
/// animation begins — search algorithms only ever read the tree, they never
/// mutate it. <see cref="Depth"/> and <see cref="InorderIndex"/> are the
/// precomputed layout coordinates <see cref="TreeLayout"/> turns into actual
/// pixel/percentage positions.
///
/// Every builder (see <c>Services/BstBuilder.cs</c>, <c>Services/AvlBuilder.cs</c>)
/// assigns <see cref="Id"/> as the node's index in the backing list, so any
/// <c>nodes[id]</c> lookup is a direct, O(1) array access — no dictionary
/// needed anywhere search or layout code touches the tree.
/// </summary>
public sealed class TreeNode
{
  public required int Id { get; init; }

  public required int Value { get; init; }

  public int? ParentId { get; set; }

  public int? LeftId { get; set; }

  public int? RightId { get; set; }

  /// <summary>Root is 0; each level below adds 1. Determines the node's vertical position.</summary>
  public int Depth { get; set; }

  /// <summary>This node's rank (0-based) in an in-order traversal of the whole tree. A BST's in-order
  /// traversal visits values in ascending order, so this doubles as "how many nodes sort before this
  /// one" and determines the node's horizontal position — the same trick <c>ChartLayout</c> uses to
  /// place sorted bars, just applied to tree nodes instead of array slots.</summary>
  public int InorderIndex { get; set; }
}
