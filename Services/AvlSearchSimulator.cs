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
  public static readonly string[] CodeLines =
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
  ];

  public static TreeSearchResult Record(
    int[] values,
    int target
  )
  {
    var (nodes, rootId) = AvlBuilder.Build(values);
    return BstSearchSimulator.Search(nodes, rootId, target);
  }
}
