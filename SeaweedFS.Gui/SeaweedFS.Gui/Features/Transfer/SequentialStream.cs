using System;
using System.IO;

namespace SeaweedFS.Gui.Features.Transfer;

/// <summary>
/// Simulates Position when the inner stream doesn't support it.
/// </summary>
public class SequentialStream : Stream
{
    private readonly Stream inner;
    private int position;

    public SequentialStream(Stream stream)
    {
        inner = stream;
    }

    public override void Flush()
    {
        inner.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        var read = inner.Read(buffer, offset, count);
        position += read;
        return read;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotSupportedException();
    }

    public override void SetLength(long value)
    {
        inner.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        inner.Write(buffer, offset, count);
        position += count;
    }

    public override bool CanRead => inner.CanRead;

    public override bool CanSeek => inner.CanSeek;

    public override bool CanWrite => inner.CanWrite;

    public override long Length => inner.Length;

    public override long Position
    {
        get => position;
        set => throw new NotSupportedException();
    }
}