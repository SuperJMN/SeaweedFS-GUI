using System.Reactive;
using System.Reactive.Linq;
using CSharpFunctionalExtensions;
using Microsoft.Reactive.Testing;
using SeaweedFS.Gui.Features.Transfer;

namespace SeaweedFS.Tests;

public class UploadTests
{
    [Fact]
    public async Task Transfer_should_succeed()
    {
        var sut = new Upload("Test", () => Task.FromResult((Stream) new MemoryStream("pepito"u8.ToArray())), (_, _) => Task.FromResult(Result.Success()), _ => { });

        var testableObserver = new TestScheduler().CreateObserver<string>();

        sut.ErrorMessage.Subscribe(testableObserver);
        await sut.Start.Execute();

        testableObserver.Messages.Should().BeEmpty();
    }

    [Fact]
    public async Task Transfer_should_contain_error()
    {
        var sut = new Upload("Test", () => Task.FromResult((Stream) new MemoryStream("pepito"u8.ToArray())), (_, _) => Task.FromResult(Result.Failure("Error")), _ => { });

        var testableObserver = new TestScheduler().CreateObserver<string>();

        sut.ErrorMessage.Subscribe(testableObserver);
        await sut.Start.Execute();

        testableObserver.Messages.Should().Contain(new Recorded<Notification<string>>(0, Notification.CreateOnNext("Error")));
    }
}