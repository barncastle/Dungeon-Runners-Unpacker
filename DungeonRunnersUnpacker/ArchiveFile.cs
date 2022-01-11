using DungeonRunnersUnpacker.Models;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Buffers;
using System.IO;
using System.Text;

namespace DungeonRunnersUnpacker;

internal class ArchiveFile : IDisposable
{
    public const string ArchiveFileName = "game.pkg";

    public Guid Guid { get; }
    public Stream Stream { get; }
    public string FilePath { get; }

    public ArchiveFile(string directory)
    {
        FilePath = Path.Combine(directory, ArchiveFileName);

        if (!File.Exists(FilePath))
            throw new FileNotFoundException($"Missing {ArchiveFileName}");

        Stream = File.OpenRead(FilePath);
        using BinaryReader reader = new(Stream, Encoding.UTF8, true);

        Guid = reader.Read<Guid>();
    }

    public void ExtractFile(IndexEntry entry, string filename)
    {
        // ensure directory exists
        Directory.CreateDirectory(Directory.GetParent(filename).FullName);
        using var filestream = File.OpenWrite(filename);

        // scan to offset
        Stream.Position = entry.ArchiveOffset;

        if (entry.IsCompressed)
        {
            // extract and zlib inflate
            using SubStream substream = new(Stream, entry.CompressedSize);
            using InflaterInputStream inflater = new(substream);
            inflater.CopyTo(filestream);
        }
        else
        {
            // flat file extraction
            var buffer = ArrayPool<byte>.Shared.Rent(entry.DecompressedSize);
            Stream.Read(buffer, 0, entry.DecompressedSize);
            filestream.Write(buffer, 0, entry.DecompressedSize);
            ArrayPool<byte>.Shared.Return(buffer);
        }

        filestream.Flush();

        if (filestream.Length != entry.DecompressedSize)
            throw new Exception($"Failed extracting {filename}!");

        filestream.Close();
    }

    public void Dispose() => Stream.Dispose();
}
