using System.Windows.Input;

namespace SeaweedFS.Gui.Features.Session;

public interface IFolderViewModel : IEntryViewModel
{
    ICommand Go { get; }
}