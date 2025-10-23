using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.人工位.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DevMsg_水冷板检测
    {
        public Dev_CmdTriggerFlag flag;

        [Endian(Endianness.BigEndian)]
        public uint batch;

        public bool Trigger => flag.HasFlag(Dev_CmdTriggerFlag.Trigger1);
        public uint Batch => batch;
    }
}
