using System;
using System.IO;

namespace DungeonRunnersUnpacker;

internal static class Program
{
    private static void Main(string[] args)
    {
        var directory = GetDirectory(args);
        var output = Path.Combine(directory, "dump");
        var indexFile = new IndexFile(directory);
        using ArchiveFile archiveFile = new(directory);

        if (indexFile.Header.ArchiveGuid != archiveFile.Guid)
            throw new Exception("Archive and Index hashes do not match");

        Directory.CreateDirectory(output);

        foreach (var entry in indexFile.Entries)
        {
            var filename = entry.GetFileName(indexFile);
            Console.WriteLine(filename);

            archiveFile.ExtractFile(entry, Path.Combine(output, filename));
        }
    }

    private static string GetDirectory(string[] args)
    {
        return args?.Length == 1 ? args[0] : Directory.GetCurrentDirectory();
    }
}
