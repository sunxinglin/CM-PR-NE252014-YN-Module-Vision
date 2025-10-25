using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组转运.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg
    {
        public Mst_SideHeart Heart;

        public MstMsg_CAM1 CAM1;

		public MstMsg_CAM2 CAM2;

        public MstMsg_CAM3 CAM3;
    }
}
