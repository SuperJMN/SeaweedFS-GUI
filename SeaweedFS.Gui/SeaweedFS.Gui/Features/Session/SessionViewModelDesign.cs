using System;
using System.Reactive.Linq;
using ReactiveUI;
using SeaweedFS.Gui.Features.Transfer;

namespace SeaweedFS.Gui.Features.Session;

class SessionViewModelDesign : ISessionViewModel
{
    public IObservable<IFolderContentsViewModel> CurrentFolder { get; set; }
    public IReactiveCommand GoBack { get; }
    public ITransferManager TransferManager { get; }

    public static SessionViewModelDesign SampleData => new()
    {
        CurrentFolder = Observable.Return(new FolderContentsViewModelDesign()
        {
            ChildrenItems =
            {
                new EntryViewModelHostDesign()
                {
                    IsSelected = false,
                    ViewModel = new FolderItemViewModelDesign { Path = "home"},
                },
                new EntryViewModelHostDesign()
                {
                    IsSelected = true,
                    ViewModel = new FileItemViewModelDesign()
                    {
                        Path = "home/file1.txt",
                    }
                },
                new EntryViewModelHostDesign()
                {
                    IsSelected = false,
                    ViewModel = new FileViewModelDesign()
                    {
                        Path = "home/file2.txt",
                    }
                },
                new EntryViewModelHostDesign()
                {
                    IsSelected = true,
                    ViewModel = new FileViewModelDesign()
                    {
                        Path = "home/file3.txt",
                    }
                }
            }
        })
    };
}