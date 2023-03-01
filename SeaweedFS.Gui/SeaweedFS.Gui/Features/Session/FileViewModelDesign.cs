using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

public class FileViewModelDesign : IFileViewModel
{
    private ReactiveCommand<Unit, Result> delete;
    public string Path { get; set; }
    public IReactiveCommand Download => ReactiveCommand.Create(() => { });
    public ReactiveCommand<Unit, Result> Delete { get; }
}