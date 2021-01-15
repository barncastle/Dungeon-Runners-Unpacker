using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DungeonRunnersUnpacker.Models
{
    [StructLayout(LayoutKind.Sequential)]
    struct IndexEntry
    {
        public int StringOffset;
        public int Attributes;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int ArchiveOffset;
        public int Unknown4;
        public int DecompressedSize;
        public int Unknown5;
        public int CompressedSize;

        public FileType FileType => (FileType)(Attributes & 0x3F);
        public bool IsCompressed => (Attributes & 0x40) != 0;

        public string GetFileName(IndexFile index)
        {
            var filename = index.FileNames[StringOffset];
            if (FileType < FileType.Max)
                filename += $".{FileType}";

            return filename;
        }
    }
}
