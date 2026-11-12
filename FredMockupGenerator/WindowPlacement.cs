namespace FredMockupGenerator;

public partial class MainWindow
{
  private sealed class WindowPlacement
  {
    public double Left { get; set; }
    public double Top { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public bool IsMaximized { get; set; }
  }
}
