using System;
using System.Reactive.Linq;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.Session;

class SessionViewModelDesign : ISessionViewModel
{
    public IObservable<INewFolderViewModel> CurrentFolder { get; set; }

    public static SessionViewModelDesign SampleData => new()
    {
        CurrentFolder = Observable.Return(new NewFolderViewModelDesign()
        {
            ChildrenItems =
            {
                new EntryViewModelDesign()
                {
                    IsSelected = false,
                    EntryModel = new FolderModelDesign() { Path = "home"},
                },
                new EntryViewModelDesign()
                {
                    IsSelected = true,
                    EntryModel = new FileModelDesign()
                    {
                        Path = "home/file1.txt",
                    }
                },
                new EntryViewModelDesign()
                {
                    IsSelected = false,
                    EntryModel = new FileModelDesign()
                    {
                        Path = "home/file2.txt",
                    }
                },
                new EntryViewModelDesign()
                {
                    IsSelected = true,
                    EntryModel = new FileModelDesign()
                    {
                        Path = "home/file3.txt",
                    }
                }
            }
        })
    };
}