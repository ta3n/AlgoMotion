using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Builds a self-balancing AVL tree — standard recursive insertion with the
/// four rotation cases (LL, RR, LR, RL), keeping every subtree's left/right
/// height difference within [-1, 1]. Unlike <see cref="BstBuilder"/>, the
/// resulting shape never depends on insertion order: depth stays O(log n)
/// even for already-sorted input, which is the whole point of
/// <see cref="AvlSearchSimulator"/> existing alongside plain BST search.
///
/// Rotations move whole subtrees around, which invalidates the
/// <see cref="TreeNode.Depth"/>/<see cref="TreeNode.ParentId"/> assigned at
/// insertion time — <see cref="FixParentPointers"/> walks the finished tree
/// once to recompute both before layout/search ever look at them.
/// </summary>
public static class AvlBuilder
{
  public static (List<TreeNode> Nodes, int? RootId) Build(
    IReadOnlyList<int> values
  )
  {
    var nodes = new List<TreeNode>();
    var heights = new Dictionary<int, int>();
    int? rootId = null;

    foreach (var value in values)
    {
      rootId = Insert(nodes, heights, rootId, value);
    }

    FixParentPointers(nodes, rootId, parentId: null, depth: 0);
    BstBuilder.AssignInorderIndices(nodes, rootId);
    return (nodes, rootId);
  }

  private static int Height(
    Dictionary<int, int> heights,
    int? id
  )
  {
    return id is null ? -1 : heights[id.Value];
  }

  private static int Insert(
    List<TreeNode> nodes,
    Dictionary<int, int> heights,
    int? id,
    int value
  )
  {
    if (id is null)
    {
      var newId = nodes.Count;
      nodes.Add(new TreeNode { Id = newId, Value = value });
      heights[newId] = 0;
      return newId;
    }

    var node = nodes[id.Value];

    if (value < node.Value)
    {
      node.LeftId = Insert(nodes, heights, node.LeftId, value);
    }
    else
    {
      node.RightId = Insert(nodes, heights, node.RightId, value);
    }

    heights[id.Value] = 1 + Math.Max(Height(heights, node.LeftId), Height(heights, node.RightId));
    var balance = Height(heights, node.LeftId) - Height(heights, node.RightId);

    // Left-heavy: the new value landed in the left subtree's left side (LL) or its right side (LR).
    if (balance > 1)
    {
      var leftChildValue = nodes[node.LeftId!.Value].Value;
      return value < leftChildValue
        ? RotateRight(nodes, heights, id.Value)
        : RotateLeftRight(nodes, heights, id.Value);
    }

    // Right-heavy: the new value landed in the right subtree's right side (RR) or its left side (RL).
    if (balance < -1)
    {
      var rightChildValue = nodes[node.RightId!.Value].Value;
      return value >= rightChildValue
        ? RotateLeft(nodes, heights, id.Value)
        : RotateRightLeft(nodes, heights, id.Value);
    }

    return id.Value;
  }

  private static int RotateLeftRight(
    List<TreeNode> nodes,
    Dictionary<int, int> heights,
    int zId
  )
  {
    var z = nodes[zId];
    z.LeftId = RotateLeft(nodes, heights, z.LeftId!.Value);
    return RotateRight(nodes, heights, zId);
  }

  private static int RotateRightLeft(
    List<TreeNode> nodes,
    Dictionary<int, int> heights,
    int zId
  )
  {
    var z = nodes[zId];
    z.RightId = RotateRight(nodes, heights, z.RightId!.Value);
    return RotateLeft(nodes, heights, zId);
  }

  private static int RotateRight(
    List<TreeNode> nodes,
    Dictionary<int, int> heights,
    int yId
  )
  {
    var y = nodes[yId];
    var xId = y.LeftId!.Value;
    var x = nodes[xId];
    var handedOver = x.RightId;

    x.RightId = yId;
    y.LeftId = handedOver;

    heights[yId] = 1 + Math.Max(Height(heights, y.LeftId), Height(heights, y.RightId));
    heights[xId] = 1 + Math.Max(Height(heights, x.LeftId), Height(heights, x.RightId));

    return xId;
  }

  private static int RotateLeft(
    List<TreeNode> nodes,
    Dictionary<int, int> heights,
    int xId
  )
  {
    var x = nodes[xId];
    var yId = x.RightId!.Value;
    var y = nodes[yId];
    var handedOver = y.LeftId;

    y.LeftId = xId;
    x.RightId = handedOver;

    heights[xId] = 1 + Math.Max(Height(heights, x.LeftId), Height(heights, x.RightId));
    heights[yId] = 1 + Math.Max(Height(heights, y.LeftId), Height(heights, y.RightId));

    return yId;
  }

  private static void FixParentPointers(
    List<TreeNode> nodes,
    int? id,
    int? parentId,
    int depth
  )
  {
    if (id is null)
    {
      return;
    }

    var node = nodes[id.Value];
    node.ParentId = parentId;
    node.Depth = depth;
    FixParentPointers(nodes, node.LeftId, id, depth + 1);
    FixParentPointers(nodes, node.RightId, id, depth + 1);
  }
}
