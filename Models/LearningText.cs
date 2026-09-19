using System.Resources;

namespace AlgoMotion.Models;

public static class LearningText
{
  private static readonly ResourceManager Vi = new("AlgoMotion.Resources.Learning", typeof(LearningText).Assembly);
  private static readonly ResourceManager En = new("AlgoMotion.Resources.Learning.English", typeof(LearningText).Assembly);

  public static string Get(string key, UiLanguage language) =>
    (language == UiLanguage.En ? En : Vi).GetString(key) ?? key;
}
