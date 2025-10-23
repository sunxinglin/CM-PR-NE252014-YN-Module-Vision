using FutureTech.Protocols;
using System.Runtime.InteropServices;

namespace VisDummy.Protocols.Common.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Mst_CmdReply
    {
        public Mst_CmdReplyFlag flag;

        [Endian(Endianness.BigEndian)]
        public uint errorCode;

        public bool Ack => flag.HasFlag(Mst_CmdReplyFlag.Ack);
        public bool AckOk => flag.HasFlag(Mst_CmdReplyFlag.Ack_Ok);
        public bool AckNg => flag.HasFlag(Mst_CmdReplyFlag.Ack_Ng);
        public uint ErrorCode => errorCode;

        public void SetOn(bool isok, uint err)
        {
            flag = new MstMsg_CmdReplyFlagsBuilder(flag).SetOn(isok).Build();
            errorCode = err;
        }
        public void SetOff()
        {
            flag = new MstMsg_CmdReplyFlagsBuilder(flag).SetOff().Build();
            errorCode = 0;
        }
    }
}
