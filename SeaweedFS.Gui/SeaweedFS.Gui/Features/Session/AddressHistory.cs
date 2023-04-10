namespace SeaweedFS.Gui.Features.Session;

public class AddressHistory : History<string>, IAddressHistory
{
    public AddressHistory(string initial) : base(initial)
    {
    }
}