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
  public static readonly string[] CodeLines =
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
            Caption = $"Lấy node {node.Value} ra khỏi hàng đợi  →  đúng mục tiêu {target}, tìm thấy!"
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
          Caption = $"Lấy node {node.Value} ra khỏi hàng đợi  →  chưa khớp {target}"
            + (enqueuedCount > 0 ? $", thêm {enqueuedCount} node con vào hàng đợi." : ", không có con để thêm.")
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
        Caption = $"Hàng đợi rỗng — không tìm thấy {target}."
      }
    );

    return new TreeSearchResult(nodes, rootId, steps);
  }
}
