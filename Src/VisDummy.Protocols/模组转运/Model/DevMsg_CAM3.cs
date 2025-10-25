using FutureTech.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VisDummy.Protocols.Common;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组转运.Model
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public class DevMsg_CAM3
	{
		public Dev_PhotoREQFlag PhotoREQ;

		[Endian(Endianness.BigEndian)]
		public ushort FunctionNumber;

		[Endian(Endianness.BigEndian)]
		public ushort PhotoNumben;

		[Endian(Endianness.BigEndian)]
		public float PositionX;

		[Endian(Endianness.BigEndian)]
		public float PositionY;

		[Endian(Endianness.BigEndian)]
		public float PositionZ;

		[Endian(Endianness.BigEndian)]
		public float PositionA;

		public String48 RFID;

		public bool Req => this.PhotoREQ.HasFlag(Dev_PhotoREQFlag.REQ);

		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 38)]
		public byte[] __reserved;
	}
}
