using System.ServiceModel;
using System.Windows;
using System.Windows.Controls;
using Catl.MesInvocation.CatlMesInvoker;
using Catl.MesInvocation.CatlMesParams;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using StdUnit.One.Shared;
using VisDummy.Lang.Resources;
using VisDummy.Shared.LogGroup;
using Unit = System.Reactive.Unit;

namespace VisDummy.MtMesp.CatlMes.ViewModel
{
    public class DataCollectForResourceFAIVM : ReactiveObject
    {
        private readonly CatlMesIniConfigHelper _config;
        private readonly IMediator _mediator;

        public DataCollectForResourceFAIVM(CatlMesIniConfigHelper config, IMediator mediator)
        {
            this._config = config;
            this._mediator = mediator;
            this.CmdReload = ReactiveCommand.Create<PasswordBox>(LoadConfigAsync);
            this.CmdSave = ReactiveCommand.Create<PasswordBox>(SaveConfigAsync);
        }
        private void LoadConfigAsync(PasswordBox password)
        {
            var config = this._config.GetDataCollectForResourceFAIConfig();
            try
            {
                if (config == null)
                {
                    MessageBox.Show(Language.Msg_当前INI文件不含有首件的配置段);
                }
                else
                {
                    this.Url = config.ConnectionParams.Url;
                    this.UserName = config.ConnectionParams.UserName;
                    this.Timeout = config.ConnectionParams.Timeout;
                    this.BasicHttpSecurityMode = config.ConnectionParams.BasicHttpSecurityMode;
                    this.Site = config.InterfaceParams.Site;
                    this.User = config.InterfaceParams.User;
                    this.Operation = config.InterfaceParams.Operation;
                    this.OperationRevision = config.InterfaceParams.OperationRevision;
                    this.DcGroup = config.InterfaceParams.DcGroup;
                    this.DcMode = config.InterfaceParams.DcMode;
                    this.Material = config.InterfaceParams.Material;
                    this.MaterialRevision = config.InterfaceParams.MaterialRevision;
                    this.DcGroupRevision = config.InterfaceParams.DcGroupRevision;
                    this.Resource = config.InterfaceParams.Resource;
                    this.DcGroupSequence = config.InterfaceParams.DcGroupSequence;
                    if (password != null)
                        password.Password = config.ConnectionParams.Password;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); ;
            }
        }
        private async void SaveConfigAsync(PasswordBox password)
        {
            try
            {
                var config = new DataCollectForResourceFAIConfig
                {
                    ConnectionParams = new CatlMesConnectionParams
                    {
                        Url = this.Url,
                        UserName = this.UserName,
                        Timeout = this.Timeout,
                        Password = password?.Password ?? "",
                        BasicHttpSecurityMode = this.BasicHttpSecurityMode,
                    },
                    InterfaceParams = new DataCollectForResourceFAIParams
                    {
                        Site = this.Site,
                        User = this.User,
                        Operation = this.Operation,
                        OperationRevision = this.OperationRevision,
                        DcGroup = this.DcGroup,
                        DcGroupRevision = this.DcGroupRevision,
                        Resource = this.Resource,
                        DcMode = this.DcMode,
                        Material = this.Material,
                        MaterialRevision = this.MaterialRevision,
                        DcGroupSequence = this.DcGroupSequence,
                    },
                };
                var read = this._config.GetDataCollectForResourceFAIConfig();
                this._config.SetDataCollectForResourceFAIConfig(config);
                await RecordLogAsync($"{Language.Msg_首件参数变动}：{JsonConvert.SerializeObject(config)}，{Language.Msg_修改前}：{JsonConvert.SerializeObject(read)}");
                MessageBox.Show(Language.Msg_保存成功);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        [Reactive]
        public string Url { get; set; }
        [Reactive]
        public string UserName { get; set; }
        [Reactive]
        public int Timeout { get; set; }
        [Reactive]
        public BasicHttpSecurityMode BasicHttpSecurityMode { get; set; }
        [Reactive]
        public string Site { get; set; }
        [Reactive]
        public string User { get; set; }
        [Reactive]
        public string Operation { get; set; }
        [Reactive]
        public string OperationRevision { get; set; }
        [Reactive]
        public string DcGroup { get; set; }
        [Reactive]
        public string DcGroupRevision { get; set; }
        [Reactive]
        public string DcMode { get; set; }
        [Reactive]
        public string Material { get; set; }
        [Reactive]
        public string MaterialRevision { get; set; }
        [Reactive]
        public string DcGroupSequence { get; set; }
        [Reactive]
        public string Resource { get; set; }
        public ReactiveCommand<PasswordBox, Unit> CmdReload { get; }
        public ReactiveCommand<PasswordBox, Unit> CmdSave { get; }

        public async Task RecordLogAsync(string logmsg)
        {
            logmsg = $"{LogGroupName.OperationLog}：{logmsg}";
            var notification = new UILogNotification(new LogMessage
            {
                EventSource = LogGroupName.OperationLog,
                EventGroup = LogGroupName.OperationLog,
                Level = LogLevel.Critical,
                Content = logmsg,
                Timestamp = DateTime.Now,
            });
            await _mediator.Publish(notification);
        }
    }
}
