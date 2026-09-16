using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Common tree algorithms (traversal + basic BST operations).
/// </summary>
public static class TreeAlgorithms
{
  public static List<int> Preorder(
    BinaryTreeNode? root
  )
  {
    var result = new List<int>();
    Preorder(root, result);
    return result;
  }

  public static List<int> Inorder(
    BinaryTreeNode? root
  )
  {
    var result = new List<int>();
    Inorder(root, result);
    return result;
  }

  public static List<int> Postorder(
    BinaryTreeNode? root
  )
  {
    var result = new List<int>();
    Postorder(root, result);
    return result;
  }

  public static List<int> LevelOrder(
    BinaryTreeNode? root
  )
  {
    var result = new List<int>();
    if (root is null) return result;

    var queue = new Queue<BinaryTreeNode>();
    queue.Enqueue(root);
    while (queue.Count > 0)
    {
      var current = queue.Dequeue();
      result.Add(current.Value);

      if (current.Left is not null) queue.Enqueue(current.Left);
      if (current.Right is not null) queue.Enqueue(current.Right);
    }

    return result;
  }

  public static BinaryTreeNode? BuildBst(
    IEnumerable<int> values
  )
  {
    ArgumentNullException.ThrowIfNull(values);

    BinaryTreeNode? root = null;
    foreach (var value in values)
    {
      root = InsertBst(root, value);
    }

    return root;
  }

  public static BinaryTreeNode InsertBst(
    BinaryTreeNode? root,
    int value
  )
  {
    if (root is null) return new BinaryTreeNode(value);

    if (value < root.Value) root.Left = InsertBst(root.Left, value);
    else root.Right = InsertBst(root.Right, value);

    return root;
  }

  public static bool ContainsBst(
    BinaryTreeNode? root,
    int value
  )
  {
    var current = root;
    while (current is not null)
    {
      if (value == current.Value) return true;
      current = value < current.Value ? current.Left : current.Right;
    }

    return false;
  }

  public static BinaryTreeNode? DeleteBst(
    BinaryTreeNode? root,
    int value
  )
  {
    if (root is null) return null;

    if (value < root.Value)
    {
      root.Left = DeleteBst(root.Left, value);
      return root;
    }

    if (value > root.Value)
    {
      root.Right = DeleteBst(root.Right, value);
      return root;
    }

    if (root.Left is null) return root.Right;
    if (root.Right is null) return root.Left;

    var successor = FindMin(root.Right);
    root.Value = successor.Value;
    root.Right = DeleteBst(root.Right, successor.Value);
    return root;
  }

  private static BinaryTreeNode FindMin(
    BinaryTreeNode node
  )
  {
    var current = node;
    while (current.Left is not null)
    {
      current = current.Left;
    }

    return current;
  }

  private static void Preorder(
    BinaryTreeNode? node,
    List<int> output
  )
  {
    if (node is null) return;

    output.Add(node.Value);
    Preorder(node.Left, output);
    Preorder(node.Right, output);
  }

  private static void Inorder(
    BinaryTreeNode? node,
    List<int> output
  )
  {
    if (node is null) return;

    Inorder(node.Left, output);
    output.Add(node.Value);
    Inorder(node.Right, output);
  }

  private static void Postorder(
    BinaryTreeNode? node,
    List<int> output
  )
  {
    if (node is null) return;

    Postorder(node.Left, output);
    Postorder(node.Right, output);
    output.Add(node.Value);
  }
}
