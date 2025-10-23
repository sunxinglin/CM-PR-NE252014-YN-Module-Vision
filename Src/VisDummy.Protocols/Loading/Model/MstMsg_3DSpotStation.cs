using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.Loading.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_3DSpotStation
    {
        public Mst_CmdReplyFlag flag;
        public bool Ack => flag.HasFlag(Mst_CmdReplyFlag.Ack);
        public bool AckOk => flag.HasFlag(Mst_CmdReplyFlag.Ack_Ok);
        public bool AckNg => flag.HasFlag(Mst_CmdReplyFlag.Ack_Ng);

        public void SetOn(bool isok)
        {
            flag = new MstMsg_CmdReplyFlagsBuilder(flag).SetOn(isok).Build();
        }
        public void SetOff()
        {
            flag = new MstMsg_CmdReplyFlagsBuilder(flag).SetOff().Build();
        }
    }
}
