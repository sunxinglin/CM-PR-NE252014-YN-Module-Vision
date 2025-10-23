using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.侧板自动拧紧.Model
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public class MstMsg_CAM3
	{
		public Mst_Side Flag;

		public void SetOff()
		{
			Flag.SetReady(false, 0);
		}
	}
}
