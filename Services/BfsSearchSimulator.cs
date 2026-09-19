using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records a Breadth-First (level-order) search: a FIFO queue holds every
/// node waiting to be checked, so the whole root level is visited before any
/// grandchild — the opposite exploration order from the DFS variants, which
/// dive to the bottom of one subtree before backing out. Like the DFS
/// searches, it ignores the BST ordering property and checks every reachable
/// node, so it's O(n).
///
/// <see cref="TreeSearchStep.QueuedNodeIds"/> is only populated by this
/// simulator — it's what lets the UI ring nodes that are waiting in the
/// queue, the one piece of BFS-specific state none of the other algorithms
/// need.
///
/// <code>
///  1  bool bfs_search(node *root, int target)
///  2  {
///  3      queue q;
///  4      enqueue(&amp;q, root);
///  5      while (!queue_empty(&amp;q)) {
///  6          node *current = dequeue(&amp;q);
///  7          if (current-&gt;value == target) {
///  8              return true;
///  9          }
/// 10          if (current-&gt;left != NULL) enqueue(&amp;q, current-&gt;left);
/// 11          if (current-&gt;right != NULL) enqueue(&amp;q, current-&gt;right);
/// 12      }
/// 13      return false;
/// 14  }
/// </code>
/// </summary>
public static class BfsSearchSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage =
    new Dictionary<CodeLanguage, string[]>
    {
      [CodeLanguage.C] =
      [
        "<span class=\"tok-type\">bool</span> <span class=\"tok-fn\">bfs_search</span>(<span class=\"tok-type\">node</span> *root, <span class=\"tok-type\">int</span> target)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">queue</span> q;",
        "&nbsp;&nbsp;&nbsp;&nbsp;enqueue(&amp;q, root);",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (!queue_empty(&amp;q)) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">node</span> *current = dequeue(&amp;q);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current-&gt;value == target) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current-&gt;left != <span class=\"tok-kw\">NULL</span>) enqueue(&amp;q, current-&gt;left);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current-&gt;right != <span class=\"tok-kw\">NULL</span>) enqueue(&amp;q, current-&gt;right);",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "}"
      ],
      [CodeLanguage.CSharp] =
      [
        "// Node contains an integer Value and nullable Left/Right children.",
        "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">bool</span> <span class=\"tok-fn\">BfsSearch</span>(<span class=\"tok-type\">Node</span>? root, <span class=\"tok-type\">int</span> target)",
        "{",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">var</span> queue = <span class=\"tok-kw\">new</span> System.Collections.Generic.Queue&lt;<span class=\"tok-type\">Node</span>&gt;();",
        "&nbsp;&nbsp;&nbsp;&nbsp;queue.<span class=\"tok-fn\">Enqueue</span>(root);",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (queue.Count &gt; 0) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">Node</span> current = queue.<span class=\"tok-fn\">Dequeue</span>();",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.Value == target) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.Left <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">not</span> <span class=\"tok-kw\">null</span>) queue.<span class=\"tok-fn\">Enqueue</span>(current.Left);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.Right <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">not</span> <span class=\"tok-kw\">null</span>) queue.<span class=\"tok-fn\">Enqueue</span>(current.Right);",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "}"
      ],
      [CodeLanguage.Java] =
      [
        "// Node contains an int value and nullable left/right children.",
        "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">boolean</span> <span class=\"tok-fn\">bfsSearch</span>(<span class=\"tok-type\">Node</span> root, <span class=\"tok-type\">int</span> target) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;java.util.Queue&lt;<span class=\"tok-type\">Node</span>&gt; queue = <span class=\"tok-kw\">new</span> java.util.ArrayDeque&lt;&gt;();",
        "&nbsp;&nbsp;&nbsp;&nbsp;queue.<span class=\"tok-fn\">add</span>(root);",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (!queue.<span class=\"tok-fn\">isEmpty</span>()) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">Node</span> current = queue.<span class=\"tok-fn\">remove</span>();",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.value == target) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.left != <span class=\"tok-kw\">null</span>) queue.<span class=\"tok-fn\">add</span>(current.left);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.right != <span class=\"tok-kw\">null</span>) queue.<span class=\"tok-fn\">add</span>(current.right);",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "}"
      ],
      [CodeLanguage.Python] =
      [
        "# Nodes have value, left, and right attributes; missing children are None.",
        "<span class=\"tok-kw\">from</span> collections <span class=\"tok-kw\">import</span> deque",
        "",
        "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">bfs_search</span>(root, target):",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">None</span>:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">False</span>",
        "&nbsp;&nbsp;&nbsp;&nbsp;queue = <span class=\"tok-fn\">deque</span>([root])",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> queue:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;current = queue.<span class=\"tok-fn\">popleft</span>()",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> current.value == target:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">True</span>",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> current.left <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">not</span> <span class=\"tok-kw\">None</span>:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;queue.<span class=\"tok-fn\">append</span>(current.left)",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> current.right <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">not</span> <span class=\"tok-kw\">None</span>:",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;queue.<span class=\"tok-fn\">append</span>(current.right)",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">False</span>"
      ],
      [CodeLanguage.TypeScript] =
      [
        "<span class=\"tok-kw\">interface</span> <span class=\"tok-type\">TreeNode</span> {",
        "&nbsp;&nbsp;&nbsp;&nbsp;value: <span class=\"tok-type\">number</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;left: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;right: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>;",
        "}",
        "",
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">bfsSearch</span>(root: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>, target: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">boolean</span> {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> queue: <span class=\"tok-type\">TreeNode</span>[] = [root];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> head = 0; head &lt; queue.length; head++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> current = queue[head]!;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.value === target) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.left !== <span class=\"tok-kw\">null</span>) queue.<span class=\"tok-fn\">push</span>(current.left);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.right !== <span class=\"tok-kw\">null</span>) queue.<span class=\"tok-fn\">push</span>(current.right);",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "}"
      ],
      [CodeLanguage.JavaScript] =
      [
        "// Nodes have value, left, and right properties; missing children are null.",
        "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">bfsSearch</span>(root, target) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> queue = [root];",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">let</span> head = 0; head &lt; queue.length; head++) {",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">const</span> current = queue[head];",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.value === target) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">true</span>;",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.left !== <span class=\"tok-kw\">null</span>) queue.<span class=\"tok-fn\">push</span>(current.left);",
        "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.right !== <span class=\"tok-kw\">null</span>) queue.<span class=\"tok-fn\">push</span>(current.right);",
        "&nbsp;&nbsp;&nbsp;&nbsp;}",
        "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">false</span>;",
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

    var queue = new Queue<int>();
    if (rootId is not null)
    {
      queue.Enqueue(rootId.Value);
    }

    while (queue.Count > 0)
    {
      var currentId = queue.Dequeue();
      var node = nodes[currentId];
      compareCount++;
      visited.Add(currentId);

      if (node.Value == target)
      {
        steps.Add(
          new TreeSearchStep
          {
            Type = TreeSearchStepType.Found,
            CurrentNodeId = currentId,
            VisitedNodeIds = [.. visited],
            QueuedNodeIds = [.. queue],
            CompareCount = compareCount,
            VisitCount = visited.Count,
            ActiveCodeLines = [7, 8],
            Caption = Res.Caption("Bfs_Found", language, node.Value, target)
          }
        );

        return new TreeSearchResult(nodes, rootId, steps);
      }

      var enqueuedCount = 0;
      if (node.LeftId is not null)
      {
        queue.Enqueue(node.LeftId.Value);
        enqueuedCount++;
      }

      if (node.RightId is not null)
      {
        queue.Enqueue(node.RightId.Value);
        enqueuedCount++;
      }

      steps.Add(
        new TreeSearchStep
        {
          Type = TreeSearchStepType.Skip,
          CurrentNodeId = currentId,
          VisitedNodeIds = [.. visited],
          QueuedNodeIds = [.. queue],
          CompareCount = compareCount,
          VisitCount = visited.Count,
          ActiveCodeLines = [7, 10, 11],
          Caption = enqueuedCount > 0
            ? Res.Caption("Bfs_SkipWithChildren", language, node.Value, target, enqueuedCount)
            : Res.Caption("Bfs_SkipNoChildren", language, node.Value, target)
        }
      );
    }

    steps.Add(
      new TreeSearchStep
      {
        Type = TreeSearchStepType.NotFound,
        CurrentNodeId = null,
        VisitedNodeIds = [.. visited],
        CompareCount = compareCount,
        VisitCount = visited.Count,
        ActiveCodeLines = [13],
        Caption = Res.Caption("Bfs_NotFound", language, target)
      }
    );

    return new TreeSearchResult(nodes, rootId, steps);
  }
}
