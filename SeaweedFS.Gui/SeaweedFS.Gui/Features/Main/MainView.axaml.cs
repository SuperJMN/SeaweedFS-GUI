using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace SeaweedFS.Gui.Features.Main;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (!Design.IsDesignMode)
        {
            DataContext = CompositionRoot.Create(TopLevel.GetTopLevel(this)!);
        }
    }
}