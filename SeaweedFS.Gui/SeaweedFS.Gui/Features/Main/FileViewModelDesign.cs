using System.Windows.Input;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Main;

public class FileViewModelDesign : IFileViewModel
{
    public string Name { get; set; }
    public string Path { get; set; }
    public ICommand Download => ReactiveCommand.Create(() => { });
}