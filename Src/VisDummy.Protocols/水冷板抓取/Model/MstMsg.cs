using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.水冷板抓取.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg
    {
        public Mst_CmdHeart Heart;

        public MstMsg_定位引导 机器人1定位引导;

        public MstMsg_定位引导 机器人2定位引导;
        
        public MstMsg_校验 机器人1校验;
        
        public MstMsg_校验 机器人2校验;
        
        public MstMsg_自动校准 机器人1自动校准;
        
        public MstMsg_自动校准 机器人2自动校准;

    }
}
