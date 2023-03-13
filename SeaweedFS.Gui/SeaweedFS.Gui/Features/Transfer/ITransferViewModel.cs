using Zafiro.UI.Transfers;

namespace SeaweedFS.Gui.Features.Transfer;

public interface ITransferViewModel : ITransfer
{
    public string Icon { get; }
}