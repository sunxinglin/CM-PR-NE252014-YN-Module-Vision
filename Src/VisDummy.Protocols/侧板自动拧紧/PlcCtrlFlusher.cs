using Itminus.Protocols;
using Microsoft.Extensions.Options;
using StdUnit.Sharp7.Options;
using VisDummy.Lang.Resources;
using VisDummy.Protocols.侧板自动拧紧.Model;

namespace VisDummy.Protocols.侧板自动拧紧
{
    public class 侧板自动拧紧Flusher : S7PlcFlusher<侧板自动拧紧Scanner, DevMsg, MstMsg>
    {
        public 侧板自动拧紧Flusher(IOptionsMonitor<S7ScanOpt> scanOptsMonitor, 侧板自动拧紧Scanner scanner) : base(scanOptsMonitor, scanner)
        {
        }

        protected override string PlcName => PlcNames.PLCNAME_侧板自动拧紧;

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


