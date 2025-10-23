using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.水冷板抓取.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_自动校准
    {
        public Mst_AckFlag ackFlag;

        public Mst_自动校准NGFlag ngFlag;

        public bool Ack => ackFlag.HasFlag(Mst_AckFlag.Ack);
        public bool AckOk => ackFlag.HasFlag(Mst_AckFlag.Ack_Ok);
        public bool AckNg => ackFlag.HasFlag(Mst_AckFlag.Ack_Ng);

        public bool 找特征点NG => ngFlag.HasFlag(Mst_自动校准NGFlag.找特征点NG);
        public bool 像素精度NG => ngFlag.HasFlag(Mst_自动校准NGFlag.像素精度NG);
        public bool 测距NG => ngFlag.HasFlag(Mst_自动校准NGFlag.测距NG);
        public bool 视觉流程NG => ngFlag.HasFlag(Mst_自动校准NGFlag.视觉流程NG);
    }
}
