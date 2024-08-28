using System.Windows;

namespace Key2Btn.MainView.Interfaces
{
    public interface ITaskbarIconVM : IVM
    {
        string ToolTipText { get; set; }
        Visibility Visibility { get; set; }
        bool ContextMenuIsOpen { get; set; }
    }
}
