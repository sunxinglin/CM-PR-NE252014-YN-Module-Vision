using Itminus.Protocols;
using Microsoft.Extensions.Options;
using StdUnit.Sharp7.Options;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.垫片检测.Model;

namespace VisDummy.Protocols.垫片检测
{
    public class 垫片检测Flusher : S7PlcFlusher<垫片检测Scanner, DevMsg, MstMsg>
    {
        public 垫片检测Flusher(IOptionsMonitor<S7ScanOpt> scanOptsMonitor, 垫片检测Scanner scanner) : base(scanOptsMonitor, scanner)
        {
        }

        protected override string PlcName => PlcNames.PLCNAME_垫片检测;

        public override async Task FlushAsync(MstMsg mstmsg)
        {
            var s7ScanOpt = _scanOptsMonitor.Get(_scanner.ScanName);
            var write = await _scanner.PlcCtrl.WriteDBAsync(s7ScanOpt.MstMsg_DB_INDEX, s7ScanOpt.MstMsg_DB_OFFSET, mstmsg);
            if (write.IsError)
            {
                throw new Exception($"【{PlcName}】{Language.Msg_向PLC写数据错误}：{write.ErrorValue}");
            }
        }
    }

}


