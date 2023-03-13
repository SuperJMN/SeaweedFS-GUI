using System.Collections.ObjectModel;

namespace SeaweedFS.Gui.Features.Transfer;

public interface ITransferManager
{
    ReadOnlyObservableCollection<ITransferViewModel> Transfers { get; }
    void Add(ITransferViewModel transfer);
}