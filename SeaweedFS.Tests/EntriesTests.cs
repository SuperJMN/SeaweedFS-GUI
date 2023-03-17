using DynamicData;
using Moq;
using SeaweedFS.Gui.Model;
using SeaweedFS.Gui.SeaweedFS;

namespace SeaweedFS.Tests;

public class EntriesTests
{
    [Fact]
    public void Folder_contents_are_exposed()
    {
        var folders = SampleFolders();

        var sut = new SeaweedFolder("folder", SeaweedService(folders));
        sut.Children.Bind(out var contents).Subscribe();
        contents.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Delete_should_succeed()
    {
        var folders = SampleFolders();

        var sut = new SeaweedFolder("folder", SeaweedService(folders));
        sut.Children.Bind(out var contents).Subscribe();
        var delete = await sut.Delete(contents.First());
        delete.Should().BeSuccess();
    }

    [Fact]
    public async Task Delete_should_remove_the_item()
    {
        var folders = SampleFolders();

        var sut = new SeaweedFolder("folder", SeaweedService(folders));
        sut.Children.Bind(out var contents).Subscribe();
        var toDelete = contents.First();
        await sut.Delete(toDelete);
        contents.Should().NotContain(toDelete);
    }

    [Fact]
    public async Task Add_should_succeed()
    {
        var folders = SampleFolders();

        var sut = new SeaweedFolder("folder", SeaweedService(folders));
        sut.Children.Bind(out var contents).Subscribe();
        var created = await sut.Add("test.jpg", new MemoryStream("pepito"u8.ToArray()), CancellationToken.None);
        created.Should().BeSuccess();
    }

    [Fact]
    public async Task Add_should_be_reflected_in_collection()
    {
        var folders = SampleFolders();

        var sut = new SeaweedFolder("folder", SeaweedService(folders));
        sut.Children.Bind(out var contents).Subscribe();
        var created = await sut.Add("test.jpg", new MemoryStream("pepito"u8.ToArray()), CancellationToken.None);
        contents.Should().Contain(created.Value);
    }

    private static Folder SampleFolders()
    {
        return new Folder()
        {
            Path = "folder/",
            Entries = new []
            {
                new Entry(){FullPath = "folder/file1.txt", Chunks = new List<Chunk>() },
                new Entry(){FullPath = "folder/file2.txt", Chunks = new List<Chunk>() },
            }.ToList(),
        };
    }

    private static ISeaweedFS SeaweedService(Folder folders)
    {
        return Mock.Of<ISeaweedFS>(x => x.GetContents("folder") == Task.FromResult(folders));
    }
}