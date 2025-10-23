using Itminus.Protocols;
using Microsoft.Extensions.Options;
using StdUnit.Sharp7.Options;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.人工位.Model;

namespace VisDummy.Protocols.人工位
{
    public class PlcCtrlFlusher : S7PlcFlusher<PlcScanner, DevMsg, MstMsg>
    {
        public PlcCtrlFlusher(IOptionsMonitor<S7ScanOpt> scanOptsMonitor, PlcScanner scanner) : base(scanOptsMonitor, scanner)
        {
        }

        protected override string PlcName => PlcNames.PLCNAME_人工位;

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


