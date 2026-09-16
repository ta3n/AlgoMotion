namespace AlgoMotion.Services;

/// <summary>
/// Builds the insertion order fed to <c>BstBuilder</c>/<c>AvlBuilder</c> so the
/// resulting plain BST reaches at least a requested depth, instead of leaving
/// depth entirely up to chance the way a plain random shuffle does.
///
/// A random shuffle of n distinct values produces a BST with expected depth
/// O(log n) — fine for showing off average-case behavior, but too shallow to
/// clearly demonstrate a search walking down many levels, or to show why
/// depth matters at all. This deliberately front-loads a strictly ascending
/// "spine" — the largest <c>depth + 1</c> values, inserted in ascending
/// order — which a naive BST insert always turns into a straight
/// right-leaning chain of exactly that depth, then appends every remaining
/// (smaller) value in random order. Since every remaining value is smaller
/// than the spine's root, they all branch off to the root's left, filling
/// out natural-looking side branches without disturbing the spine.
///
/// <see cref="AvlBuilder"/> rebalances on every insert regardless of this
/// order, so feeding it the same adversarial-looking sequence still yields a
/// balanced O(log n) tree — the contrast is the point: cranking the depth
/// slider up makes BST/DFS/BFS grow a visibly deep spine while AVL stays
/// compact no matter what.
/// </summary>
public static class TreeInputGenerator
{
  public static int[] GenerateInsertionOrder(
    int n,
    int minDepth,
    Random rng
  )
  {
    if (n <= 0)
    {
      return [];
    }

    var spineLength = Math.Clamp(minDepth, 0, n - 1) + 1;
    var spineStart = (n - spineLength) + 1;
    var spine = Enumerable.Range(spineStart, spineLength);
    var remainder = Enumerable.Range(1, spineStart - 1).OrderBy(_ => rng.Next());

    return [.. spine, .. remainder];
  }
}
