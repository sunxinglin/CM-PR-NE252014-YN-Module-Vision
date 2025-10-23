using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisDummy.Abstractions
{
    public class WarpHelper
    {
        public static string ResultConvert(ushort resultStatus)
        {
            return resultStatus switch
            {
                2 => $"[{resultStatus}]正常-视觉发送电芯抓取点",
                1 => $"[{resultStatus}]异常-电芯尺寸异常（电芯侧躺）",
                6 => $"[{resultStatus}]正常-空泡棉",
                8 => $"[{resultStatus}]异常-抓取列电芯数量异常",
                9 => $"[{resultStatus}]异常-Z向角度异常或者来料位置超限",
                10 => $"[{resultStatus}]异常-抓取列电芯高度异常",
                14 => $"[{resultStatus}]异常-点云异常（来料超出ROI/异物遮挡/纸壳粘连）",
                _ => $"[{resultStatus}]--"
            };
        }
    }
}
