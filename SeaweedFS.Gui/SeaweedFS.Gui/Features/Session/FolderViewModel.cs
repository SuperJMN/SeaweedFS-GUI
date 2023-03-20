using System;
using System.Windows.Input;
using ReactiveUI;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

internal class FolderViewModel : IFolderViewModel
{
    public FolderViewModel(IFolder fo, Action<string> onGo)
    {
        Path = fo.Path;
        Go = ReactiveCommand.Create(() => onGo(Path));
    }

    public string Path { get; }
    public ICommand Go { get; }
}