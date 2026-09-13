namespace AlgoMotion.Models;

/// <summary>
/// The kind of event a single recorded step represents. The UI uses this
/// to decide timing, captions and which code lines to light up.
/// </summary>
public enum StepType
{
    StartPass,
    Compare,
    Swap,
    NoSwap,
    MarkSorted,
    EndPass,
    Completed
}

/// <summary>
/// One immutable frame of the Bubble Sort animation.
///
/// The simulator (<see cref="Services.BubbleSortSimulator"/>) runs the whole
/// algorithm up front and records every frame as a <see cref="SortStep"/>.
/// The UI never re-implements the algorithm: it simply plays this list back,
/// one step at a time, and lets CSS animate the difference between frames.
/// </summary>
public sealed class SortStep
{
    public StepType Type { get; set; }

    /// <summary>The full array state *after* this step is applied.</summary>
    public int[] Snapshot { get; set; } = [];

    /// <summary>Outer loop index (pass number), -1 when not applicable.</summary>
    public int I { get; set; } = -1;

    /// <summary>Inner loop index (comparison position), -1 when not applicable.</summary>
    public int J { get; set; } = -1;

    public int CompareCount { get; set; }

    public int SwapCount { get; set; }

    public bool Swapped { get; set; }

    /// <summary>Index of the left element of the pair being compared/swapped.</summary>
    public int? LeftIndex { get; set; }

    /// <summary>Index of the right element of the pair being compared/swapped.</summary>
    public int? RightIndex { get; set; }

    /// <summary>1-based source line numbers to highlight in the code panel.</summary>
    public int[] ActiveCodeLines { get; set; } = [];

    /// <summary>Indices that have already settled into their final, sorted position.</summary>
    public int[] SortedIndices { get; set; } = [];

    /// <summary>Human readable caption shown under the code panel.</summary>
    public string Caption { get; set; } = "";
}
