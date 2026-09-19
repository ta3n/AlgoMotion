using System.Globalization;

namespace AlgoMotion.Services;

public static class CustomInput
{
  public const int MaxValue = 10000;

  public static bool TryParse(string text, int minCount, int maxCount, out int[] values)
  {
    values = [];
    var parts = text.Split([',', ';', ' ', '\t', '\r', '\n'],
      StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (parts.Length < minCount || parts.Length > maxCount) return false;
    var parsed = new int[parts.Length];
    for (var i = 0; i < parts.Length; i++)
    {
      if (!int.TryParse(parts[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out parsed[i])
          || parsed[i] is < 1 or > MaxValue) return false;
    }
    values = parsed;
    return true;
  }
}
