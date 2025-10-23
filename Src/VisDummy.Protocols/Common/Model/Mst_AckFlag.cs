using Itminus.Protocols.Common;

namespace VisDummy.Protocols.Common.Model
{
    public enum Mst_AckFlag : byte
    {
        None = 0,
        Ack = 1 << 0,
        Ack_Ok = 1 << 1,
        Ack_Ng = 1 << 2,
    }
    public class Mst_AckFlagsBuilder : FlagsBuilder<Mst_AckFlag>
    {
        public Mst_AckFlagsBuilder(Mst_AckFlag wCmd) : base(wCmd)
        {
        }

        public Mst_AckFlagsBuilder SetOn(bool isOk)
        {
            SetOnOff(Mst_AckFlag.Ack, true);
            SetOnOff(Mst_AckFlag.Ack_Ok, isOk);
            SetOnOff(Mst_AckFlag.Ack_Ng, !isOk);
            return this;
        }
        public Mst_AckFlagsBuilder SetOff()
        {
            SetOnOff(Mst_AckFlag.Ack, false);
            SetOnOff(Mst_AckFlag.Ack_Ok, false);
            SetOnOff(Mst_AckFlag.Ack_Ng, false);
            return this;
        }
    }
}
