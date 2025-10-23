using System.Runtime.InteropServices;
using FutureTech.Protocols;
using VisDummy.Protocols.Common;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组贴标.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_2DStation
    {
        public Mst_CmdReply CmdReply;

        public SuperBarCode 电芯码;

        [Endian(Endianness.BigEndian)]
        public float X;

        [Endian(Endianness.BigEndian)]
        public float Y;
        
        [Endian(Endianness.BigEndian)]
        public float R;


        public string BarCode1 => 电芯码?.EffectiveContent ?? string.Empty;

        public void SetOff()
        {
            CmdReply.SetOff();
            电芯码 = SuperBarCode.New(string.Empty);
            X = 0;
            Y = 0;
            R = 0;
        }
        public void Set电芯码(string barcode)
        {
            电芯码 = SuperBarCode.New(barcode);
        }
    }
}
