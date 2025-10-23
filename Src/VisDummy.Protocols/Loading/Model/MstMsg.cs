using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.Loading.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg
    {
        public Mst_CmdHeart Heart;

        public MstMsg_3DStation Station3D;
        
        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
        public byte[] __reserved2;

        public MstMsg_3DSpotStation Station3DSpot;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
        public byte[] __reserved3;

        public MstMsg_2DStation Station2D;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
        public byte[] __reserved4;

        public MstMsg_2DSpotStation Station2DSpot;
    }
}
