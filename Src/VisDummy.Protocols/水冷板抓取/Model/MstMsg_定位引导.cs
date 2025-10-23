using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.水冷板抓取.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_定位引导
    {
        public Mst_AckFlag ackFlag;

        public Mst_NGFlag ngFlag;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
        public byte[] __reserved1;

        [Endian(Endianness.BigEndian)]
        public float x;

        [Endian(Endianness.BigEndian)]
        public float y;

        [Endian(Endianness.BigEndian)]
        public float a;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 100)]
        public byte[] __reserved2;


        public bool Ack => ackFlag.HasFlag(Mst_AckFlag.Ack);
        public bool AckOk => ackFlag.HasFlag(Mst_AckFlag.Ack_Ok);
        public bool AckNg => ackFlag.HasFlag(Mst_AckFlag.Ack_Ng);

        public bool 视觉流程NG => ngFlag.HasFlag(Mst_NGFlag.视觉流程NG);
        public bool 偏移量NG => ngFlag.HasFlag(Mst_NGFlag.偏移量NG);
        public bool 找特征点NG => ngFlag.HasFlag(Mst_NGFlag.找特征点NG);
        public bool PLC参数NG => ngFlag.HasFlag(Mst_NGFlag.PLC参数NG);

        public float X => x;
        public float Y => y;
        public float A => a;
        //public string BarCode1 => 电芯码?.EffectiveContent ?? string.Empty;

        //public void SetOff()
        //{
        //    CmdReply.SetOff();
        //    电芯码 = SuperBarCode.New(string.Empty);
        //    X = 0;
        //    Y = 0;
        //    R = 0;
        //}
        //public void Set电芯码(string barcode)
        //{
        //    电芯码 = SuperBarCode.New(barcode);
        //}
    }
}
