using AlgoMotion.Models;

namespace AlgoMotion.Services;

/// <summary>
/// Plays a pre-recorded list of <see cref="SortStep"/> back on a timer.
///
/// This class owns *only* timing and the current index — never the sorting
/// logic. Each step type gets a base dwell time (how long it stays on screen
/// before advancing), scaled by <see cref="PlaybackState.Speed"/>. The actual
/// motion (bars sliding, glows fading, code line highlighting) is left
/// entirely to CSS transitions reacting to the state change; this class just
/// tells the UI "we are now on step N" and lets the browser animate there.
/// </summary>
public sealed class AnimationPlayer : IDisposable
{
    private CancellationTokenSource? _cts;
    private List<SortStep> _steps = [];

    public PlaybackState State { get; } = new();

    /// <summary>Raised whenever the current step index (or play state) changes.</summary>
    public event Action? Changed;

    public IReadOnlyList<SortStep> Steps => _steps;

    public SortStep? Current =>
        State.CurrentIndex >= 0 && State.CurrentIndex < _steps.Count
            ? _steps[State.CurrentIndex]
            : null;

    public bool IsAtEnd => State.CurrentIndex >= _steps.Count - 1;

    public void Load(List<SortStep> steps)
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

    public void SetSpeed(double speed)
    {
        State.Speed = Math.Clamp(speed, PlaybackSpeeds.Min, PlaybackSpeeds.Max);
        Changed?.Invoke();
    }

    public void StepForward()
    {
        if (_steps.Count == 0) return;
        if (State.CurrentIndex < _steps.Count - 1)
        {
            State.CurrentIndex++;
            Changed?.Invoke();
        }
    }

    public void Play()
    {
        if (State.IsPlaying || _steps.Count == 0) return;
        if (IsAtEnd) State.CurrentIndex = -1;

        State.IsPlaying = true;
        _cts = new CancellationTokenSource();
        _ = RunAsync(_cts.Token);
        Changed?.Invoke();
    }

    public void Pause()
    {
        if (!State.IsPlaying) return;
        State.IsPlaying = false;
        _cts?.Cancel();
        _cts = null;
        Changed?.Invoke();
    }

    private async Task RunAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested && State.CurrentIndex < _steps.Count - 1)
            {
                State.CurrentIndex++;
                Changed?.Invoke();

                var dwellMs = BaseDwellMs(_steps[State.CurrentIndex].Type) / State.Speed;
                await Task.Delay(TimeSpan.FromMilliseconds(dwellMs), token);
            }
        }
        catch (TaskCanceledException)
        {
            // Pause() cancelled us — expected, nothing to do.
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

    private static double BaseDwellMs(StepType type) => type switch
    {
        StepType.StartPass => 550,
        StepType.Compare => 550,
        StepType.Swap => 700,
        StepType.NoSwap => 380,
        StepType.MarkSorted => 480,
        StepType.EndPass => 350,
        StepType.Completed => 900,
        StepType.NewCandidate => 450,
        StepType.SetPivot => 600,
        StepType.RangeDone => 500,
        StepType.SplitRange => 500,
        StepType.MergeCompare => 500,
        StepType.MergeWrite => 450,
        _ => 400
    };

    public void Dispose()
    {
        _cts?.Cancel();
        _cts?.Dispose();
    }
}
