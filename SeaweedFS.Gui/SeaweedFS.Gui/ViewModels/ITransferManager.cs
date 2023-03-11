using System.Collections.ObjectModel;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.ViewModels;

public interface ITransferManager
{
    ReadOnlyObservableCollection<ITransfer> Transfers { get; }
    void Add(ITransfer transfer);
}