using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.水冷板抓取.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_校验
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

        [Endian(Endianness.BigEndian)]
        public float 像素精度;

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
    }
}
