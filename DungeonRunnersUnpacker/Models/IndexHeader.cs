using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DungeonRunnersUnpacker.Models
{
    [StructLayout(LayoutKind.Sequential)]
    struct IndexHeader
    {
        public int Version;
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
        public int NumInfos;
        public int Unknown5;
        public int StringTableSize;
        public int StringTableOffset;
    }
}
