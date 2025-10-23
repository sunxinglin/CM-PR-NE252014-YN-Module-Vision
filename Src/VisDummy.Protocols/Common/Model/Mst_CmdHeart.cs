using Itminus.Protocols.Common;
using System.Runtime.InteropServices;

namespace VisDummy.Protocols.Common.Model
{
    [Flags]
    public enum Mst_CmdFlags : ushort
    {
        HeartBeat = 1 << 0,
    }

    public class MstMsg_CmdFlagsBuilder : FlagsBuilder<Mst_CmdFlags>
    {
        public MstMsg_CmdFlagsBuilder(Mst_CmdFlags wCmd) : base(wCmd)
        {
        }

        public MstMsg_CmdFlagsBuilder SetHeartBeatOnOff(bool onoff)
        {
            SetOnOff(Mst_CmdFlags.HeartBeat, onoff);
            return this;
        }
    }
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Mst_CmdHeart
    {
        public Mst_CmdFlags CmdFlags;
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
        public byte[] __reserved1;
        public bool HasHeartBeat => CmdFlags.HasFlag(Mst_CmdFlags.HeartBeat);
    }
}
