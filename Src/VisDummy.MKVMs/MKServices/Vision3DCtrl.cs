using Itminus.FSharpExtensions;
using MediatR;
using Microsoft.FSharp.Core;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VisDummy.MKVMs.Messages;
using VisDummy.Shared.Opts;

namespace VisDummy.MKVMs.MKServices
{
    public class Vision3DCtrl(Vision3DOpt monitor, IMediator mediator)
    {
        private TcpClient _tcpClient = null;

        public bool Connected => _tcpClient?.Connected ?? false;

        public string ProcName { get; } = monitor.ProcName;

        #region 交互基础方法
        public string Cmd_101(ushort pack) => $"101,{pack},1,0";
        public string Cmd_102(ushort pack) => $"102,{pack}";
        public string Cmd_103(ushort pack, ushort count) => $"103,{pack},{count}";
        public string Cmd_110(ushort pack) => $"110,{pack}";

        public async Task<FSharpResult<string, string>> ReadCmdAsync(string cmd)
        {
            try
            {
                await EnsureConnectAsync();
                await mediator.Publish(new Vision3DNotification { ProcName = ProcName, Message = new Vision3DMessage { Way = Way.Send, Content = cmd } });
                _tcpClient.SendTimeout = 15000;
                _tcpClient.ReceiveTimeout = 5000;
                var stream = _tcpClient.GetStream();
                var sendbyte = Encoding.Default.GetBytes(cmd);
                await stream.WriteAsync(sendbyte, 0, sendbyte.Length);
                byte[] rbuffer = new byte[256];
                var revleng = await stream.ReadAsync(rbuffer, 0, rbuffer.Length);
                byte[] recbuffer = new byte[revleng];
                for (int i = 0; i < revleng; i++)
                {
                    recbuffer[i] = rbuffer[i];
                }
                var str = Encoding.Default.GetString(recbuffer);
                str = str.Replace("\r", "");
                await mediator.Publish(new Vision3DNotification { ProcName = ProcName, Message = new Vision3DMessage { Way = Way.Receive, Content = str } });
                return str.ToOkResult<string, string>();
            }
            catch (Exception ex)
            {
                await DisposeAsync();
                return ex.Message.ToErrResult<string, string>();
            }
        }
        public async Task EnsureConnectAsync()
        {
            try
            {
                var opt = monitor;
                _tcpClient ??= new TcpClient();
                if (!_tcpClient.Connected)
                {
                    await _tcpClient.ConnectAsync(opt.IpAddr, opt.Port);
                    await mediator.Publish(new Vision3DNotification { ProcName = ProcName, Message = new Vision3DMessage { Way = Way.Info, Content = "Connect Success" } });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task DisposeAsync()
        {
            if (_tcpClient != null)
            {
                _tcpClient.Close();
                _tcpClient.Dispose();
                _tcpClient = null;
            }
            return Task.CompletedTask;
        }
        #endregion
    }
}
