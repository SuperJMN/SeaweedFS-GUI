using System.IO;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.SeaweedFS;
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
        //var stream = File.OpenRead(@"D:\5 - Unimportant\Descargas\linux_amd64.tar.gz");
        //return stream;
        using (var response = await seaweed.GetFileContent(Path))
        {
            var readAsStreamAsync = await response.Content.ReadAsStreamAsync();
            var ms = new MemoryStream();
            await readAsStreamAsync.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
        }
    }

    public ICommand Download { get; }

    public string Path { get; }

    public string Name => System.IO.Path.GetFileName(Path);
}

internal interface IFileViewModel : IEntryViewModel
{
    public ICommand Download { get; }
}