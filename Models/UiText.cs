namespace AlgoMotion.Models;

/// <summary>Every fixed (non-per-algorithm, non-per-step) UI string in the app — nav links,
/// control labels, button text, section headers, status-chip labels, tooltips. Per-algorithm
/// text (name/subtitle/hint/counter labels) lives on <c>SortAlgorithmInfo</c>/<c>TreeSearchAlgorithmInfo</c>
/// themselves; per-step captions are built inline by each simulator's <c>Record</c> method using
/// its own local <c>T(vi, en)</c> helper — both are too numerous and too context-specific to route
/// through one shared enum the way this shorter, fixed set of strings can.</summary>
public enum UiTextKey
{
  NavSort,
  NavTreeSearch,
  NavGroupVisualizer,
  NavGroupLearn,
  NavMenuOpen,
  NavMenuClose,
  BrandTagline,
  SubSort,
  SubTreeSearch,
  SubComplexity,
  SubCompare,
  SubQuiz,
  SubGraphs,
  SubDp,
  SubSearching,

  AlgorithmLabel,
  CodeLanguageLabel,
  UiLanguageLabel,

  Play,
  Pause,
  StepBack,
  Step,
  Reset,
  Replay,
  ScrubberLabel,

  NewArray,
  NewArrayTitle,
  NewTree,
  NewTreeTitle,

  CustomInputPlaceholderArray,
  CustomInputPlaceholderTree,
  CustomInputApply,
  CustomInputError,

  DepthLabel,
  TargetLabel,
  SpeedLabel,

  ZoomIn,
  ZoomOut,
  FitView,
  FitViewTitle,

  CurrentSection,
  HistorySection,

  LegendUnvisited,
  LegendActive,
  LegendVisited,
  LegendQueued,
  LegendFound,

  ChipN,
  ChipI,
  ChipJ,
  ChipSwapped,
  ChipTarget,
  ChipCurrentNode,
  ChipFound,
  ChipChecked,

  True,
  False,

  InitialArrayCaption,
  InitialTreeCaption,

  PwaInstall,
  PwaUpdateAvailable,
  PwaUpdateAction
}

public static class UiText
{
  private static readonly Dictionary<UiTextKey, (string Vi, string En)> Values = new()
  {
    [UiTextKey.NavSort] = ("Sắp xếp", "Sort"),
    [UiTextKey.NavTreeSearch] = ("Tìm kiếm trên cây", "Tree Search"),
    [UiTextKey.NavGroupVisualizer] = ("Minh họa", "Visualizer"),
    [UiTextKey.NavGroupLearn] = ("Học tập", "Learn"),
    [UiTextKey.NavMenuOpen] = ("Mở menu", "Open menu"),
    [UiTextKey.NavMenuClose] = ("Đóng menu", "Close menu"),
    [UiTextKey.BrandTagline] = ("Trình minh họa thuật toán", "Algorithm visualizer"),
    [UiTextKey.SubSort] = ("Xem từng bước các thuật toán sắp xếp", "Step through sorting algorithms"),
    [UiTextKey.SubTreeSearch] = ("Duyệt và tìm kiếm trên cây BST/AVL", "Traverse and search BST/AVL trees"),
    [UiTextKey.SubComplexity] =
      ("Đo số phép so sánh và hoán đổi khi n tăng", "Measure comparisons and swaps as n grows"),
    [UiTextKey.SubCompare] = ("Chạy hai thuật toán cạnh nhau", "Run two algorithms side by side"),
    [UiTextKey.SubQuiz] = ("Kiểm tra lại kiến thức của bạn", "Test what you have learned"),
    [UiTextKey.SubGraphs] = ("Xem từng bước các thuật toán trên đồ thị", "Step through graph algorithms"),
    [UiTextKey.SubDp] = ("Xem từng bước bảng quy hoạch động", "Step through dynamic-programming tables"),
    [UiTextKey.SubSearching] =
      ("Xem từng bước các thuật toán tìm kiếm trên mảng đã sắp xếp", "Step through search algorithms on a sorted array"),
    [UiTextKey.AlgorithmLabel] = ("Thuật toán", "Algorithm"),
    [UiTextKey.CodeLanguageLabel] = ("Ngôn ngữ code", "Code language"),
    [UiTextKey.UiLanguageLabel] = ("Giao diện", "Interface"),
    [UiTextKey.Play] = ("▶ Phát", "▶ Play"),
    [UiTextKey.Pause] = ("⏸ Tạm dừng", "⏸ Pause"),
    [UiTextKey.StepBack] = ("⏮ Lùi", "⏮ Back"),
    [UiTextKey.Step] = ("⏭ Bước", "⏭ Step"),
    [UiTextKey.Reset] = ("⟲ Đặt lại", "⟲ Reset"),
    [UiTextKey.Replay] = ("↻ Phát lại", "↻ Replay"),
    [UiTextKey.ScrubberLabel] = ("Tua tới bước", "Scrub to step"),
    [UiTextKey.NewArray] = ("🔀 Mảng mới", "🔀 New array"),
    [UiTextKey.NewArrayTitle] =
      ("Tạo mảng ngẫu nhiên mới cùng kích thước", "Generate a new random array of the same size"),
    [UiTextKey.NewTree] = ("🔀 Cây mới", "🔀 New tree"),
    [UiTextKey.NewTreeTitle] =
      ("Tạo cây ngẫu nhiên mới cùng kích thước", "Generate a new random tree of the same size"),
    [UiTextKey.CustomInputPlaceholderArray] = ("vd: 5, 2, 8, 1, 9", "e.g. 5, 2, 8, 1, 9"),
    [UiTextKey.CustomInputPlaceholderTree] = ("vd: 5, 2, 8, 1, 9", "e.g. 5, 2, 8, 1, 9"),
    [UiTextKey.CustomInputApply] = ("Áp dụng", "Apply"),
    [UiTextKey.CustomInputError] =
      ("Nhập từ {0} đến {1} số nguyên trong khoảng 1–10000, cách nhau bằng dấu phẩy, dấu chấm phẩy hoặc khoảng trắng.",
        "Enter {0}–{1} integers from 1 to 10000, separated by commas, semicolons, or whitespace."),
    [UiTextKey.DepthLabel] = ("Độ sâu", "Depth"),
    [UiTextKey.TargetLabel] = ("Mục tiêu", "Target"),
    [UiTextKey.SpeedLabel] = ("Tốc độ", "Speed"),
    [UiTextKey.ZoomIn] = ("Phóng to", "Zoom in"),
    [UiTextKey.ZoomOut] = ("Thu nhỏ", "Zoom out"),
    [UiTextKey.FitView] = ("⛶ Vừa khung", "⛶ Fit view"),
    [UiTextKey.FitViewTitle] = ("Chỉnh zoom để nhìn toàn bộ cây", "Adjust zoom to fit the whole tree"),
    [UiTextKey.CurrentSection] = ("HIỆN TẠI", "CURRENT"),
    [UiTextKey.HistorySection] = ("LỊCH SỬ", "HISTORY"),
    [UiTextKey.LegendUnvisited] = ("Chưa ghé", "Unvisited"),
    [UiTextKey.LegendActive] = ("Đang xét", "Active"),
    [UiTextKey.LegendVisited] = ("Đã ghé", "Visited"),
    [UiTextKey.LegendQueued] = ("Trong hàng đợi", "Queued"),
    [UiTextKey.LegendFound] = ("Tìm thấy", "Found"),
    [UiTextKey.ChipN] = ("n", "n"),
    [UiTextKey.ChipI] = ("i", "i"),
    [UiTextKey.ChipJ] = ("j", "j"),
    [UiTextKey.ChipSwapped] = ("đã đổi chỗ", "swapped"),
    [UiTextKey.ChipTarget] = ("mục tiêu", "target"),
    [UiTextKey.ChipCurrentNode] = ("node hiện tại", "current node"),
    [UiTextKey.ChipFound] = ("tìm thấy", "found"),
    [UiTextKey.ChipChecked] = ("vị trí xét", "checked"),
    [UiTextKey.True] = ("đúng", "true"),
    [UiTextKey.False] = ("sai", "false"),
    [UiTextKey.InitialArrayCaption] =
      ("Dãy ban đầu. Nhấn Play để bắt đầu sắp xếp.", "Initial array. Press Play to start sorting."),
    [UiTextKey.InitialTreeCaption] =
      ("Cây ban đầu. Nhấn Play để bắt đầu tìm kiếm.", "Initial tree. Press Play to start the search."),
    [UiTextKey.PwaInstall] = ("Cài đặt ứng dụng", "Install app"),
    [UiTextKey.PwaUpdateAvailable] = ("Có bản cập nhật mới.", "A new version is available."),
    [UiTextKey.PwaUpdateAction] = ("Tải lại", "Reload")
  };

  public static string Get(
    UiTextKey key,
    UiLanguage language
  )
  {
    var (vi, en) = Values[key];
    return language == UiLanguage.En ? en : vi;
  }

  /// <summary>Localizes a boolean the same way status chips already render it — "true"/"false" in
  /// English, "đúng"/"sai" in Vietnamese — instead of a raw <c>bool.ToString()</c>.</summary>
  public static string Bool(
    bool value,
    UiLanguage language
  )
  {
    return Get(value ? UiTextKey.True : UiTextKey.False, language);
  }
}
