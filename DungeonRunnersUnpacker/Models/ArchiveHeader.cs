using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DungeonRunnersUnpacker.Models
{
    [StructLayout(LayoutKind.Sequential)]
    struct ArchiveHeader
    {
        public int Unknown1;
        public int Unknown2;
        public int Unknown3;
        public int Unknown4;
    }
}
