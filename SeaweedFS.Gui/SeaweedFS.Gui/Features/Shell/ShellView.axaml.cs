using Avalonia;
using Avalonia.Controls;

namespace SeaweedFS.Gui.Features.Shell
{
    public partial class ShellView : UserControl
    {
        public ShellView()
        {
            InitializeComponent();
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            if (!Design.IsDesignMode)
            {
                DataContext = CompositionRoot.Create(TopLevel.GetTopLevel(this));
            }
        }
    }
}
