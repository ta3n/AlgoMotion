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
/// <see cref="DiagramWidthPx"/> always returns a concrete pixel width (never
/// "100%") — <see cref="TreeDiagram"/> needs a real number to compute the
/// zoomed footprint its scroll container should reserve (see
/// <see cref="ScaledWidthPx"/>/<see cref="ScaledHeightPx"/>; a percentage-
/// sized element combined with a CSS <c>transform: scale()</c> would leave
/// the scrollable area sized to the *unscaled* box, wasting scroll space
/// instead of shrinking it — that's also why the diagram is scaled through
/// an explicitly-sized wrapper rather than the element the percentages
/// resolve against). Up to <see cref="CompactThreshold"/> nodes it's a fixed
/// baseline width with node size tapering down to stay legible (same idea as
/// <see cref="ChartLayout.BarFillRatio"/>); past that, shrinking further
/// would make 1-3 digit values illegible, so node size instead freezes at a
/// fixed, comfortable size and the required width grows instead, all the way
/// up to 200 nodes — the zoom control is what keeps a wide or deep tree
/// like that viewable as a whole despite never shrinking below readable.
/// </summary>
public static class TreeLayout
{
  public const int RowHeight = 74;
  public const int TopPad = 26;
  public const int BottomPad = 20;

  /// <summary>Above this many nodes, the diagram stops shrinking nodes to fit and starts growing
  /// wider than <see cref="BaseDiagramWidthPx"/> instead (see <see cref="DiagramWidthPx"/>).</summary>
  public const int CompactThreshold = 31;

  /// <summary>Baseline diagram width at/below <see cref="CompactThreshold"/> nodes — roughly the
  /// visible width of the tree panel on a typical desktop viewport (comfortably narrower than it so
  /// small trees never need to scroll there; the zoom control covers narrower viewports).</summary>
  private const int BaseDiagramWidthPx = 900;

  /// <summary>Center-to-center spacing once nodes stop shrinking — comfortably wider than
  /// <see cref="NodeDiameter"/>'s frozen size so nodes never crowd each other no matter how many
  /// there are, since a wider diagram (zoom out to compensate) is always an option past this point.</summary>
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

  /// <summary>How wide the diagram itself needs to be — <see cref="BaseDiagramWidthPx"/> up to
  /// <see cref="CompactThreshold"/> nodes (tapering node size already keeps everything
  /// non-overlapping within that), <c>count * LargeSlotWidthPx</c> beyond it.</summary>
  public static int DiagramWidthPx(
    int count
  )
  {
    return count <= CompactThreshold ? BaseDiagramWidthPx : count * LargeSlotWidthPx;
  }

  /// <summary>Floor for the zoom slider. Deliberately not low enough to ever fully fit a 200-node,
  /// 100-deep tree — nodes are already frozen at their smallest *readable* size (see
  /// <see cref="NodeDiameter"/>), and scaling those down further would make an "overview" that
  /// shows nothing legible, which defeats the point. Past a certain size, "see the whole tree" and
  /// "read every value" are simply in tension — like any zoomable canvas, the honest answer is
  /// "zoom out for shape, zoom in to read", not a magic zoom level that satisfies both.</summary>
  public const double MinZoom = 0.4;

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
  /// one starts at (or near) 100% since it already fits.</summary>
  public static double SuggestedZoom(
    int diagramWidthPx,
    int areaHeightPx
  )
  {
    var byWidth = diagramWidthPx <= 0 ? 1.0 : AssumedViewportWidth / diagramWidthPx;
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
