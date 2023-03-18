using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

internal class FileViewModel : IFileViewModel
{
    public FileViewModel(IFileModel fi)
    {
        Path = fi.Path;
    }

    public string Path { get; }
}