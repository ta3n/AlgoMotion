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

  public static DpRecording EditDistance(
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
    // Base row/column: turning an i-length prefix into the empty string (or vice versa)
    // always costs i deletions/insertions, so they're seeded and recorded before the fill.
    for (var i = 1; i <= first.Length; i++)
    {
      table[i][0] = i;
      steps.Add(new(i, 0, Copy(table), [(i - 1, 0)]));
    }

    for (var j = 1; j <= second.Length; j++)
    {
      table[0][j] = j;
      steps.Add(new(0, j, Copy(table), [(0, j - 1)]));
    }

    for (var i = 1; i <= first.Length; i++)
    {
      for (var j = 1; j <= second.Length; j++)
      {
        var equal = first[i - 1] == second[j - 1];
        table[i][j] = equal
          ? table[i - 1][j - 1]
          : 1 + Math.Min(table[i - 1][j - 1], Math.Min(table[i - 1][j], table[i][j - 1]));
        steps.Add(new(i, j, Copy(table), equal ? [(i - 1, j - 1)] : [(i - 1, j - 1), (i - 1, j), (i, j - 1)]));
      }
    }

    return new(
      ["∅", .. first.Select(c => c.ToString())],
      ["∅", .. second.Select(c => c.ToString())],
      steps,
      table[^1][^1]
    );
  }

  public static DpRecording CoinChange(
    int[] coins,
    int amount
  )
  {
    if (amount is < 1 or > 30)
    {
      throw new ArgumentOutOfRangeException(nameof(amount));
    }

    if (coins.Length is < 1 or > 8 || coins.Any(c => c is < 1 or > 20))
    {
      throw new ArgumentException("Invalid coin denominations.", nameof(coins));
    }

    // amount + 1 coins can never be needed for a reachable amount (every coin is worth at
    // least 1), so it doubles as an "unreachable" sentinel without a separate flag per cell.
    var unreachable = amount + 1;
    var table = Table(coins.Length + 1, amount + 1);
    var steps = new List<DpStep> { new(-1, -1, Copy(table), []) };
    for (var a = 1; a <= amount; a++)
    {
      table[0][a] = unreachable;
      steps.Add(new(0, a, Copy(table), []));
    }

    for (var i = 1; i <= coins.Length; i++)
    {
      for (var a = 0; a <= amount; a++)
      {
        table[i][a] = table[i - 1][a];
        var dependencies = new List<(int, int)> { (i - 1, a) };
        if (coins[i - 1] <= a)
        {
          table[i][a] = Math.Min(table[i][a], table[i][a - coins[i - 1]] + 1);
          dependencies.Add((i, a - coins[i - 1]));
        }

        steps.Add(new(i, a, Copy(table), [.. dependencies]));
      }
    }

    var best = table[^1][amount];
    return new(
      ["0", .. coins.Select((c, i) => $"{i + 1}: coin={c}")],
      Enumerable.Range(0, amount + 1).Select(i => i.ToString()).ToArray(),
      steps,
      best > amount ? -1 : best
    );
  }

  public static DpRecording Lis(
    int[] sequence
  )
  {
    if (sequence.Length is < 1 or > 12)
    {
      throw new ArgumentException("Sequence must have 1 to 12 elements.", nameof(sequence));
    }

    var table = Table(1, sequence.Length);
    var steps = new List<DpStep> { new(-1, -1, Copy(table), []) };
    for (var i = 0; i < sequence.Length; i++)
    {
      table[0][i] = 1;
      var dependencies = new List<(int, int)>();
      for (var j = 0; j < i; j++)
      {
        if (sequence[j] >= sequence[i])
        {
          continue;
        }

        table[0][i] = Math.Max(table[0][i], table[0][j] + 1);
        dependencies.Add((0, j));
      }

      steps.Add(new(0, i, Copy(table), [.. dependencies]));
    }

    // Unlike Fibonacci/Knapsack/LCS, the answer isn't the last cell — the longest run can end
    // anywhere in the sequence, so the result is the maximum over the whole row.
    return new(["dp[i]"], sequence.Select(v => v.ToString()).ToArray(), steps, table[0].Max());
  }
}
