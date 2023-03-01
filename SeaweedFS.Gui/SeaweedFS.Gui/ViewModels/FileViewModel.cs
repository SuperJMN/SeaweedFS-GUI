using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.UI;

namespace SeaweedFS.Gui.ViewModels;

class FileViewModel : EntryViewModel
{
    private readonly ISeaweed seaweed;

    public FileViewModel(string path, ISeaweed seaweed, ISaveFilePicker saveFilePicker, TransferManager transferManager)
    {
        this.seaweed = seaweed;
        Path = path;
        Download = ReactiveCommand.CreateFromObservable(() =>
        {
            return saveFilePicker
                .Pick(System.IO.Path.GetFileNameWithoutExtension(path), System.IO.Path.GetExtension(path)[1..])
                .SelectMany(async maybe =>
                {
                    if (maybe.HasValue)
                    {
                        transferManager.Add(new Transfer(Path, async () => await GetStream(), () => maybe.Value.OpenWrite()));
                    }

                    return Result.Success();
                });
        });
    }

    private async Task<Stream> GetStream()
    {
        var response = await seaweed.GetFileContent(Path);
        return await response.Content.ReadAsStreamAsync();
    }

    public ReactiveCommand<Unit, Result> Download { get; }

    public string Path { get; }

    public string Name => System.IO.Path.GetFileName(Path);
}
