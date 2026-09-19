using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Plays a pre-recorded list of <see cref="TreeSearchStep"/> back on a timer —
/// the tree-search counterpart of <see cref="AnimationPlayer"/>. Kept as its
/// own class rather than generalizing <see cref="AnimationPlayer"/> over the
/// step type: the two players' dwell-time tables are keyed to different step
/// enums, and duplicating this small class is cheaper than adding generics to
/// an already-working, already-tested one.
/// </summary>
public sealed class TreeAnimationPlayer : IDisposable
{
  private CancellationTokenSource? _cts;
  private List<TreeSearchStep> _steps = [];

  public PlaybackState State { get; } = new();

  /// <summary>Zero-based pause point; -1 disables the breakpoint.</summary>
  public int BreakAtIndex { get; set; } = -1;

  /// <summary>Raised whenever the current step index (or play state) changes.</summary>
  public event Action? Changed;

  public IReadOnlyList<TreeSearchStep> Steps => _steps;

  public TreeSearchStep? Current =>
    State.CurrentIndex >= 0 && State.CurrentIndex < _steps.Count
      ? _steps[State.CurrentIndex]
      : null;

  public bool IsAtEnd => State.CurrentIndex >= _steps.Count - 1;

  public void Load(
    List<TreeSearchStep> steps
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

  /// <summary>Rewinds one step. Symmetric with <see cref="StepForward"/> — index -1 means
  /// "before the first step", so this bottoms out there instead of going negative.</summary>
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

  /// <summary>Jumps straight to an arbitrary step — the scrubber's operation. Pauses first so
  /// dragging the scrubber always wins over an in-progress autoplay instead of racing it.</summary>
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
    TreeSearchStepType type
  )
  {
    return type switch
    {
      TreeSearchStepType.Visit => 600,
      TreeSearchStepType.GoLeft => 650,
      TreeSearchStepType.GoRight => 650,
      TreeSearchStepType.Skip => 500,
      TreeSearchStepType.Found => 1100,
      TreeSearchStepType.NotFound => 1100,
      _ => 500
    };
  }

  public void Dispose()
  {
    _cts?.Cancel();
    _cts?.Dispose();
  }
}
