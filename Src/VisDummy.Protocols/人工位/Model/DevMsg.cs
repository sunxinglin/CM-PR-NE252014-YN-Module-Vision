using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.人工位.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class DevMsg
    {
        public Dev_CmdHeart Heart;

        public DevMsg_水冷板检测 水冷板检测;
    }
}
