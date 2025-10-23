using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;


namespace VisDummy.Protocols.Loading.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DevMsg
    {
        public Dev_CmdHeart Heart;

        public DevMsg_3DStation Station3D;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
        public byte[] __reserved2;

        public DevMsg_3DSpotStation Station3DSpot;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
        public byte[] __reserved3;

        public DevMsg_2DStation Station2D;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
        public byte[] __reserved4;

        public DevMsg_2DSpotStation Station2DSpot;

    }
}
