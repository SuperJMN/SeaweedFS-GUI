using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Main;

public class History : ReactiveObject, IHistory
{
    private readonly Stack<IFolderViewModel> currentFolderStack;

    public History(IFolderViewModel initial)
    {
        currentFolderStack = new Stack<IFolderViewModel>(new[] { initial });
        var whenAnyValue = this.WhenAnyValue(x => x.CanGoBack);
        GoBack = ReactiveCommand.Create(OnBack, whenAnyValue);
    }

    public ReactiveCommand<Unit, Unit> GoBack { get; }

    private bool CanGoBack => currentFolderStack.Count > 1;

    public IFolderViewModel CurrentFolder
    {
        get => currentFolderStack.Peek();
        set
        {
            if (value == currentFolderStack.Peek())
            {
                return;
            }

            currentFolderStack.Push(value);
            this.RaisePropertyChanged(nameof(CanGoBack));
            this.RaisePropertyChanged(nameof(PreviousFolder));
            this.RaisePropertyChanged();
        }
    }

    public IFolderViewModel PreviousFolder => currentFolderStack.SkipLast(1).First();

    private void OnBack()
    {
        currentFolderStack.Pop();
        this.RaisePropertyChanged(nameof(CurrentFolder));
        this.RaisePropertyChanged(nameof(PreviousFolder));
        this.RaisePropertyChanged(nameof(CanGoBack));
    }
}