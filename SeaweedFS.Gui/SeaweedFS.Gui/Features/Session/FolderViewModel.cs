using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

internal class FolderViewModel : IFolderViewModel
{
    public FolderViewModel(IFolder fo, Action<string> onGo, Func<string, Task<Result>> onDelete)
    {
        Path = fo.Path;
        Go = ReactiveCommand.Create(() => onGo(Path));
        Delete = ReactiveCommand.CreateFromTask(() => onDelete(fo.Name));
    }

    public string Path { get; }
    public ICommand Go { get; }
    public ReactiveCommand<Unit, Result> Delete { get; }
}