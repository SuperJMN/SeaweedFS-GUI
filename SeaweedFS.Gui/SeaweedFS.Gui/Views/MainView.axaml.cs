using Avalonia;
using Avalonia.Controls;
using Avalonia.LogicalTree;

namespace SeaweedFS.Gui.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        DataContext = CompositionRoot.Create(TopLevel.GetTopLevel(this)!);
    }
}