using FutureTech.Protocols;
using System.Runtime.InteropServices;

namespace VisDummy.Protocols.Common.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Dev_CmdTrigger
    {
        public Dev_CmdTriggerFlag flag;

        [Endian(Endianness.BigEndian)]
        public ushort function;

        [Endian(Endianness.BigEndian)]
        public ushort position;

        [Endian(Endianness.BigEndian)]
        public uint batch;

        public bool Trigger => flag.HasFlag(Dev_CmdTriggerFlag.Trigger1);
        public ushort Function => function;
        public ushort Position => position;
        public uint Batch => batch;
    }
}
