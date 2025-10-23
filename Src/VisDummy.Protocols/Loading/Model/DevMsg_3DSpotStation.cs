using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.Loading.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DevMsg_3DSpotStation
    {
        public Dev_CmdTriggerFlag flag;

        [Endian(Endianness.BigEndian)]
        public ushort position;

        public bool Trigger => flag.HasFlag(Dev_CmdTriggerFlag.Trigger1);
        public bool TriggerFinish => flag.HasFlag(Dev_CmdTriggerFlag.Trigger2);
        public ushort Position => position;
    }
}
