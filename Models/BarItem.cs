namespace AlgoMotion.Models;

/// <summary>
/// Pure view-model for a single bar. Built fresh from the current
/// <see cref="SortStep"/> on every render; holds no animation state itself —
/// the browser animates the difference between two renders via CSS
/// transitions on <see cref="SlotIndex"/> (left offset) and the state flags.
/// </summary>
public sealed class BarItem
{
    /// <summary>Stable identity used as the Blazor @key so the DOM node is
    /// moved (and CSS-transitioned) instead of recreated when bars swap.</summary>
    public int Value { get; init; }

    /// <summary>Current position in the array (0-based, left to right).</summary>
    public int SlotIndex { get; init; }

    public bool IsActive { get; init; }

    public bool IsSwapping { get; init; }

    public bool IsSorted { get; init; }
}
