using System.Globalization;

namespace AlgoMotion.Models;

/// <summary>
/// Single source of truth for the geometry of the tree diagram — the tree
/// equivalent of <see cref="ChartLayout"/> for the bar chart.
///
/// Horizontal placement uses each node's <see cref="TreeNode.InorderIndex"/>
/// as a percentage of the diagram's width (same "equal slots" idea
/// <see cref="ChartLayout"/> uses for bars), which keeps parents naturally
/// centered over their subtrees without any per-node overlap math: a BST's
/// in-order traversal is exactly its left-to-right visual order. Vertical
/// placement is a fixed row height per <see cref="TreeNode.Depth"/>.
///
/// Up to <see cref="CompactThreshold"/> nodes, the diagram fills 100% of its
/// container and node size tapers down to stay legible (same idea as
/// <see cref="ChartLayout.BarFillRatio"/>). Past that, shrinking further
/// would make 1-3 digit values illegible, so node size instead freezes at a
/// fixed, comfortable size and <see cref="RequiredWidthPx"/> grows the
/// diagram wider than its container — the containing element scrolls
/// horizontally (see <c>.tree-chart-area</c> in app.css) rather than the
/// nodes ever shrinking below a readable size, all the way up to 200 nodes.
/// </summary>
public static class TreeLayout
{
  public const int RowHeight = 74;
  public const int TopPad = 26;
  public const int BottomPad = 20;

  /// <summary>Above this many nodes, the diagram stops shrinking nodes to fit and starts growing
  /// wider than its container instead (see <see cref="RequiredWidthPx"/>).</summary>
  public const int CompactThreshold = 31;

  /// <summary>Center-to-center spacing once nodes stop shrinking — comfortably wider than
  /// <see cref="NodeDiameter"/>'s frozen size so nodes never crowd each other no matter how many
  /// there are, since going wider (with a scrollbar) is always an option past this point.</summary>
  private const int LargeSlotWidthPx = 34;

  public static double SlotPercent(
    int count
  )
  {
    return count <= 0 ? 100.0 : 100.0 / count;
  }

  public static double CenterPercent(
    int inorderIndex,
    int count
  )
  {
    return (inorderIndex * SlotPercent(count)) + (SlotPercent(count) / 2);
  }

  /// <summary>Node circle diameter in px — shrinks as the tree gets wider so nodes never overlap
  /// their neighbors' equal-width slot, up to <see cref="CompactThreshold"/>; beyond that it freezes
  /// (see <see cref="RequiredWidthPx"/>).</summary>
  public static int NodeDiameter(
    int count
  )
  {
    return count switch
    {
      <= 10 => 46,
      <= 16 => 40,
      <= 24 => 34,
      <= 31 => 28,
      _ => 22
    };
  }

  public static double NodeFontSizePx(
    int count
  )
  {
    return count switch
    {
      <= 10 => 16,
      <= 16 => 14,
      <= 24 => 12,
      _ => 10
    };
  }

  /// <summary>How wide the diagram itself needs to be once nodes have frozen at their smallest
  /// comfortable size — 0 below <see cref="CompactThreshold"/>, meaning "just use 100%, the
  /// tapering above already keeps everything non-overlapping within the container".</summary>
  public static int RequiredWidthPx(
    int count
  )
  {
    return count <= CompactThreshold ? 0 : count * LargeSlotWidthPx;
  }

  public const double MinZoom = 0.15;
  public const double MaxZoom = 1.5;
  public const double ZoomStep = 0.05;
  public const double DefaultZoom = 1.0;

  /// <summary>Rough size of the visible tree panel, used only to pick a starting zoom — not a real
  /// measurement (this app makes no DOM measurement/JS interop calls anywhere), just enough to make
  /// a freshly generated deep or wide tree start already shrunk to roughly fit instead of opening
  /// zoomed all the way in on one corner of it.</summary>
  private const double AssumedViewportWidth = 900;

  private const double AssumedViewportHeight = 560;

  /// <summary>Picks a starting zoom so a new tree opens roughly fitted to the panel instead of
  /// always at 100% — a wide (many nodes) or deep (large min-depth) tree starts zoomed out; a small
  /// one starts at 100% since it already fits.</summary>
  public static double SuggestedZoom(
    int requiredWidthPx,
    int areaHeightPx
  )
  {
    var byWidth = requiredWidthPx <= 0 ? 1.0 : AssumedViewportWidth / requiredWidthPx;
    var byHeight = areaHeightPx <= 0 ? 1.0 : AssumedViewportHeight / areaHeightPx;
    return Math.Clamp(Math.Min(1.0, Math.Min(byWidth, byHeight)), MinZoom, MaxZoom);
  }

  public static int TopFor(
    int depth
  )
  {
    return TopPad + (depth * RowHeight);
  }

  public static int AreaHeight(
    int maxDepth
  )
  {
    return TopPad + ((maxDepth + 1) * RowHeight) + BottomPad;
  }

  /// <summary>Formats a percentage value as a culture-invariant CSS length, e.g. "12.5%".</summary>
  public static string Pct(
    double value
  )
  {
    return value.ToString("0.####", CultureInfo.InvariantCulture) + "%";
  }

  /// <summary>Formats a plain number as a culture-invariant string, e.g. for SVG coordinates.</summary>
  public static string Num(
    double value
  )
  {
    return value.ToString("0.###", CultureInfo.InvariantCulture);
  }
}
