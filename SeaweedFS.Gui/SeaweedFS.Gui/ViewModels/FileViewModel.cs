using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.Core;
using Zafiro.UI;

namespace SeaweedFS.Gui.ViewModels;

class FileViewModel : EntryViewModel, IFileViewModel
{
    private readonly ISeaweed seaweed;

    public FileViewModel(string path, ISeaweed seaweed, ISaveFilePicker saveFilePicker, ITransferManager transferManager)
    {
        this.seaweed = seaweed;
        Path = path;
        Download = ReactiveCommand.CreateFromObservable(() =>
        {
            return saveFilePicker
                .Pick(System.IO.Path.GetFileNameWithoutExtension(path), System.IO.Path.GetExtension(path)[1..])
                .SelectMany(maybe =>
                {
                    if (maybe.HasValue)
                    {
                        transferManager.Add(new Transfer(Path, async () => await GetStream(), () => maybe.Value.OpenWrite()));
                    }

                    return Task.FromResult(Result.Success());
                });
        });
    }

    private async Task<Stream> GetStream()
    {
        return await HttpResponseMessageStream.Create(await seaweed.GetFileContent(Path));
    }

    public ICommand Download { get; }

    public string Path { get; }

    public string Name => System.IO.Path.GetFileName(Path);
}

internal interface IFileViewModel : IEntryViewModel
{
    public ICommand Download { get; }
}