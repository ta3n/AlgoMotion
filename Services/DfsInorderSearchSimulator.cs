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
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
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
    ],
    [CodeLanguage.CSharp] =
    [
      "// Node contains an integer Value and nullable Left/Right children.",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">bool</span> <span class=\"tok-fn\">InorderSearch</span>(<span class=\"tok-type\">Node</span>? root, <span class=\"tok-type\">int</span> target)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">InorderSearch</span>(root.Left, target)",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| root.Value == target",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| <span class=\"tok-fn\">InorderSearch</span>(root.Right, target);",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "// Node contains an int value and nullable left/right children.",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">boolean</span> <span class=\"tok-fn\">inorderSearch</span>(<span class=\"tok-type\">Node</span> root, <span class=\"tok-type\">int</span> target) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">inorderSearch</span>(root.left, target)",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| root.value == target",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| <span class=\"tok-fn\">inorderSearch</span>(root.right, target);",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "# Nodes have value, left, and right attributes; missing children are None.",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">inorder_search</span>(root, target):",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">None</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">False</span>",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> (<span class=\"tok-fn\">inorder_search</span>(root.left, target)",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">or</span> root.value == target",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">or</span> <span class=\"tok-fn\">inorder_search</span>(root.right, target))"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">interface</span> <span class=\"tok-type\">TreeNode</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;value: <span class=\"tok-type\">number</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;left: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;right: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>;",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">inorderSearch</span>(root: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>, target: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">boolean</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">inorderSearch</span>(root.left, target)",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| root.value === target",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| <span class=\"tok-fn\">inorderSearch</span>(root.right, target);",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "// Nodes have value, left, and right properties; missing children are null.",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">inorderSearch</span>(root, target) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">inorderSearch</span>(root.left, target)",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| root.value === target",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;|| <span class=\"tok-fn\">inorderSearch</span>(root.right, target);",
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
          ActiveCodeLines = [9],
          Caption = Res.Caption("DfsVisit_Skip", language, node.Value, target)
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
          Caption = Res.Caption("DfsVisit_NotFoundInorder", language, target)
        }
      );
    }

    return new TreeSearchResult(nodes, rootId, steps);
  }
}
