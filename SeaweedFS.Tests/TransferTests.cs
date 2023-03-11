using System.Reactive.Linq;
using System.Text;
using SeaweedFS.Gui.ViewModels;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Tests;

public class TransferTests
{
    [Fact]
    public async Task Transfer()
    {
        var memoryStream = new MemoryStream("Saludos"u8.ToArray());
        var sut = new SendTransfer("Test", () => Task.FromResult((Stream)memoryStream), new Uri("http://192.168.1.31:8888/Guión.txt"));
        sut.Start.ThrownExceptions.Subscribe(exception => { });
        await sut.Start.Execute();
    }
}