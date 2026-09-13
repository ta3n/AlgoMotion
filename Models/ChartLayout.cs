using System.Globalization;

namespace AlgoMotion.Models;

/// <summary>
/// Single source of truth for the geometry of the bars area, shared by
/// <c>SortBars</c> and <c>RobotArm</c> so the crane always lines up with the
/// bar it's pointing at.
///
/// Horizontal placement is expressed as a <b>percentage</b> of the chart's
/// width rather than a fixed pixel slot. That way the track always fills the
/// available width — with few elements the bars are wide, with many elements
/// they shrink to fit — and nothing ever needs to scroll horizontally.
///
/// Everything below is a continuous function of <c>count</c> (the array
/// size), not a hardcoded look for 8 items: font size, crane size and even
/// how much of each slot the bar body fills all shrink smoothly as the array
/// grows, so the layout stays readable from a handful of items up to 200.
/// Past a point a bar is just too narrow for a number or a claw to land on
/// legibly — <see cref="ShowValueText"/> and <see cref="ShowCrane"/> mark
/// where the UI drops those details rather than rendering them unreadably.
/// </summary>
public static class ChartLayout
{
    public const int BarAreaHeight = 200;
    public const int BarMinHeight = 24;

    /// <summary>Above this many items the UI switches to its compact visual treatment (smaller glows, tighter rounding).</summary>
    public const int CompactThreshold = 20;

    /// <summary>Above this many items, a bar is too narrow for its number or a crane claw to land on
    /// legibly: value labels and the crane are dropped, and glows/rounding shrink further.</summary>
    public const int UltraCompactThreshold = 60;

    public static double SlotPercent(int count) => count <= 0 ? 100.0 : 100.0 / count;

    public static double LeftPercent(int index, int count) => index * SlotPercent(count);

    /// <summary>Fraction of each equal-width slot filled by the bar body. Grows as items shrink, so
    /// there's still enough pixel width left for the value label instead of wasting it on gaps.</summary>
    public static double BarFillRatio(int count) => count switch
    {
        <= 20 => 0.70,
        <= 35 => 0.80,
        <= 60 => 0.88,
        <= 120 => 0.92,
        _ => 0.95
    };

    public static double BarWidthPercent(int count) => SlotPercent(count) * BarFillRatio(count);

    public static double CenterPercent(int index, int count) => LeftPercent(index, count) + BarWidthPercent(count) / 2;

    public static int BarHeight(int value, int maxValue) =>
        maxValue <= 0
            ? BarMinHeight
            : BarMinHeight + (int)Math.Round((BarAreaHeight - BarMinHeight) * (value / (double)maxValue));

    /// <summary>Value-label font size in px — shrinks as bars get narrower so text never overflows a bar.
    /// Irrelevant once <see cref="ShowValueText"/> turns the label off entirely.</summary>
    public static double BarFontSizePx(int count) => count switch
    {
        <= 10 => 16,
        <= 16 => 14,
        <= 22 => 12,
        <= 30 => 10,
        <= 40 => 9,
        _ => 8
    };

    /// <summary>Uniform scale factor for the crane (head + claws) so it never grows wider than the
    /// slot it's pointing at once the array gets crowded. Irrelevant once <see cref="ShowCrane"/> hides it.</summary>
    public static double CraneScale(int count) => Math.Clamp(16.0 / Math.Max(count, 1), 0.32, 1.0);

    public static bool IsCompact(int count) => count > CompactThreshold;

    public static bool IsUltraCompact(int count) => count > UltraCompactThreshold;

    /// <summary>Below the ultra-compact threshold there's room to print the number inside each bar.</summary>
    public static bool ShowValueText(int count) => count <= UltraCompactThreshold;

    /// <summary>A claw only reads as "pointing at this bar" while a bar is still wider than the claw
    /// graphic itself; past the threshold the crane is dropped in favor of the bars' own highlight glow.</summary>
    public static bool ShowCrane(int count) => count <= UltraCompactThreshold;

    /// <summary>Formats a percentage value as a culture-invariant CSS length, e.g. "12.5%".</summary>
    public static string Pct(double value) => value.ToString("0.####", CultureInfo.InvariantCulture) + "%";

    /// <summary>Formats a plain number as a culture-invariant CSS value, e.g. for px/scale.</summary>
    public static string Num(double value) => value.ToString("0.###", CultureInfo.InvariantCulture);
}
