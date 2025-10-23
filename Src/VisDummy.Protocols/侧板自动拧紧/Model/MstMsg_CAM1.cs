using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.侧板自动拧紧.Model
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public class MstMsg_CAM1
	{
		public Mst_Side Flag;

		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 374)]
		public byte[] __reserved;

		public void SetOff()
		{
			Flag.SetReady(false, 0);
		}
	}
}
