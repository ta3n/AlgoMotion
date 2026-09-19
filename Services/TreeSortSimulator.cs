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
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
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
    ],
    [CodeLanguage.CSharp] =
    [
      "<span class=\"tok-kw\">sealed</span> <span class=\"tok-kw\">class</span> <span class=\"tok-type\">Node</span>",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">public</span> <span class=\"tok-type\">int</span> Value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">public</span> <span class=\"tok-type\">Node</span>? Left, Right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">public</span> <span class=\"tok-type\">Node</span>(<span class=\"tok-type\">int</span> value) { Value = value; }",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">Node</span> <span class=\"tok-fn\">Insert</span>(<span class=\"tok-type\">Node</span>? root, <span class=\"tok-type\">int</span> value)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">new</span> <span class=\"tok-type\">Node</span>(value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; root.Value) root.Left = <span class=\"tok-fn\">Insert</span>(root.Left, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">else</span> root.Right = <span class=\"tok-fn\">Insert</span>(root.Right, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> root;",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">InOrder</span>(<span class=\"tok-type\">Node</span>? root, <span class=\"tok-type\">int</span>[] a, <span class=\"tok-kw\">ref</span> <span class=\"tok-type\">int</span> k)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">InOrder</span>(root.Left, a, <span class=\"tok-kw\">ref</span> k);",
      "&nbsp;&nbsp;&nbsp;&nbsp;a[k++] = root.Value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">InOrder</span>(root.Right, a, <span class=\"tok-kw\">ref</span> k);",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">TreeSort</span>(<span class=\"tok-type\">int</span>[] a)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">Node</span>? root = <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">foreach</span> (<span class=\"tok-type\">int</span> value <span class=\"tok-kw\">in</span> a) root = <span class=\"tok-fn\">Insert</span>(root, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> k = 0;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">InOrder</span>(root, a, <span class=\"tok-kw\">ref</span> k);",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "<span class=\"tok-kw\">static</span> <span class=\"tok-kw\">class</span> <span class=\"tok-type\">Node</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">int</span> value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">Node</span> left, right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">Node</span>(<span class=\"tok-type\">int</span> value) { <span class=\"tok-kw\">this</span>.value = value; }",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">Node</span> <span class=\"tok-fn\">insert</span>(<span class=\"tok-type\">Node</span> root, <span class=\"tok-type\">int</span> value) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">new</span> <span class=\"tok-type\">Node</span>(value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; root.value) root.left = <span class=\"tok-fn\">insert</span>(root.left, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">else</span> root.right = <span class=\"tok-fn\">insert</span>(root.right, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> root;",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">int</span> <span class=\"tok-fn\">inOrder</span>(<span class=\"tok-type\">Node</span> root, <span class=\"tok-type\">int</span>[] a, <span class=\"tok-type\">int</span> k) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root == <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> k;",
      "&nbsp;&nbsp;&nbsp;&nbsp;k = <span class=\"tok-fn\">inOrder</span>(root.left, a, k);",
      "&nbsp;&nbsp;&nbsp;&nbsp;a[k++] = root.value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">inOrder</span>(root.right, a, k);",
      "}",
      "",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">void</span> <span class=\"tok-fn\">treeSort</span>(<span class=\"tok-type\">int</span>[] a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">Node</span> root = <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-type\">int</span> value : a) root = <span class=\"tok-fn\">insert</span>(root, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">inOrder</span>(root, a, 0);",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "<span class=\"tok-kw\">class</span> <span class=\"tok-type\">Node</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">__init__</span>(self, value):",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;self.value = value",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;self.left = <span class=\"tok-kw\">None</span>",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;self.right = <span class=\"tok-kw\">None</span>",
      "",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">insert</span>(root, value):",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">None</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-type\">Node</span>(value)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> value &lt; root.value:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root.left = <span class=\"tok-fn\">insert</span>(root.left, value)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">else</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root.right = <span class=\"tok-fn\">insert</span>(root.right, value)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> root",
      "",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">in_order</span>(root, a, k):",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> root <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">None</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> k",
      "&nbsp;&nbsp;&nbsp;&nbsp;k = <span class=\"tok-fn\">in_order</span>(root.left, a, k)",
      "&nbsp;&nbsp;&nbsp;&nbsp;a[k] = root.value",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">in_order</span>(root.right, a, k + 1)",
      "",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">tree_sort</span>(a):",
      "&nbsp;&nbsp;&nbsp;&nbsp;root = <span class=\"tok-kw\">None</span>",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> value <span class=\"tok-kw\">in</span> a:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;root = <span class=\"tok-fn\">insert</span>(root, value)",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">in_order</span>(root, a, 0)"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">class</span> <span class=\"tok-type\">TreeNode</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;left: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span> = <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;right: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span> = <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">constructor</span>(<span class=\"tok-kw\">public</span> value: <span class=\"tok-type\">number</span>) {}",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">insert</span>(root: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>, value: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">TreeNode</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">new</span> <span class=\"tok-type\">TreeNode</span>(value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; root.value) root.left = <span class=\"tok-fn\">insert</span>(root.left, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">else</span> root.right = <span class=\"tok-fn\">insert</span>(root.right, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> root;",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">inOrder</span>(root: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>, a: <span class=\"tok-type\">number</span>[], k: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">number</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> k;",
      "&nbsp;&nbsp;&nbsp;&nbsp;k = <span class=\"tok-fn\">inOrder</span>(root.left, a, k);",
      "&nbsp;&nbsp;&nbsp;&nbsp;a[k++] = root.value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">inOrder</span>(root.right, a, k);",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">treeSort</span>(a: <span class=\"tok-type\">number</span>[]): <span class=\"tok-type\">void</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> root: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span> = <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) root = <span class=\"tok-fn\">insert</span>(root, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">inOrder</span>(root, a, 0);",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "<span class=\"tok-kw\">class</span> <span class=\"tok-type\">TreeNode</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">constructor</span>(value) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">this</span>.value = value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">this</span>.left = <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">this</span>.right = <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">insert</span>(root, value) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> <span class=\"tok-kw\">new</span> <span class=\"tok-type\">TreeNode</span>(value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (value &lt; root.value) root.left = <span class=\"tok-fn\">insert</span>(root.left, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">else</span> root.right = <span class=\"tok-fn\">insert</span>(root.right, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> root;",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">inOrder</span>(root, a, k) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (root === <span class=\"tok-kw\">null</span>) <span class=\"tok-kw\">return</span> k;",
      "&nbsp;&nbsp;&nbsp;&nbsp;k = <span class=\"tok-fn\">inOrder</span>(root.left, a, k);",
      "&nbsp;&nbsp;&nbsp;&nbsp;a[k++] = root.value;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-fn\">inOrder</span>(root.right, a, k);",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">treeSort</span>(a) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> root = <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">for</span> (<span class=\"tok-kw\">const</span> value <span class=\"tok-kw\">of</span> a) root = <span class=\"tok-fn\">insert</span>(root, value);",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-fn\">inOrder</span>(root, a, 0);",
      "}"
    ]
  };
  public static List<SortStep> Record(
    IReadOnlyList<int> input,
    UiLanguage language
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
          Snapshot = StepArrays.Snapshot(a),
          I = i,
          CompareCount = compareCount,
          SwapCount = writeCount,
          RightIndex = i,
          SortedIndices = StepArrays.Sorted(sorted),
          ActiveCodeLines = [30, 31],
          Caption = Res.Caption("TreeSort_InsertStart", language, i, a[i])
        }
      );

      root = Insert(root, a[i], i, a, steps, sorted, ref compareCount, writeCount, language);
    }

    steps.Add(
      new SortStep
      {
        Type = StepType.EndPass,
        Snapshot = StepArrays.Snapshot(a),
        CompareCount = compareCount,
        SwapCount = writeCount,
        SortedIndices = StepArrays.Sorted(sorted),
        ActiveCodeLines = [33, 34],
        Caption = Res.Caption("TreeSort_TraverseStart", language)
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
          SortedIndices = StepArrays.Sorted(sorted),
          ActiveCodeLines = [23],
          Caption = Res.Caption("TreeSort_Place", language, visited[idx].Value, visited[idx].SourceIndex, idx)
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
        SortedIndices = StepArrays.Sorted(sorted),
        ActiveCodeLines = [27, 28],
        Caption = Res.Caption("Common_SortCompleted", language)
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
    int writeCount,
    UiLanguage language
  )
  {
    if (node is null)
    {
      steps.Add(
        new SortStep
        {
          Type = StepType.NoSwap,
          Snapshot = StepArrays.Snapshot(a),
          CompareCount = compareCount,
          SwapCount = writeCount,
          RightIndex = originalIndex,
          SortedIndices = StepArrays.Sorted(sorted),
          ActiveCodeLines = [8, 9],
          Caption = Res.Caption("TreeSort_EmptySlot", language, value)
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
        Snapshot = StepArrays.Snapshot(a),
        CompareCount = compareCount,
        SwapCount = writeCount,
        LeftIndex = node.OriginalIndex,
        RightIndex = originalIndex,
        SortedIndices = StepArrays.Sorted(sorted),
        ActiveCodeLines = goLeft ? [11, 12] : [11, 13, 14],
        Caption = Res.Caption(
          goLeft ? "TreeSort_CompareLeft" : "TreeSort_CompareRight",
          language,
          value, node.OriginalIndex, node.Value
        )
      }
    );

    if (goLeft)
    {
      node.Left = Insert(node.Left, value, originalIndex, a, steps, sorted, ref compareCount, writeCount, language);
    }
    else
    {
      node.Right = Insert(node.Right, value, originalIndex, a, steps, sorted, ref compareCount, writeCount, language);
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
