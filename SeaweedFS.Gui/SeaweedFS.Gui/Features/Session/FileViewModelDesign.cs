using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

public class FileViewModelDesign : IFileViewModel
{
    public string Path { get; set; }
    public IReactiveCommand Download => ReactiveCommand.Create(() => { });
    public IReactiveCommand Delete { get; }
}