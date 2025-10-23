using FutureTech.Protocols;
using System.Runtime.InteropServices;

namespace VisDummy.Protocols.Common.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Dev_CmdSpot
    {
        public Dev_CmdTriggerFlag flag;

        [Endian(Endianness.BigEndian)]
        public ushort camera;

        [Endian(Endianness.BigEndian)]
        public ushort position;

        public bool Trigger => flag.HasFlag(Dev_CmdTriggerFlag.Trigger1);
        public ushort Camera => camera;
        public ushort Position => position;
    }
}
