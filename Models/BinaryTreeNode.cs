namespace AlgoMotion.Models;

/// <summary>
/// Basic mutable node for binary tree / BST algorithms.
/// </summary>
public sealed class BinaryTreeNode
{
  public BinaryTreeNode(
    int value
  )
  {
    Value = value;
  }

  public int Value { get; set; }

  public BinaryTreeNode? Left { get; set; }

  public BinaryTreeNode? Right { get; set; }
}
