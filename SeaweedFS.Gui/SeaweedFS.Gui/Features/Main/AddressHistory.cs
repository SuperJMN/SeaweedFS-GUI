namespace SeaweedFS.Gui.Features.Main;

public class AddressHistory : History<string>, IAddressHistory
{
    public AddressHistory(string initial) : base(initial)
    {
    }
}