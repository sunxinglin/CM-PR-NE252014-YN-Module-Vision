using Itminus.Protocols;
using Microsoft.Extensions.Options;
using StdUnit.Sharp7.Options;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.模组转运.Model;

namespace VisDummy.Protocols.模组转运
{
    public class 模组转运Flusher : S7PlcFlusher<模组转运Scanner, DevMsg, MstMsg>
    {
        public 模组转运Flusher(IOptionsMonitor<S7ScanOpt> scanOptsMonitor, 模组转运Scanner scanner) : base(scanOptsMonitor, scanner)
        {
        }

        protected override string PlcName => PlcNames.PLCNAME_模组转运;

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


