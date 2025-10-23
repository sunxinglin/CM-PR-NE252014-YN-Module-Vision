using FutureTech.Protocols;
using System.Runtime.InteropServices;

namespace VisDummy.Protocols.Common.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class Mst_CmdSpot
    {
        public Mst_CmdReplyFlag flag;

        [Endian(Endianness.BigEndian)]
        public float errorCode;
        [Endian(Endianness.BigEndian)]
        public float features;
        [Endian(Endianness.BigEndian)]
        public float pixels;

        public bool Ack => flag.HasFlag(Mst_CmdReplyFlag.Ack);
        public bool AckOk => flag.HasFlag(Mst_CmdReplyFlag.Ack_Ok);
        public bool AckNg => flag.HasFlag(Mst_CmdReplyFlag.Ack_Ng);
        public float ErrorCode => errorCode;
        public float Features => features;
        public float Pixels => pixels;

        public void SetOn(bool isok, uint err, float feat, float pix)
        {
            flag = new MstMsg_CmdReplyFlagsBuilder(flag).SetOn(isok).Build();
            errorCode = err;
            features = feat;
            pixels = pix;
        }
        public void SetOff()
        {
            flag = new MstMsg_CmdReplyFlagsBuilder(flag).SetOff().Build();
            errorCode = 0;
            features = 0;
            pixels = 0;
        }
    }
}
