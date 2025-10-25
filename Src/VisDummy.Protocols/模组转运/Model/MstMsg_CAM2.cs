using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组转运.Model
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public class MstMsg_CAM2
	{
		public Mst_SideReplyFlag Flag;

		/// <summary>
		/// 错误代码（1,特征点NG   2,视觉检测流程NG   3,其它NG   4,PLC参数NG）
		/// </summary>
		[Endian(Endianness.BigEndian)]
		public Mst_SideNGFlag ErrorCode;

		[Endian(Endianness.BigEndian)]
		public float X;

		[Endian(Endianness.BigEndian)]
		public float Y;

		[Endian(Endianness.BigEndian)]
		public float Z;

		[Endian(Endianness.BigEndian)]
		public float A;

		/// <summary>
		/// 1 ok  2 ng
		/// </summary>
		[Endian(Endianness.BigEndian)]
		public ushort Status;

		[Endian(Endianness.BigEndian)]
		public float X1;

		[Endian(Endianness.BigEndian)]
		public float Y1;

		[Endian(Endianness.BigEndian)]
		public float A1;


		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 374)]
		public byte[] __reserved;


		public bool Ready => Flag.HasFlag(Mst_SideReplyFlag.Ready);
		public Mst_SideNGFlag errorCode => ErrorCode;
		public bool Response => Flag.HasFlag(Mst_SideReplyFlag.Response);
		public bool Response_Ok => Flag.HasFlag(Mst_SideReplyFlag.Response_Ok);
		public bool Response_Ng => Flag.HasFlag(Mst_SideReplyFlag.Response_Ng);


		public bool 特征点NG => ErrorCode.HasFlag(Mst_SideNGFlag.特征点NG);
		public bool 视觉检测流程NG => ErrorCode.HasFlag(Mst_SideNGFlag.视觉检测流程NG);
		public bool 其它NG => ErrorCode.HasFlag(Mst_SideNGFlag.其它NG);
		public bool PLC参数NG => ErrorCode.HasFlag(Mst_SideNGFlag.PLC参数NG);

		public float XPosition => X;
		public float YPosition => Y;
		public float ZPosition => Z;
		public float APosition => A;
		public float X1Position => X1;
		public float YX1Position => Y1;
		public float A1Position => A1;
		public void SetReady(bool isok, uint err)
		{
			ErrorCode = (Mst_SideNGFlag)err;
		}
	}
}
