using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records BST Search: use the binary-search-tree property itself to skip
/// whole subtrees — smaller than the current node means the answer (if any)
/// can only be in the left subtree, bigger means only the right, so at most
/// one child is ever visited per level. That's what makes it O(log n) on a
/// balanced tree, in contrast to the DFS/BFS traversal searches, which check
/// every reachable node because they don't use the tree's ordering at all.
///
/// <code>
///  1  node *bst_search(node *root, int target)
///  2  {
///  3      node *current = root;
///  4      while (current != NULL) {
///  5          if (target == current-&gt;value) {
///  6              return current;
///  7          }
///  8          if (target &lt; current-&gt;value) {
///  9              current = current-&gt;left;
/// 10          } else {
/// 11              current = current-&gt;right;
/// 12          }
/// 13      }
/// 14      return NULL;
/// 15  }
/// </code>
/// </summary>
public static class BstSearchSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-type\">node</span> *<span class=\"tok-fn\">bst_search</span>(<span class=\"tok-type\">node</span> *root, <span class=\"tok-type\">int</span> target)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">node</span> *current = root;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (current != <span class=\"tok-kw\">NULL</span>) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (target == current-&gt;value) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> current;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (target &lt; current-&gt;value) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;current = current-&gt;left;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;} <span class=\"tok-kw\">else</span> {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;current = current-&gt;right;",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">NULL</span>;",
    "}"
  ];

  public static TreeSearchResult Record(
    int[] values,
    int target
  )
  {
    var (nodes, rootId) = BstBuilder.Build(values);
    return Search(nodes, rootId, target);
  }

  /// <summary>Shared with <see cref="AvlSearchSimulator"/> — once a tree is built, walking down by
  /// comparing against <c>target</c> is identical whether the tree happens to be balanced or not.</summary>
  internal static TreeSearchResult Search(
    List<TreeNode> nodes,
    int? rootId,
    int target
  )
  {
    var steps = new List<TreeSearchStep>();
    var visited = new List<int>();
    var compareCount = 0;

    var currentId = rootId;
    while (currentId is not null)
    {
      var node = nodes[currentId.Value];
      compareCount++;
      visited.Add(currentId.Value);

      if (target == node.Value)
      {
        steps.Add(
          new TreeSearchStep
          {
            Type = TreeSearchStepType.Found,
            CurrentNodeId = currentId,
            VisitedNodeIds = [.. visited],
            CompareCount = compareCount,
            VisitCount = visited.Count,
            ActiveCodeLines = [5, 6],
            Caption = $"So sánh {target} với node {node.Value}  →  bằng nhau, tìm thấy!"
          }
        );

        return new TreeSearchResult(nodes, rootId, steps);
      }

      var goLeft = target < node.Value;

      steps.Add(
        new TreeSearchStep
        {
          Type = goLeft ? TreeSearchStepType.GoLeft : TreeSearchStepType.GoRight,
          CurrentNodeId = currentId,
          VisitedNodeIds = [.. visited],
          CompareCount = compareCount,
          VisitCount = visited.Count,
          ActiveCodeLines = goLeft ? [8, 9] : [10, 11],
          Caption = $"So sánh {target} với node {node.Value}"
            + (goLeft ? "  →  nhỏ hơn, đi sang trái" : "  →  lớn hơn, đi sang phải")
        }
      );

      currentId = goLeft ? node.LeftId : node.RightId;
    }

    steps.Add(
      new TreeSearchStep
      {
        Type = TreeSearchStepType.NotFound,
        CurrentNodeId = null,
        VisitedNodeIds = [.. visited],
        CompareCount = compareCount,
        VisitCount = visited.Count,
        ActiveCodeLines = [13, 14],
        Caption = $"Gặp nhánh rỗng — không tìm thấy {target} trong cây."
      }
    );

    return new TreeSearchResult(nodes, rootId, steps);
  }
}
