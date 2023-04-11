using System.Reactive;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

internal class FolderViewModelDesign : IFolderViewModel
{
    public string Path { get; set; }
    public ICommand Go { get; }
    public ReactiveCommand<Unit, Result> Delete { get; }
}