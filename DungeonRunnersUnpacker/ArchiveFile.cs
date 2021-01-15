using DungeonRunnersUnpacker.Models;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DungeonRunnersUnpacker
{
    class ArchiveFile : IDisposable
    {
        public const string ArchiveFileName = "game.pkg";

        public ArchiveHeader Header { get; }
        public Stream Stream { get; }
        public string FilePath { get; }

        public ArchiveFile(string directory)
        {
            FilePath = Path.Combine(directory, ArchiveFileName);

            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"Missing {ArchiveFileName}");

            Stream = File.OpenRead(FilePath);
            using var reader = new BinaryReader(Stream, Encoding.UTF8, true);

            Header = reader.Read<ArchiveHeader>();
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
                using var substream = new SubStream(Stream, entry.CompressedSize);
                using var inflater = new InflaterInputStream(substream);
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
}
