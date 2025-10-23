using System.Runtime.InteropServices;

namespace VisDummy.Protocols.Common.Model
{
    [Flags]
    public enum Dev_CmdFlags : ushort
    {
        HeartBeat = 1 << 0,

        Maintain = 1 << 8,
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Dev_CmdHeart
    {
        public Dev_CmdFlags CmdFlags;
        
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
        public byte[] __reserved1;
        
        public bool HasHeartBeat => CmdFlags.HasFlag(Dev_CmdFlags.HeartBeat);
        
        public bool IsMaintaining => CmdFlags.HasFlag(Dev_CmdFlags.Maintain);
    }
}
