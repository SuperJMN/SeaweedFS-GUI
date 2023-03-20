using System.Windows.Input;

namespace SeaweedFS.Gui.Features.Session;

internal class FolderItemViewModelDesign : IFolderViewModel
{
    public string Path { get; set; }
    public ICommand Go { get; }
}