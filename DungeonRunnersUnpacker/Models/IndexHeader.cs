using System;
using System.Runtime.InteropServices;

namespace DungeonRunnersUnpacker.Models;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
internal struct IndexHeader
{
    public int Version;
    public Guid ArchiveGuid; // pairs index to pkg
    public int NumInfos;
    public int DataOffset; // ?
    public int StringTableSize;
    public int StringTableOffset;
}
