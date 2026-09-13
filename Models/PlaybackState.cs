using System.Globalization;

namespace AlgoMotion.Models;

/// <summary>Helpers for the continuous playback-speed range (0.5x – 20x, in 0.5x steps).</summary>
public static class PlaybackSpeeds
{
  public const double Min = 0.5;
  public const double Max = 20.0;
  public const double Step = 0.5;

  public static string Format(
    double speed
  )
  {
    return speed.ToString("0.#", CultureInfo.InvariantCulture) + "x";
  }
}

/// <summary>
/// Holds the state of the *player*, not the algorithm: which step we're on,
/// whether we're playing, and at what speed. Kept separate from
/// <see cref="SortStep"/> so the timeline can be scrubbed, replayed or
/// reset without touching the recorded algorithm data.
/// </summary>
public sealed class PlaybackState
{
  /// <summary>-1 means "before the first step" (initial, unsorted array).</summary>
  public int CurrentIndex { get; set; } = -1;

  public bool IsPlaying { get; set; }

  /// <summary>Playback speed multiplier, from <see cref="PlaybackSpeeds.Min"/> to <see cref="PlaybackSpeeds.Max"/>.</summary>
  public double Speed { get; set; } = 1.0;
}
