using System.Runtime.InteropServices;
using FutureTech.Protocols;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组入箱.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_2DStation
    {
        public Mst_CmdReply CmdReply;

        [Endian(Endianness.BigEndian)]
        public float x;
        [Endian(Endianness.BigEndian)]
        public float y;
        [Endian(Endianness.BigEndian)]
        public float a;

        public float X => x;
        public float Y => y;
        public float A => a;
        public void SetOff()
        {
            CmdReply.SetOff();
            x = 0;
            y = 0;
            a = 0;
        }
        public void SetOffset(float _x, float _y, float _a)
        {
            x = _x;
            y = _y;
            a = _a;
        }
    }
}
