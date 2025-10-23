using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组入箱.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg
    {
        public Mst_CmdHeart Heart;

        public MstMsg_2DStation Station2D1;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 40)]
        public byte[] __reserved4;

        public MstMsg_2DSpotStation Station2DSpot;
    }
}
