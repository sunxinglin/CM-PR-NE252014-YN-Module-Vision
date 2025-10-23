using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisDummy.Protocols.Common.Model
{
	[Flags]
	public enum Dev_SideReqFlags : ushort
	{
		None = 0,
		Req = 1 << 0,
	}
}
