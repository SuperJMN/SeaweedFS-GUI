using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

internal class FileItemViewModelDesign : IFileViewModel
{
    public string Path { get; set; }
    public IReactiveCommand Download { get; }
    public IReactiveCommand Delete { get; }
}