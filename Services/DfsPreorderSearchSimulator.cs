using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records a Preorder (root, left, right) DFS search — a deliberate contrast
/// with <see cref="BstSearchSimulator"/>: this walk checks *every* reachable
/// node in a fixed visiting order and never uses the BST's ordering property
/// to skip a subtree, so it degrades to O(n) even though the tree underneath
/// happens to be a search tree.
///
/// Implemented as a real recursive walk with early-exit propagated through
/// the return value (matching the C reference below) rather than an explicit
/// stack, since the visiting order it produces is what matters here, not the
/// mechanics of simulating recursion iteratively.
///
/// <code>
///  1  bool preorder_search(node *root, int target)
///  2  {
///  3      if (root == NULL) {
///  4          return false;
///  5      }
///  6      if (root-&gt;value == target) {
///  7          return true;
///  8      }
///  9      if (preorder_search(root-&gt;left, target)) {
/// 10          return true;
/// 11      }
/// 12      return preorder_search(root-&gt;right, target);
/// 13  }
/// </code>
/// </summary>
public static class DfsPreorderSearchSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-type\">bool</span> <span class=\"tok-fn\">preorder_search</span>(<span class=\"tok-type\">node</span> *root, <span class=\"tok-type\">int</span> target)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">NULL</span>) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root-&gt;value == target) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (<span class=\"tok-fn\">preorder_search</span>(root-&gt;left, target)) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">preorder_search</span>(root-&gt;right, target);",
    "}"
  ];

  public static TreeSearchResult Record(
    int[] values,
    int target
  )
  {
    var (nodes, rootId) = BstBuilder.Build(values);
    var steps = new List<TreeSearchStep>();
    var visited = new List<int>();
    var compareCount = 0;

    bool Visit(
      int? id
    )
    {
      if (id is null)
      {
        return false;
      }

      var node = nodes[id.Value];
      compareCount++;
      visited.Add(id.Value);

      if (node.Value == target)
      {
        steps.Add(
          new TreeSearchStep
          {
            Type = TreeSearchStepType.Found,
            CurrentNodeId = id,
            VisitedNodeIds = [.. visited],
            CompareCount = compareCount,
            VisitCount = visited.Count,
            ActiveCodeLines = [6, 7],
            Caption = $"Ghé node {node.Value}  →  đúng mục tiêu {target}, tìm thấy!"
          }
        );

        return true;
      }

      steps.Add(
        new TreeSearchStep
        {
          Type = TreeSearchStepType.Skip,
          CurrentNodeId = id,
          VisitedNodeIds = [.. visited],
          CompareCount = compareCount,
          VisitCount = visited.Count,
          ActiveCodeLines = [6],
          Caption = $"Ghé node {node.Value}  →  chưa khớp {target}, tiếp tục duyệt."
        }
      );

      if (Visit(node.LeftId))
      {
        return true;
      }

      return Visit(node.RightId);
    }

    var found = Visit(rootId);

    if (!found)
    {
      steps.Add(
        new TreeSearchStep
        {
          Type = TreeSearchStepType.NotFound,
          CurrentNodeId = null,
          VisitedNodeIds = [.. visited],
          CompareCount = compareCount,
          VisitCount = visited.Count,
          ActiveCodeLines = [3, 4],
          Caption = $"Đã duyệt hết cây theo preorder — không tìm thấy {target}."
        }
      );
    }

    return new TreeSearchResult(nodes, rootId, steps);
  }
}
