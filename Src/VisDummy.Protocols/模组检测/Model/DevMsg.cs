using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组检测.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DevMsg
    {
        public Dev_CmdHeart Heart;

        public DevMsg_2DStation Station2D;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 38)]
        public byte[] __reserved4;

        public DevMsg_2DSpotStation Station2DSpot;
    }
}
