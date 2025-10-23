using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.Loading.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_2DStation
    {
        public Mst_CmdReply CmdReply;

        public BarCode BarCode;

        [Endian(Endianness.BigEndian)]
        public ushort direction;

        public ushort Direction => direction;

        public void SetOff()
        {
            CmdReply.SetOff();
            direction = 0;
            BarCode = BarCode.New(string.Empty);
        }
    }
}
