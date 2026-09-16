using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records Tree Sort: insert every element into a binary search tree, one at
/// a time, then read the sorted order back out with an in-order traversal.
///
/// Each tree node remembers the original array index its value came from,
/// so while the tree is being built nothing actually moves in the array —
/// insertion only *compares* values, it never swaps or writes. That lets
/// <see cref="StepType.Compare"/> point the crane at two genuinely stable,
/// meaningful positions (the node being visited and the value being
/// inserted) even though this phase's outcome is an invisible tree, not a
/// rearranged array. Once every element has been inserted, the in-order
/// traversal writes each value straight into its final sorted slot — the
/// exact same <see cref="StepType.CountPlace"/> vocabulary
/// <see cref="CountingSortSimulator"/>/<see cref="RadixSortSimulator"/> use
/// for "no comparison, just place it" writes. Because those write steps only
/// set <see cref="SortStep.RightIndex"/>, the crane naturally has nothing to
/// point at and hides itself — no extra logic needed for the phase change,
/// so <see cref="SortAlgorithmInfo.ShowCrane"/> can stay true throughout.
///
/// <code>
///  1  typedef struct node {
///  2      int value;
///  3      struct node *left, *right;
///  4  } node;
///  5
///  6  node *insert(node *root, int value)
///  7  {
///  8      if (root == NULL) {
///  9          return new_node(value);
/// 10      }
/// 11      if (value &lt; root-&gt;value) {
/// 12          root-&gt;left = insert(root-&gt;left, value);
/// 13      } else {
/// 14          root-&gt;right = insert(root-&gt;right, value);
/// 15      }
/// 16      return root;
/// 17  }
/// 18
/// 19  void in_order(node *root, int a[], int *k)
/// 20  {
/// 21      if (root == NULL) return;
/// 22      in_order(root-&gt;left, a, k);
/// 23      a[(*k)++] = root-&gt;value;
/// 24      in_order(root-&gt;right, a, k);
/// 25  }
/// 26
/// 27  void tree_sort(int a[], size_t n)
/// 28  {
/// 29      node *root = NULL;
/// 30      for (size_t i = 0; i &lt; n; i++) {
/// 31          root = insert(root, a[i]);
/// 32      }
/// 33      int k = 0;
/// 34      in_order(root, a, &amp;k);
/// 35  }
/// </code>
/// </summary>
public static class TreeSortSimulator
{
  public static readonly string[] CodeLines =
  [
    "<span class=\"tok-kw\">typedef</span> <span class=\"tok-kw\">struct</span> <span class=\"tok-type\">node</span> {",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> value;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">struct</span> <span class=\"tok-type\">node</span> *left, *right;",
    "} <span class=\"tok-type\">node</span>;",
    "",
    "<span class=\"tok-type\">node</span> *<span class=\"tok-fn\">insert</span>(<span class=\"tok-type\">node</span> *root, <span class=\"tok-type\">int</span> value)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">NULL</span>) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">new_node</span>(value);",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; root-&gt;value) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root-&gt;left = <span class=\"tok-fn\">insert</span>(root-&gt;left, value);",
    "&nbsp;&nbsp;&nbsp;&nbsp;} <span class=\"tok-kw\">else</span> {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root-&gt;right = <span class=\"tok-fn\">insert</span>(root-&gt;right, value);",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> root;",
    "}",
    "",
    "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">in_order</span>(<span class=\"tok-type\">node</span> *root, <span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">int</span> *k)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">NULL</span>) <span class=\"tok-kw\">return</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">in_order</span>(root-&gt;left, a, k);",
    "&nbsp;&nbsp;&nbsp;&nbsp;a[(*k)++] = root-&gt;value;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">in_order</span>(root-&gt;right, a, k);",
    "}",
    "",
    "<span class=\"tok-type\">void</span> <span class=\"tok-fn\">tree_sort</span>(<span class=\"tok-type\">int</span> a[], <span class=\"tok-type\">size_t</span> n)",
    "{",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">node</span> *root = <span class=\"tok-kw\">NULL</span>;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">size_t</span> i = 0; i &lt; n; i++) {",
    "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root = <span class=\"tok-fn\">insert</span>(root, a[i]);",
    "&nbsp;&nbsp;&nbsp;&nbsp;}",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> k = 0;",
    "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">in_order</span>(root, a, &amp;k);",
    "}"
  ];

  public static List<SortStep> Record(
    IReadOnlyList<int> input
  )
  {
    var a = input.ToArray();
    var n = a.Length;
    var steps = new List<SortStep>();
    var sorted = new SortedSet<int>();

    var compareCount = 0;
    var writeCount = 0;

    Node? root = null;

    for (var i = 0; i < n; i++)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.StartPass,
          Snapshot = [.. a],
          I = i,
          CompareCount = compareCount,
          SwapCount = writeCount,
          RightIndex = i,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [30, 31],
          Caption = $"Chèn a[{i}] = {a[i]} vào cây nhị phân tìm kiếm."
        }
      );

      root = Insert(root, a[i], i, a, steps, sorted, ref compareCount, writeCount);
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.EndPass,
        Snapshot = [.. a],
        CompareCount = compareCount,
        SwapCount = writeCount,
        SortedIndices = [.. sorted],
        ActiveCodeLines = [33, 34],
        Caption = "Cây đã dựng xong — duyệt in-order (trái → gốc → phải) để lấy dãy đã sắp xếp."
      }
    );

    var visited = new List<(int SourceIndex, int Value)>();
    InOrder(root, visited);

    var output = new int[n];
    for (var idx = 0; idx < visited.Count; idx++)
    {
      output[idx] = visited[idx].Value;
    }

    for (var idx = 0; idx < visited.Count; idx++)
    {
      writeCount++;
      sorted.Add(idx);

      steps.Add(
        new SortStep
        {
          Type = StepType.CountPlace,
          Snapshot = output,
          CompareCount = compareCount,
          SwapCount = writeCount,
          RightIndex = idx,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [23],
          Caption = $"Duyệt in-order: đặt {visited[idx].Value} (từ a[{visited[idx].SourceIndex}] ban đầu) vào a[{idx}]."
        }
      );
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.Completed,
        Snapshot = output,
        CompareCount = compareCount,
        SwapCount = writeCount,
        SortedIndices = [.. sorted],
        ActiveCodeLines = [27, 28],
        Caption = "Hoàn tất! Dãy đã được sắp xếp."
      }
    );

    return steps;
  }

  /// <summary>Walks down from <paramref name="node"/> comparing against <paramref name="value"/>
  /// at each visited node (recording one Compare step per node), then attaches a new node once
  /// an empty spot is found. Returns the (possibly new) subtree root.</summary>
  private static Node Insert(
    Node? node,
    int value,
    int originalIndex,
    int[] a,
    List<SortStep> steps,
    SortedSet<int> sorted,
    ref int compareCount,
    int writeCount
  )
  {
    if (node is null)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.NoSwap,
          Snapshot = [.. a],
          CompareCount = compareCount,
          SwapCount = writeCount,
          RightIndex = originalIndex,
          SortedIndices = [.. sorted],
          ActiveCodeLines = [8, 9],
          Caption = $"Gặp chỗ trống trong cây — gắn node mới cho giá trị {value} tại đây."
        }
      );

      return new Node { Value = value, OriginalIndex = originalIndex };
    }

    compareCount++;
    var goLeft = value < node.Value;

    steps.Add(
      new SortStep
      {
        Type = StepType.Compare,
        Snapshot = [.. a],
        CompareCount = compareCount,
        SwapCount = writeCount,
        LeftIndex = node.OriginalIndex,
        RightIndex = originalIndex,
        SortedIndices = [.. sorted],
        ActiveCodeLines = goLeft ? [11, 12] : [11, 13, 14],
        Caption = $"So sánh {value} với node hiện tại a[{node.OriginalIndex}] = {node.Value}"
          + (goLeft ? "  →  nhỏ hơn, đi sang trái" : "  →  lớn hơn, đi sang phải")
      }
    );

    if (goLeft)
    {
      node.Left = Insert(node.Left, value, originalIndex, a, steps, sorted, ref compareCount, writeCount);
    }
    else
    {
      node.Right = Insert(node.Right, value, originalIndex, a, steps, sorted, ref compareCount, writeCount);
    }

    return node;
  }

  /// <summary>In-order traversal (left, root, right) — visits nodes in ascending value order.</summary>
  private static void InOrder(
    Node? node,
    List<(int SourceIndex, int Value)> visited
  )
  {
    if (node is null)
    {
      return;
    }

    InOrder(node.Left, visited);
    visited.Add((node.OriginalIndex, node.Value));
    InOrder(node.Right, visited);
  }

  private sealed class Node
  {
    public required int Value { get; init; }

    public required int OriginalIndex { get; init; }

    public Node? Left { get; set; }

    public Node? Right { get; set; }
  }
}
