using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

internal class FileItemViewModelDesign : IFileViewModel
{
    public string Path { get; set; }
    public IReactiveCommand Download { get; }
    public ReactiveCommand<Unit, Result> Delete { get; }
}