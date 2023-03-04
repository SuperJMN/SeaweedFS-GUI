using System.Collections.ObjectModel;

namespace SeaweedFS.Gui.ViewModels;

public interface ITransferManager
{
    ReadOnlyObservableCollection<ITransfer> Transfers { get; }
    void Add(Transfer copier);
}