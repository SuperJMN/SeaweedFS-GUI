using System.Collections.ObjectModel;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.Features.Transfer;

public interface ITransferManager
{
    ReadOnlyObservableCollection<ITransferViewModel> Transfers { get; }
    void Add(ITransferViewModel transfer);
    void Remove(TransferKey key);
}