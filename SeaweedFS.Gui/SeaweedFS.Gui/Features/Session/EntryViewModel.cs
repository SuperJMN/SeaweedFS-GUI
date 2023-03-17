using ReactiveUI;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

public class EntryViewModel : ReactiveObject, IEntryViewModel
{
    public EntryViewModel(IEntryModel entryModel)
    {
        EntryModel = entryModel;
    }

    public IEntryModel EntryModel { get; set; }
    public bool IsSelected { get; set; }
}