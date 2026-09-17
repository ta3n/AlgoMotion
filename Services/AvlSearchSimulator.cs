using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Records AVL Search: identical walk-down logic to <see cref="BstSearchSimulator"/>
/// (comparing against the current node, going left or right) — the only
/// difference is which builder produced the tree. Plain BST insertion can
/// degenerate into a linked list for adversarial input (e.g. already-sorted
/// values), making search O(n) worst case; <see cref="AvlBuilder"/> keeps
/// every subtree balanced as it inserts, so this same code stays O(log n)
/// no matter what order the values arrive in.
///
/// <code>
///  1  node *avl_search(node *root, int target)
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
public static class AvlSearchSimulator
{
  public static readonly IReadOnlyDictionary<CodeLanguage, string[]> CodeByLanguage = new Dictionary<CodeLanguage, string[]>
  {
    [CodeLanguage.C] =
    [
      "<span class=\"tok-type\">node</span> *<span class=\"tok-fn\">avl_search</span>(<span class=\"tok-type\">node</span> *root, <span class=\"tok-type\">int</span> target)",
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
    ],
    [CodeLanguage.CSharp] =
    [
      "// Node contains an integer Value and nullable Left/Right children.",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">Node</span>? <span class=\"tok-fn\">AvlSearch</span>(<span class=\"tok-type\">Node</span>? root, <span class=\"tok-type\">int</span> target)",
      "{",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">Node</span>? current = root;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (current <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">not</span> <span class=\"tok-kw\">null</span>) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.Value == target) <span class=\"tok-kw\">return</span> current;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;current = target &lt; current.Value ? current.Left : current.Right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">null</span>;",
      "}"
    ],
    [CodeLanguage.Java] =
    [
      "// Node contains an int value and nullable left/right children.",
      "<span class=\"tok-kw\">static</span> <span class=\"tok-type\">Node</span> <span class=\"tok-fn\">avlSearch</span>(<span class=\"tok-type\">Node</span> root, <span class=\"tok-type\">int</span> target) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-type\">Node</span> current = root;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (current != <span class=\"tok-kw\">null</span>) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.value == target) <span class=\"tok-kw\">return</span> current;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;current = target &lt; current.value ? current.left : current.right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">null</span>;",
      "}"
    ],
    [CodeLanguage.Python] =
    [
      "# Nodes have value, left, and right attributes; missing children are None.",
      "<span class=\"tok-kw\">def</span> <span class=\"tok-fn\">avl_search</span>(root, target):",
      "&nbsp;&nbsp;&nbsp;&nbsp;current = root",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> current <span class=\"tok-kw\">is</span> <span class=\"tok-kw\">not</span> <span class=\"tok-kw\">None</span>:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> current.value == target:",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> current",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;current = current.left <span class=\"tok-kw\">if</span> target &lt; current.value <span class=\"tok-kw\">else</span> current.right",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">None</span>"
    ],
    [CodeLanguage.TypeScript] =
    [
      "<span class=\"tok-kw\">interface</span> <span class=\"tok-type\">TreeNode</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;value: <span class=\"tok-type\">number</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;left: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>;",
      "&nbsp;&nbsp;&nbsp;&nbsp;right: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>;",
      "}",
      "",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">avlSearch</span>(root: <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span>, target: <span class=\"tok-type\">number</span>): <span class=\"tok-type\">TreeNode</span> | <span class=\"tok-kw\">null</span> {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> current = root;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (current !== <span class=\"tok-kw\">null</span>) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.value === target) <span class=\"tok-kw\">return</span> current;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;current = target &lt; current.value ? current.left : current.right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">null</span>;",
      "}"
    ],
    [CodeLanguage.JavaScript] =
    [
      "// Nodes have value, left, and right properties; missing children are null.",
      "<span class=\"tok-kw\">function</span> <span class=\"tok-fn\">avlSearch</span>(root, target) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">let</span> current = root;",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">while</span> (current !== <span class=\"tok-kw\">null</span>) {",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">if</span> (current.value === target) <span class=\"tok-kw\">return</span> current;",
      "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;current = target &lt; current.value ? current.left : current.right;",
      "&nbsp;&nbsp;&nbsp;&nbsp;}",
      "&nbsp;&nbsp;&nbsp;&nbsp;<span class=\"tok-kw\">return</span> <span class=\"tok-kw\">null</span>;",
      "}"
    ]
  };

  public static TreeSearchResult Record(
    int[] values,
    int target,
    UiLanguage language
  )
  {
    var (nodes, rootId) = AvlBuilder.Build(values);
    return BstSearchSimulator.Search(nodes, rootId, target, language);
  }
}
