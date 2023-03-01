using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using ReactiveUI;
using SeaweedFS.Gui.SeaweedFS;
using Zafiro.FileSystem;
using Zafiro.UI;

namespace SeaweedFS.Gui.ViewModels;

class FileViewModel : EntryViewModel
{
    private readonly ISeaweed seaweed;

    public FileViewModel(string path, ISeaweed seaweed, ISaveFilePicker saveFilePicker)
    {
        this.seaweed = seaweed;
        Path = path;
        Download = ReactiveCommand.CreateFromObservable(() =>
        {
            return saveFilePicker
                .Pick(System.IO.Path.GetFileNameWithoutExtension(path), System.IO.Path.GetExtension(path))
                .SelectMany(async maybe =>
                {
                    if (maybe.HasValue)
                    {
                        return await SaveTo(path, maybe.Value);
                    }

                    return Result.Success();
                });
        });
    }

    private async Task<Result<Unit>> SaveTo(string path, IStorable storable)
    {
        var fileContent = await seaweed.GetFileContent(path);
        await using var source = await fileContent.Content.ReadAsStreamAsync();
        await using (var destination = await storable.OpenWrite())
        {
            await source.CopyToAsync(destination);
        }

        return Result.Success(Unit.Default);
    }

    public ReactiveCommand<Unit, Result> Download { get; }

    public string Path { get; }
    public string Name => System.IO.Path.GetFileName(Path);
}
