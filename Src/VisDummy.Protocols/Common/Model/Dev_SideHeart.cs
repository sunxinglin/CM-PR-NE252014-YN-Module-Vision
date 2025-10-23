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
	public class Dev_SideHeart
	{
		public Dev_ChangeModeFlag ChangeMode;

		[Endian(Endianness.BigEndian)]
		public ushort WorkMode;

		public Dev_ProcessEnable ProcessEnable;

		public String24 RFID_Batch;

		public String48 ModelCode1;

		public String48 ModelCode2;

		public Dev_CmdFlags CmdFlags;

		[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 66)]
		public byte[] __reserved1;

		public bool HasHeartBeat => CmdFlags.HasFlag(Dev_CmdFlags.HeartBeat);

		public bool IsMaintaining => CmdFlags.HasFlag(Dev_CmdFlags.Maintain);
	}

    [Flags]
    public enum Dev_ChangeModeFlag : ushort
    {
        None = 0,
        REQ = 1 << 0,
    }

    [Flags]
    public enum Dev_ProcessEnable : ushort
    {
        ProcessEnable = 0,
        ChangeType = 1 << 0,
    }
}
