using AlgoMotion.Models;

namespace AlgoMotion.Services;

public sealed record ComplexitySample(
  int N,
  int Comparisons,
  int Writes
)
{
  public int Total => Comparisons + Writes;
}

public static class ComplexityExperiment
{
  public static int[] Input(
    int n,
    string pattern
  )
  {
    var values = Enumerable.Range(1, n).ToArray();
    if (pattern == "Descending")
    {
      Array.Reverse(values);
    }

    if (pattern == "Random")
    {
#pragma warning disable S2245 // Reproducible educational data, never used for security.
      var random = new Random(1778 + n);
#pragma warning restore S2245
      random.Shuffle(values);
    }

    return values;
  }

  public static ComplexitySample Measure(
    SortAlgorithmKind kind,
    int n,
    string pattern
  )
  {
    var step = SortAlgorithms.Get(kind).Record(Input(n, pattern), UiLanguage.En)[^1];
    return new(n, step.CompareCount, step.SwapCount);
  }

  public static double Growth(
    string curve,
    int n
  )
  {
    return curve switch
    {
      "n" => n,
      "n log n" => n * Math.Log2(n),
      _ => (double)n * n
    };
  }
}
