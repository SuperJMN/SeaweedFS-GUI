using System.Threading.Tasks;
using CSharpFunctionalExtensions;

namespace SeaweedFS.Gui.Model;

public interface IRoot
{
    Task<Result<IFolder>> Get(string path);
}