using System;
using System.Windows.Input;
using ReactiveUI;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

internal class FolderViewModel : IFolderViewModel
{
    public FolderViewModel(IFolder fo, Action<string> onGo, Action<string> onDelete)
    {
        Path = fo.Path;
        Go = ReactiveCommand.Create(() => onGo(Path));
        Delete = ReactiveCommand.Create(() => onDelete(fo.Name));
    }

    public string Path { get; }
    public ICommand Go { get; }
    public IReactiveCommand Delete { get; }
}