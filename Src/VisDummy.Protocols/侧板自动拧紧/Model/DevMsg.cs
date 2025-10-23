using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.侧板自动拧紧.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DevMsg
    {
        public Dev_SideHeart Heart;

		public DevMsg_CAM1 CAM1;

		public DevMsg_CAM2 CAM2;

		public DevMsg_CAM3 CAM3;
	}
}
