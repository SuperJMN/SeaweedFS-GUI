using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using SeaweedFS.Gui.SeaweedFS;

namespace SeaweedFS.Gui.Model;

internal class Root : IRoot
{
    private readonly ISeaweedFS seaweed;

    public Root(ISeaweedFS seaweed)
    {
        this.seaweed = seaweed;
    }

    public async Task<Result<IFolderModel>> Get(string path)
    {
        return await FolderModel.Create(path, seaweed);
    }
}