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
	public class Dev_Side
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

		public bool Req => this.PhotoREQ.HasFlag(Dev_PhotoREQFlag.REQ);
	}

	[Flags]
    public enum Dev_PhotoREQFlag : ushort
    {
        None = 0,
        REQ = 1 << 0,
    }
}
