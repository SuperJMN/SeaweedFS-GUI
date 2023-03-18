using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

internal class FolderViewModel : IFolderViewModel
{
    public FolderViewModel(IFolder fo)
    {
        Path = fo.Path;
    }

    public string Path { get; }
}