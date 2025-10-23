using FutureTech.Protocols;
using Itminus.Protocols.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VisDummy.Protocols.Common.Model
{
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
	public class Mst_Side
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
		public float D;

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
		public float DPosition => D;

		public void SetReady(bool isok, uint err)
		{
            ErrorCode = (Mst_SideNGFlag)err;
		}

    }
	
	public enum Mst_SideReplyFlag :ushort
	{
		Ready = 1 << 0,
		Done = 1 << 1,
		Response = 1 << 2,
		Response_Ok = 1 << 3,
		Response_Ng = 1 << 4,
	}
	public class MstMsg_SideReplyFlagsBuilder : FlagsBuilder<Mst_SideReplyFlag>
	{
		public MstMsg_SideReplyFlagsBuilder(Mst_SideReplyFlag wCmd) : base(wCmd)
		{
		}

        public MstMsg_SideReplyFlagsBuilder SetOff()
        {
            SetOnOff(Mst_SideReplyFlag.Ready, false);
            SetOnOff(Mst_SideReplyFlag.Done, false);
            SetOnOff(Mst_SideReplyFlag.Response, false);
            SetOnOff(Mst_SideReplyFlag.Response_Ok, false);
            SetOnOff(Mst_SideReplyFlag.Response_Ng, false);
            return this;
        }

        public MstMsg_SideReplyFlagsBuilder SetReady(bool isOk)
		{
			if (isOk)
			{
                SetOnOff(Mst_SideReplyFlag.Response_Ok, true);
                SetOnOff(Mst_SideReplyFlag.Response_Ng, false);
            }
			else
			{
                SetOnOff(Mst_SideReplyFlag.Response_Ok, false);
                SetOnOff(Mst_SideReplyFlag.Response_Ng, true);
            }
            return this;
		}

		public MstMsg_SideReplyFlagsBuilder SetResponseOk(bool isOk)
		{
            SetOnOff(Mst_SideReplyFlag.Ready, true);
            SetOnOff(Mst_SideReplyFlag.Response, true);
			SetOnOff(Mst_SideReplyFlag.Response_Ok, isOk);
			SetOnOff(Mst_SideReplyFlag.Response_Ng, !isOk);
			return this;
		}
		public MstMsg_SideReplyFlagsBuilder SetResponseNg()
		{
            SetOnOff(Mst_SideReplyFlag.Ready, true);
            SetOnOff(Mst_SideReplyFlag.Response, true);
			SetOnOff(Mst_SideReplyFlag.Response_Ok, false);
			SetOnOff(Mst_SideReplyFlag.Response_Ng, true);
			return this;
		}
	}
}
