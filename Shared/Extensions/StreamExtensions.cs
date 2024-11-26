namespace Shared.Extensions;

public static class StreamExtensions
{
    public static byte[] GetBuffer(this Stream stream)
    {
        stream.Position = 0;

        switch (stream)
        {
            case MemoryStream mem:
                return mem.ToArray();
            default:
                using (var m = new MemoryStream())
                {
                    stream.CopyTo(m);
                    return m.ToArray();
                }
        }
    }
}