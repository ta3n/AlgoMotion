using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Plays a pre-recorded list of <see cref="SearchStep"/> back on a timer — the array-search counterpart of
/// <see cref="TreeAnimationPlayer"/>. Kept as its own class for the same reason that one is: the dwell-time
/// table is keyed to this step enum, and duplicating a small, already-tested class is cheaper than generics.
/// </summary>
public sealed class SearchAnimationPlayer : IDisposable
{
  private CancellationTokenSource? _cts;
  private List<SearchStep> _steps = [];

  public PlaybackState State { get; } = new();

  /// <summary>Zero-based pause point; -1 disables the breakpoint.</summary>
  public int BreakAtIndex { get; set; } = -1;

  /// <summary>Raised whenever the current step index (or play state) changes.</summary>
  public event Action? Changed;

  public IReadOnlyList<SearchStep> Steps => _steps;

  public SearchStep? Current =>
    State.CurrentIndex >= 0 && State.CurrentIndex < _steps.Count
      ? _steps[State.CurrentIndex]
      : null;

  public bool IsAtEnd => State.CurrentIndex >= _steps.Count - 1;

  public void Load(
    List<SearchStep> steps
  )
  {
    Pause();
    _steps = steps;
    State.CurrentIndex = -1;
    Changed?.Invoke();
  }

  public void Reset()
  {
    Pause();
    State.CurrentIndex = -1;
    Changed?.Invoke();
  }

  public void Replay()
  {
    Pause();
    State.CurrentIndex = -1;
    Play();
  }

  public void SetSpeed(
    double speed
  )
  {
    State.Speed = Math.Clamp(speed, PlaybackSpeeds.Min, PlaybackSpeeds.Max);
    Changed?.Invoke();
  }

  public void StepForward()
  {
    if (_steps.Count == 0)
    {
      return;
    }

    if (State.CurrentIndex < _steps.Count - 1)
    {
      State.CurrentIndex++;
      Changed?.Invoke();
    }
  }

  /// <summary>Rewinds one step; index -1 means "before the first step", so this bottoms out there.</summary>
  public void StepBack()
  {
    if (_steps.Count == 0)
    {
      return;
    }

    if (State.CurrentIndex > -1)
    {
      State.CurrentIndex--;
      Changed?.Invoke();
    }
  }

  /// <summary>Jumps straight to an arbitrary step — the scrubber's operation. Pauses first so dragging
  /// the scrubber always wins over an in-progress autoplay instead of racing it.</summary>
  public void SeekTo(
    int index
  )
  {
    Pause();
    State.CurrentIndex = Math.Clamp(index, -1, _steps.Count - 1);
    Changed?.Invoke();
  }

  public void Play()
  {
    if (State.IsPlaying || _steps.Count == 0)
    {
      return;
    }

    if (IsAtEnd)
    {
      State.CurrentIndex = -1;
    }

    State.IsPlaying = true;
    _cts = new CancellationTokenSource();
    _ = RunAsync(_cts.Token);
    Changed?.Invoke();
  }

  public void Pause()
  {
    if (!State.IsPlaying)
    {
      return;
    }

    State.IsPlaying = false;
    _cts?.Cancel();
    _cts = null;
    Changed?.Invoke();
  }

  private async Task RunAsync(
    CancellationToken token
  )
  {
    try
    {
      while (!token.IsCancellationRequested && State.CurrentIndex < _steps.Count - 1)
      {
        State.CurrentIndex++;
        Changed?.Invoke();

        if (State.CurrentIndex == BreakAtIndex)
        {
          Pause();
          break;
        }

        var dwellMs = BaseDwellMs(_steps[State.CurrentIndex].Type) / State.Speed;
        await Task.Delay(TimeSpan.FromMilliseconds(dwellMs), token);
      }
    }
    catch (TaskCanceledException)
    {
      // Pause() canceled us — expected, nothing to do.
    }
    finally
    {
      if (!token.IsCancellationRequested)
      {
        State.IsPlaying = false;
        Changed?.Invoke();
      }
    }
  }

  private static double BaseDwellMs(
    SearchStepType type
  )
  {
    return type switch
    {
      SearchStepType.Check => 600,
      SearchStepType.NarrowLeft => 650,
      SearchStepType.NarrowRight => 650,
      SearchStepType.JumpBlock => 650,
      SearchStepType.Found => 1100,
      SearchStepType.NotFound => 1100,
      _ => 500
    };
  }

  public void Dispose()
  {
    _cts?.Cancel();
    _cts?.Dispose();
  }
}
