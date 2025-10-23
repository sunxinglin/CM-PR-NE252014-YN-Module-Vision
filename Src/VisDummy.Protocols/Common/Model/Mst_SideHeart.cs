using FutureTech.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VisDummy.Protocols.Common.Model
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public class Mst_SideHeart {

		public Mst_CmdFlags CmdFlags;

		[Endian(Endianness.BigEndian)]
		public ushort SoftwareCodeError;

		[Endian(Endianness.BigEndian)]
		public ushort ChangeMode;
		public bool InProcess;
		[Endian(Endianness.BigEndian)]
		public ushort ChangeType;

		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 1286)]
		public byte[] __reserved1;

		public bool HasHeartBeat => CmdFlags.HasFlag(Mst_CmdFlags.HeartBeat);
	}
}
