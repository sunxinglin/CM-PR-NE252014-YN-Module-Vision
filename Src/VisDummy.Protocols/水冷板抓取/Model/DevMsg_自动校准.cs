using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.水冷板抓取.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DevMsg_自动校准
    {
        public Dev_CmdTriggerFlag flag;

        [Endian(Endianness.BigEndian)]
        public ushort function;

        [Endian(Endianness.BigEndian)]
        public ushort index;

        [Endian(Endianness.BigEndian)]
        public float x;

        [Endian(Endianness.BigEndian)]
        public float y;

        [Endian(Endianness.BigEndian)]
        public float a;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 96)]
        public byte[] __reserved1;

        public bool Trigger => flag.HasFlag(Dev_CmdTriggerFlag.Trigger1);
        public ushort Function => function;
        public ushort Index => index;
        public float X => x;
        public float Y => y;
        public float A => a;
    }
}
