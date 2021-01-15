using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace DungeonRunnersUnpacker
{
    class Program
    {
        static void Main(string[] args)
        {
            var directory = GetDirectory(args);
            var output = Path.Combine(directory, "dump");
            var indexFile = new IndexFile(directory);
            using var archiveFile = new ArchiveFile(directory);

            Directory.CreateDirectory(output);

            foreach (var entry in indexFile.Entries)
            {
                var filename = entry.GetFileName(indexFile);
                Console.WriteLine(filename);

                archiveFile.ExtractFile(entry, Path.Combine(output, filename));
            }
        }

        static string GetDirectory(string[] args)
        {
            return args?.Length == 1 ? args[0] : Directory.GetCurrentDirectory();
        }
    }
}
