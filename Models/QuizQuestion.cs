namespace AlgoMotion.Models;

public sealed record QuizQuestion(string Id, int Answer, int OptionCount = 3);

public static class QuizBank
{
  public static readonly IReadOnlyList<QuizQuestion> All =
  [
    new("Bubble", 1), new("Binary", 0), new("Stable", 2), new("Bfs", 1),
    new("Dijkstra", 2), new("Prim", 0), new("Knapsack", 1), new("Lcs", 2)
  ];
}

public sealed class QuizAttempt
{
  public int Index { get; private set; }
  public int Score { get; private set; }
  public int? Selected { get; private set; }
  public bool Finished => Index >= QuizBank.All.Count;
  public QuizQuestion Current => QuizBank.All[Math.Min(Index, QuizBank.All.Count - 1)];
  public bool Answer(int option)
  {
    if (Finished || Selected.HasValue || option < 0 || option >= Current.OptionCount) return false;
    Selected = option;
    if (option == Current.Answer) Score++;
    return true;
  }
  public void Next()
  {
    if (!Selected.HasValue || Finished) return;
    Index++;
    Selected = null;
  }
  public void Reset() { Index = 0; Score = 0; Selected = null; }
}
