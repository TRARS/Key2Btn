using System.Windows.Media;

namespace Key2Btn.MainView.Interfaces
{
    public interface IuRainbowLineVM : IVM
    {
        Brush BrushColor { get; set; }
        double Width { get; set; }
        double Height { get; set; }
    }
}