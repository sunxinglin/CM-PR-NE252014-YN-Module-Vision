using Itminus.Protocols.Common;

namespace VisDummy.Protocols.Common.Model
{
    public enum Mst_CmdReplyFlag : ushort
    {
        None = 0,
        Ack = 1 << 0,
        Ack_Ok = 1 << 1,
        Ack_Ng = 1 << 2,
    }
    public class MstMsg_CmdReplyFlagsBuilder : FlagsBuilder<Mst_CmdReplyFlag>
    {
        public MstMsg_CmdReplyFlagsBuilder(Mst_CmdReplyFlag wCmd) : base(wCmd)
        {
        }

        public MstMsg_CmdReplyFlagsBuilder SetOn(bool isOk)
        {
            SetOnOff(Mst_CmdReplyFlag.Ack, true);
            SetOnOff(Mst_CmdReplyFlag.Ack_Ok, isOk);
            SetOnOff(Mst_CmdReplyFlag.Ack_Ng, !isOk);
            return this;
        }
        public MstMsg_CmdReplyFlagsBuilder SetOff()
        {
            SetOnOff(Mst_CmdReplyFlag.Ack, false);
            SetOnOff(Mst_CmdReplyFlag.Ack_Ok, false);
            SetOnOff(Mst_CmdReplyFlag.Ack_Ng, false);
            return this;
        }
    }
}
