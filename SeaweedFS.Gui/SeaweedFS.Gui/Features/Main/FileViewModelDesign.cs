using System.Reactive;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Main;

public class FileViewModelDesign : IFileViewModel
{
    public string Name { get; set; }
    public string Path { get; set; }
    public ICommand Download => ReactiveCommand.Create(() => { });
    public ReactiveCommand<Unit, Result> Delete { get; }
}