using System.Reactive;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

public class FileViewModelDesign : IFileViewModel
{
    public string Path { get; set; }
    public ICommand Download => ReactiveCommand.Create(() => { });
    public ReactiveCommand<Unit, Result> Delete { get; }
}