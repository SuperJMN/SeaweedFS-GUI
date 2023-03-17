using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using SeaweedFS.Gui.Model;

namespace SeaweedFS.Gui.Features.ShellCopy;

public interface IRoot
{
    Task<Result<SeaweedFolder>> Get(string path);
}