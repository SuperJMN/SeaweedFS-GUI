using System.Reactive.Linq;
using Refit;
using SeaweedFS.Gui.SeaweedFS;
using SeaweedFS.Gui.ViewModels;
using Zafiro.Core.Mixins;
using Zafiro.UI.Transfers;

namespace SeaweedFS.Tests
{
    public class UnitTest1
    {
        //[Fact]
        //public async Task Test1()
        //{
        //    var client = CreateClient();

        //    var httpResponseMessage = await client.GetContents("Vídeo sin título.mp4");
        //    var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
        //    var obs = stream.ToObservableAlternate();
        //    await obs.LastOrDefaultAsync();
        //}

        [Fact]
        public async Task Copier()
        {
            var input = new MemoryStream(new byte[] { 1, 23 });
            var output = new MemoryStream();

            var sut = new StreamTransfer("Test", () => Task.FromResult((Stream)new TestStream(input)), () => Task.FromResult((Stream)new TestStream(output)));
            //await sut.Start.Execute();
        }

        private static ISeaweedApi CreateClient()
        {
            var httpClient = new HttpClient();
            var uriString = "http://192.168.1.31:8888";
            var uri = new Uri(uriString);
            httpClient.BaseAddress = uri;
            var client = RestService.For<ISeaweedApi>(httpClient);
            return client;
        }
    }

    public class TestStream : Stream
    {
        private readonly Stream streamImplementation;

        public TestStream(Stream inner)
        {
            streamImplementation = inner;
        }

        public override void Flush()
        {
            streamImplementation.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return streamImplementation.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return streamImplementation.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            streamImplementation.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            streamImplementation.Write(buffer, offset, count);
        }

        public override bool CanRead => streamImplementation.CanRead;

        public override bool CanSeek => streamImplementation.CanSeek;

        public override bool CanWrite => streamImplementation.CanWrite;

        public override long Length => streamImplementation.Length;

        public override long Position
        {
            get => streamImplementation.Position;
            set => streamImplementation.Position = value;
        }
    }
}