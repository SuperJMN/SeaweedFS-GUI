using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using CSharpFunctionalExtensions;
using ReactiveUI;

namespace SeaweedFS.Gui.Features.Main;

public class History<T> : ReactiveObject, IHistory<T>
{
    private readonly Stack<T> currentFolderStack;

    public History(T initial)
    {
        currentFolderStack = new Stack<T>(new[] { initial });
        var whenAnyValue = this.WhenAnyValue(x => x.CanGoBack);
        GoBack = ReactiveCommand.Create(OnBack, whenAnyValue);
    }

    private bool CanGoBack => currentFolderStack.Count > 1;

    public ReactiveCommand<Unit, Unit> GoBack { get; }

    public T CurrentFolder
    {
        get => currentFolderStack.Peek();
        set
        {
            if (Equals(value, currentFolderStack.Peek()))
            {
                return;
            }

            currentFolderStack.Push(value);
            this.RaisePropertyChanged(nameof(CanGoBack));
            this.RaisePropertyChanged(nameof(PreviousFolder));
            this.RaisePropertyChanged();
        }
    }

    public Maybe<T> PreviousFolder => currentFolderStack.SkipLast(1).TryFirst();

    private void OnBack()
    {
        currentFolderStack.Pop();
        this.RaisePropertyChanged(nameof(CurrentFolder));
        this.RaisePropertyChanged(nameof(PreviousFolder));
        this.RaisePropertyChanged(nameof(CanGoBack));
    }
}