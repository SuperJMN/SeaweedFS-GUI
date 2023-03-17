using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using SeaweedFS.Gui.Model;
using SeaweedFS.Gui.SeaweedFS;

namespace SeaweedFS.Gui.Features.ShellCopy;

internal class Root : IRoot
{
    private readonly ISeaweedFS seaweed;

    public Root(ISeaweedFS seaweed)
    {
        this.seaweed = seaweed;
    }

    public Task<Result<SeaweedFolder>> Get(string path)
    {
        return SeaweedFolder.Create(path, seaweed);
    }
}