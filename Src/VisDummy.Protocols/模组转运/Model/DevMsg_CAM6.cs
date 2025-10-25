using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组转运.Model
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public class DevMsg_CAM6
	{
		public Dev_Side Flag;

		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 58)]
		public byte[] __reserved;
	}
}
