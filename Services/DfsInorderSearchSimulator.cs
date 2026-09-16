using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records an Inorder (left, root, right) DFS search — visits every reachable
/// node in ascending value order, the same order <see cref="BstBuilder.AssignInorderIndices"/>
/// uses for layout, but here it's the *algorithm's* visiting order rather
/// than a one-off numbering pass. Like <see cref="DfsPreorderSearchSimulator"/>,
/// this never uses the BST's ordering property to skip a subtree, so it's
/// O(n) even on a search tree — only the order nodes are checked in differs
/// from Preorder/Postorder.
///
/// <code>
///  1  bool inorder_search(node *root, int target)
///  2  {
///  3      if (root == NULL) {
///  4          return false;
///  5      }
///  6      if (inorder_search(root-&gt;left, target)) {
///  7          return true;
///  8      }
///  9      if (root-&gt;value == target) {
/// 10          return true;
/// 11      }
/// 12      return inorder_search(root-&gt;right, target);
/// 13  }
/// </code>
/// </summary>
public static class DfsInorderSearchSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-type\">bool</span> <span class=\"tok-fn\">inorder_search</span>(<span class=\"tok-type\">node</span> *root, <span class=\"tok-type\">int</span> target)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">NULL</span>) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (<span class=\"tok-fn\">inorder_search</span>(root-&gt;left, target)) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root-&gt;value == target) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">inorder_search</span>(root-&gt;right, target);",
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

      if (Visit(node.LeftId))
      {
        return true;
      }

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
            ActiveCodeLines = [9, 10],
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
          ActiveCodeLines = [9],
          Caption = $"Ghé node {node.Value}  →  chưa khớp {target}, tiếp tục duyệt."
        }
      );

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
          Caption = $"Đã duyệt hết cây theo inorder — không tìm thấy {target}."
        }
      );
    }

    return new TreeSearchResult(nodes, rootId, steps);
  }
}
