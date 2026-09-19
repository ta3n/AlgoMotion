using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records a Postorder (left, right, root) DFS search — checks a node's
/// entire left and right subtrees before ever comparing the node itself, so
/// this is the "latest possible" of the three DFS orders to spot a match
/// sitting near the root. Like <see cref="DfsPreorderSearchSimulator"/> and
/// <see cref="DfsInorderSearchSimulator"/>, it visits every reachable node
/// and ignores the BST ordering property entirely — O(n) regardless of the
/// tree's shape.
///
/// <code>
///  1  bool postorder_search(node *root, int target)
///  2  {
///  3      if (root == NULL) {
///  4          return false;
///  5      }
///  6      if (postorder_search(root-&gt;left, target)) {
///  7          return true;
///  8      }
///  9      if (postorder_search(root-&gt;right, target)) {
/// 10          return true;
/// 11      }
/// 12      if (root-&gt;value == target) {
/// 13          return true;
/// 14      }
/// 15      return false;
/// 16  }
/// </code>
/// </summary>
public static class DfsPostorderSearchSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        "<span class=\"tok-type\">bool</span> <span class=\"tok-fn\">postorder_search</span>(<span class=\"tok-type\">node</span> *root, <span class=\"tok-type\">int</span> target)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">NULL</span>) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (<span class=\"tok-fn\">postorder_search</span>(root-&gt;left, target)) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (<span class=\"tok-fn\">postorder_search</span>(root-&gt;right, target)) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root-&gt;value == target) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        "// Node contains an integer Value and nullable Left/Right children.",
        "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">bool</span> <span class=\"tok-fn\">PostorderSearch</span>(<span class=\"tok-type\">Node</span>? root, <span class=\"tok-type\">int</span> target)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">PostorderSearch</span>(root.Left, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| <span class=\"tok-fn\">PostorderSearch</span>(root.Right, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| root.Value == target;",
        "}"
      ],
      [CodeLanguage.Java] =
      [
        "// Node contains an int value and nullable left/right children.",
        "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">boolean</span> <span class=\"tok-fn\">postorderSearch</span>(<span class=\"tok-type\">Node</span> root, <span class=\"tok-type\">int</span> target) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">postorderSearch</span>(root.left, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| <span class=\"tok-fn\">postorderSearch</span>(root.right, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| root.value == target;",
        "}"
      ],
      [CodeLanguage.Python] =
      [
        "# Nodes have value, left, and right attributes; missing children are None.",
        "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">postorder_search</span>(root, target):",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">None</span>:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">False</span>",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> (<span class=\"tok-fn\">postorder_search</span>(root.left, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">or</span> <span class=\"tok-fn\">postorder_search</span>(root.right, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">or</span> root.value == target)"
      ],
      [CodeLanguage.TypeScript] =
      [
        "<span class=\"tok-kw\">interface</span> <span class=\"tok-type\">TreeNode</span> {",
        "&nbsp;&nbsp;&nbsp;&nbsp;value: <span class=\"tok-type\">number</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;left: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;right: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>;",
        "}",
        "",
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">postorderSearch</span>(root: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>, target: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">boolean</span> {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">postorderSearch</span>(root.left, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| <span class=\"tok-fn\">postorderSearch</span>(root.right, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| root.value === target;",
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        "// Nodes have value, left, and right properties; missing children are null.",
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">postorderSearch</span>(root, target) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">postorderSearch</span>(root.left, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| <span class=\"tok-fn\">postorderSearch</span>(root.right, target)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| root.value === target;",
        "}"
      ]
    };

  public static TreeSearchResult Record(
    int[] values,
    int target,
    UiLanguage language
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

      if (Visit(node.RightId))
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
            ActiveCodeLines = [12, 13],
            Caption = Res.Caption("DfsVisit_Found", language, node.Value, target)
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
          ActiveCodeLines = [12],
          Caption = Res.Caption("DfsVisit_Skip", language, node.Value, target)
        }
      );

      return false;
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
          Caption = Res.Caption("DfsVisit_NotFoundPostorder", language, target)
        }
      );
    }

    return new TreeSearchResult(nodes, rootId, steps);
  }
}
