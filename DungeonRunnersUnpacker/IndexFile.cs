﻿using DungeonRunnersUnpacker.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DungeonRunnersUnpacker
{
    class IndexFile
    {
        public const string IndexFileName = "game.pki";

        public IndexHeader Header { get; }
        public IndexEntry[] Entries { get; }
        public IReadOnlyDictionary<int, string> FileNames { get; }
        public string FilePath { get; }

        public IndexFile(string directory)
        {
            FilePath = Path.Combine(directory, IndexFileName);

            if (!File.Exists(FilePath))
                throw new FileNotFoundException($"Missing {IndexFileName}");

            using var fileStream = File.OpenRead(FilePath);
            using var reader = new BinaryReader(fileStream);

            Header = reader.Read<IndexHeader>();
            Entries = reader.Read<IndexEntry>(Header.NumInfos);
            FileNames = ReadStringTable(reader);

            // sort for faster extraction
            Array.Sort(Entries, (x, y) => x.ArchiveOffset.CompareTo(y.ArchiveOffset));
        }

        private IReadOnlyDictionary<int, string> ReadStringTable(BinaryReader br)
        {
            var results = new Dictionary<int, string>(Header.NumInfos);

            var offset = 0;
            while (offset != Header.StringTableSize)
            {
                results[offset] = br.ReadCString();
                offset += results[offset].Length + 1;
            }

            return results;
        }
    }
}
