using AlgoMotion.Models;

namespace AlgoMotion.Services;

public static class DpSimulator
{
  private static int[][] Table(
    int rows,
    int columns
  )
  {
    return Enumerable.Range(0, rows).Select(_ => new int[columns]).ToArray();
  }

  private static int[][] Copy(
    int[][] table
  )
  {
    return table.Select(row => row.ToArray()).ToArray();
  }

  public static DpRecording Fibonacci(
    int n
  )
  {
    if (n is < 0 or > 30)
    {
      throw new ArgumentOutOfRangeException(nameof(n));
    }

    var table = Table(1, n + 1);
    var steps = new List<DpStep> { new(-1, -1, Copy(table), []) };
    for (var i = 0; i <= n; i++)
    {
      table[0][i] = i < 2 ? i : table[0][i - 1] + table[0][i - 2];
      steps.Add(new(0, i, Copy(table), i < 2 ? [] : [(0, i - 1), (0, i - 2)]));
    }

    return new(["F(n)"], Enumerable.Range(0, n + 1).Select(i => i.ToString()).ToArray(), steps, table[0][n]);
  }

  public static DpRecording Knapsack(
    int[] weights,
    int[] values,
    int capacity
  )
  {
    if (capacity is < 1 or > 20)
    {
      throw new ArgumentOutOfRangeException(nameof(capacity));
    }

    if (weights.Length is < 1 or > 8
      || weights.Length != values.Length
      || weights.Any(w => w is < 1 or > 20)
      || values.Any(v => v is < 1 or > 10000))
    {
      throw new ArgumentException("Invalid knapsack items.", nameof(weights));
    }

    var table = Table(weights.Length + 1, capacity + 1);
    var steps = new List<DpStep> { new(-1, -1, Copy(table), []) };
    for (var i = 1; i <= weights.Length; i++)
    {
      for (var c = 0; c <= capacity; c++)
      {
        table[i][c] = table[i - 1][c];
        var dependencies = new List<(int, int)> { (i - 1, c) };
        if (weights[i - 1] <= c)
        {
          table[i][c] = Math.Max(table[i][c], values[i - 1] + table[i - 1][c - weights[i - 1]]);
          dependencies.Add((i - 1, c - weights[i - 1]));
        }

        steps.Add(new(i, c, Copy(table), [.. dependencies]));
      }
    }

    return new(
      ["0", .. weights.Select((w, i) => $"{i + 1}: w={w}, v={values[i]}")],
      Enumerable.Range(0, capacity + 1).Select(i => i.ToString()).ToArray(),
      steps,
      table[^1][capacity]
    );
  }

  public static DpRecording Lcs(
    string first,
    string second
  )
  {
    if (first.Length > 12 || second.Length > 12)
    {
      throw new ArgumentException("Strings must have at most 12 characters.");
    }

    var table = Table(first.Length + 1, second.Length + 1);
    var steps = new List<DpStep> { new(-1, -1, Copy(table), []) };
    for (var i = 1; i <= first.Length; i++)
    {
      for (var j = 1; j <= second.Length; j++)
      {
        var equal = first[i - 1] == second[j - 1];
        table[i][j] = equal ? table[i - 1][j - 1] + 1 : Math.Max(table[i - 1][j], table[i][j - 1]);
        steps.Add(new(i, j, Copy(table), equal ? [(i - 1, j - 1)] : [(i - 1, j), (i, j - 1)]));
      }
    }

    return new(
      ["∅", .. first.Select(c => c.ToString())],
      ["∅", .. second.Select(c => c.ToString())],
      steps,
      table[^1][^1]
    );
  }
}
