using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.人工位.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_水冷板检测
    {
        public Mst_AckFlag ackFlag;

        public Mst_NGFlag ngFlag;

        [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 8)]
        public byte[] __reserved1;

        [Endian(Endianness.BigEndian)]
        public float leftShortPosition;

        [Endian(Endianness.BigEndian)]
        public float upperLeftPosition;

        [Endian(Endianness.BigEndian)]
        public float lowerLeftPosition;

        [Endian(Endianness.BigEndian)]
        public float rightShortPosition;

        [Endian(Endianness.BigEndian)]
        public float upperRightPosition;

        [Endian(Endianness.BigEndian)]
        public float lowerRightPosition;


        public bool Ack => ackFlag.HasFlag(Mst_AckFlag.Ack);
        public bool AckOk => ackFlag.HasFlag(Mst_AckFlag.Ack_Ok);
        public bool AckNg => ackFlag.HasFlag(Mst_AckFlag.Ack_Ng);

        public bool 视觉流程NG => ngFlag.HasFlag(Mst_NGFlag.视觉流程NG);
        public bool 偏移量NG => ngFlag.HasFlag(Mst_NGFlag.偏移量NG);
        public bool 找特征点NG => ngFlag.HasFlag(Mst_NGFlag.找特征点NG);
        public bool PLC参数NG => ngFlag.HasFlag(Mst_NGFlag.PLC参数NG);

        public float LeftShortPosition => leftShortPosition;
        public float UpperLeftPosition => upperLeftPosition;
        public float LowerLeftPosition => lowerLeftPosition;
        public float RightShortPosition => rightShortPosition;
        public float UpperRightPosition => upperRightPosition;
        public float LowerRightPosition => lowerRightPosition;
    }
}
