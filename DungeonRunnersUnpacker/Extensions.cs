using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace DungeonRunnersUnpacker;

public static class Extensions
{
    public unsafe static T[] Read<T>(this BinaryReader reader, int count) where T : struct
    {
        var result = new T[count];
        var buffer = reader.ReadBytes(Unsafe.SizeOf<T>() * count);
        Unsafe.CopyBlockUnaligned(Unsafe.AsPointer(ref result[0]), Unsafe.AsPointer(ref buffer[0]), (uint)buffer.Length);
        return result;
    }

    public static T Read<T>(this BinaryReader reader) where T : struct
    {
        var buffer = reader.ReadBytes(Unsafe.SizeOf<T>());
        return Unsafe.ReadUnaligned<T>(ref buffer[0]);
    }

    public static string ReadCString(this BinaryReader reader)
    {
        var sb = new StringBuilder(0x40);

        byte b;
        while ((b = reader.ReadByte()) != 0)
            sb.Append((char)b);

        return sb.ToString();
    }
}
