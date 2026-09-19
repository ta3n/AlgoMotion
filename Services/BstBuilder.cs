using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Builds a plain (unbalanced) binary search tree by inserting values one at
/// a time, in array order — the same shape a naive BST would end up with in
/// production code. Shared by every search simulator except
/// <see cref="AvlSearchSimulator"/>, which needs a self-balancing builder
/// instead (see <see cref="AvlBuilder"/>).
/// </summary>
public static class BstBuilder
{
  public static (List<TreeNode> Nodes, int? RootId) Build(
    IReadOnlyList<int> values
  )
  {
    var nodes = new List<TreeNode>();
    int? rootId = null;

    foreach (var value in values)
    {
      if (rootId is null)
      {
        rootId = AddNode(nodes, value, null, 0);
        continue;
      }

      var currentId = rootId.Value;
      while (true)
      {
        var current = nodes[currentId];

        if (value < current.Value)
        {
          if (current.LeftId is null)
          {
            current.LeftId = AddNode(nodes, value, currentId, current.Depth + 1);
            break;
          }

          currentId = current.LeftId.Value;
        }
        else
        {
          if (current.RightId is null)
          {
            current.RightId = AddNode(nodes, value, currentId, current.Depth + 1);
            break;
          }

          currentId = current.RightId.Value;
        }
      }
    }

    AssignInorderIndices(nodes, rootId);
    return (nodes, rootId);
  }

  private static int AddNode(
    List<TreeNode> nodes,
    int value,
    int? parentId,
    int depth
  )
  {
    var id = nodes.Count;
    nodes.Add(
      new TreeNode
      {
        Id = id,
        Value = value,
        ParentId = parentId,
        Depth = depth
      }
    );
    return id;
  }

  /// <summary>Walks the finished tree in order (left, self, right), numbering nodes 0, 1, 2, ... as
  /// they're visited — since a BST's in-order walk visits values ascending, this directly becomes
  /// each node's horizontal rank for <see cref="TreeLayout"/>. Shared with <see cref="AvlBuilder"/>,
  /// which needs the exact same numbering after its rotations settle.</summary>
  public static void AssignInorderIndices(
    List<TreeNode> nodes,
    int? rootId
  )
  {
    var counter = 0;
    Visit(rootId);
    return;

    void Visit(
      int? id
    )
    {
      if (id is null)
      {
        return;
      }

      var node = nodes[id.Value];
      Visit(node.LeftId);
      node.InorderIndex = counter++;
      Visit(node.RightId);
    }
  }
}
