using System.Windows.Input;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Session;

public interface IFolderViewModel : IEntryViewModel
{
    ICommand Go { get; }
}