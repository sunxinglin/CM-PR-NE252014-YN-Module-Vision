using Catl.HostComputer.CommonServices;
using Catl.MesInvocation.CatlMesParams;
using Catl.WebServices.MachineIntegrationServiceService;
using Catl.WebServices.MiCheckInventoryAttributesServiceService;
using Catl.WebServices.MiFindCustomAndSfcDataServiceService;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.ServiceModel;

namespace Catl.MesInvocation.CatlMesInvoker
{
    public class CatlMesIniConfigHelper
    {
        private readonly IOptionsMonitor<CatlMesOpt> _catlMesOptsMonitor;

        public CatlMesIniConfigHelper(IOptionsMonitor<CatlMesOpt> catlMesOptsMonitor)
        {
            this._catlMesOptsMonitor = catlMesOptsMonitor;
        }

        #region 接口名称配置
        /// <summary>
        /// 检查电芯属性接口
        /// </summary>
        public string CheckInventoryAttributesInterfaceName => this._catlMesOptsMonitor.CurrentValue.MiCheckInventoryAttributesInterfaceName;
        /// <summary>
        /// 释放模组码接口
        /// </summary>
        public string ReleaseSfcInterfaceName => this._catlMesOptsMonitor.CurrentValue.MiReleaseSfcInterfaceName;
        /// <summary>
        /// 收数接口
        /// </summary>
        public string DataCollectForSfcExInterfaceName => this._catlMesOptsMonitor.CurrentValue.DataCollectForSfcExInterfaceName;
        /// <summary>
        /// 装配模组接口
        /// </summary>
        public string AssembleComponentsForSfcsConfig => this._catlMesOptsMonitor.CurrentValue.MiAssembleComponentsForSfcsInterfaceName;
        /// <summary>
        /// 模组进站接口
        /// </summary>
        public string MiFindCustomAndSfcDataInterfaceName => this._catlMesOptsMonitor.CurrentValue.MiFindCustomAndSfcDataInterfaceName;
        /// <summary>
        /// OCV审查
        /// </summary>
        public string MiCustomDCForCellInterfaceName => this._catlMesOptsMonitor.CurrentValue.MiCustomDCForCellConfigInterfaceName;
        /// <summary>
        /// 首件
        /// </summary>
        public string DataCollectForResourceFAIInterfaceName => this._catlMesOptsMonitor.CurrentValue.DataCollectForResourceFAIInterfaceName;
        /// <summary>
        /// 电芯装配检查
        /// </summary>
        public string MiSFCAttriDataEntryInterfaceName => this._catlMesOptsMonitor.CurrentValue.MiSFCAttriDataEntryInterfaceName;
        #endregion

        #region INI配置读写

        /// <summary>
        /// 获取ini配置路径
        /// </summary>
        /// <returns></returns>
        public string GetInitPath()
        {
            var dir = this._catlMesOptsMonitor.CurrentValue.IniFileDir;
            /// 如果未在配置文件中预设，取当前文件夹
            if (string.IsNullOrEmpty(dir))
            {
                dir = Directory.GetDirectoryRoot(Assembly.GetExecutingAssembly().Location);
            }

            var fname = this._catlMesOptsMonitor.CurrentValue.IniFileName;
            // 如果未在预设，取默认名称
            if (string.IsNullOrEmpty(fname))
            {
                fname = "MESCFG.ini";
            }
            return System.IO.Path.Combine(dir, fname);
        }

        /// <summary>
        /// 获取指定配置段
        /// </summary>
        /// <param name="sectionKey"></param>
        /// <returns></returns>
        public IniSection GetSection(string sectionKey)
        {
            var inifile = new IniFile();

            var inipath = this.GetInitPath();
            if (File.Exists(inipath))
            {
                inifile.Load(inipath);
            }
            var has = inifile.TryGetSection(sectionKey, out var section);
            if (!has)
            {
                section = new IniSection();
            }

            return section;
        }

        /// <summary>
        /// 更新指定配置段
        /// </summary>
        /// <param name="sectionKey"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        public IniSection UpdateSection(string sectionKey, Action<IniSection> update)
        {
            var inipath = this.GetInitPath();

            var inifile = new IniFile();
            if (File.Exists(inipath))
            {
                inifile.Load(inipath);
            }
            var has = inifile.TryGetSection(sectionKey, out var section);
            if (!has)
            {
                section = new IniSection();
            }

            update?.Invoke(section);

            inifile.Remove(sectionKey);
            var s = inifile.Add(sectionKey, section);
            inifile.Save(inipath, FileMode.OpenOrCreate);
            return s;
        }


        /// <summary>
        /// 获取指定配置段中的CATL MES的连接参数
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public CatlMesConnectionParams GetMesConnectionParams(IniSection section)
        {
            var url = section.TryGetValue(nameof(CatlMesConnectionParams.Url), out var urlvalue) ? urlvalue.GetString() : null;
            var username = section.TryGetValue(nameof(CatlMesConnectionParams.UserName), out var usernamevalue) ? usernamevalue.GetString() : null;
            var password = section.TryGetValue(nameof(CatlMesConnectionParams.Password), out var passwdvalue) ? passwdvalue.GetString() : null;
            var timeout = section.TryGetValue(nameof(CatlMesConnectionParams.Timeout), out var timeoutvalue) ? timeoutvalue.ToInt() : 0;
            var basichttpsecuritymode = section.TryGetValue(nameof(CatlMesConnectionParams.BasicHttpSecurityMode), out var basichttpsecuritymodevalue) ? (BasicHttpSecurityMode)basichttpsecuritymodevalue.ToInt() : BasicHttpSecurityMode.None;
            return new CatlMesConnectionParams
            {
                Url = url,
                UserName = username,
                Password = password,
                Timeout = timeout,
                BasicHttpSecurityMode = basichttpsecuritymode,
            };
        }

        #endregion

        #region 电芯检查接口
        public CheckInventoryAttributeConfig GetCheckInventoryAttributeConfig()
        {
            var section = GetSection(this.CheckInventoryAttributesInterfaceName);
            var conn = this.GetMesConnectionParams(section);

            var site = section.TryGetValue(nameof(CheckInventoryParams.Site), out var sitevalue) ? sitevalue.GetString() : null;
            var user = section.TryGetValue(nameof(CheckInventoryParams.User), out var uservalue) ? uservalue.GetString() : null;
            var sfc = section.TryGetValue(nameof(CheckInventoryParams.Sfc), out var sfcvalue) ? sfcvalue.GetString() : null;
            var operation = section.TryGetValue(nameof(CheckInventoryParams.Operation), out var operationvalue) ? operationvalue.GetString() : null;
            var operationRevision = section.TryGetValue(nameof(CheckInventoryParams.OperationRevision), out var operationRevisionValue) ? operationRevisionValue.GetString() : "#";
            var activityId = section.TryGetValue(nameof(CheckInventoryParams.ActivityId), out var activityIdValue) ? activityIdValue.GetString() : null;
            var mode = section.TryGetValue(nameof(CheckInventoryParams.Mode), out var modevalue) ? (modeCheckInventory)modevalue.ToInt() : modeCheckInventory.MODE_NONE;
            var resource = section.TryGetValue(nameof(CheckInventoryParams.Resource), out var resourcevalue) ? resourcevalue.ToString() : null;
            return new CheckInventoryAttributeConfig
            {
                ConnectionParams = conn,
                InterfaceParams = new CheckInventoryParams
                {
                    Site = site,
                    User = user,
                    Sfc = sfc,
                    Operation = operation,
                    OperationRevision = operationRevision,
                    ActivityId = activityId,
                    Mode = mode,
                    Resource = resource,
                },
            };
        }

        public void SetCheckInventoryAttributeConfig(CheckInventoryAttributeConfig config)
        {
            var section = GetSection(this.CheckInventoryAttributesInterfaceName);

            this.UpdateSection(this.CheckInventoryAttributesInterfaceName, section =>
            {
                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.Url));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.Url), config.ConnectionParams.Url);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.Timeout));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.Timeout), config.ConnectionParams.Timeout);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.UserName));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.UserName), config.ConnectionParams.UserName);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.Password));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.Password), config.ConnectionParams.Password);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode), (int)config.ConnectionParams.BasicHttpSecurityMode);

                section.Remove(nameof(CheckInventoryAttributeConfig.InterfaceParams.Site));
                section.Add(nameof(CheckInventoryAttributeConfig.InterfaceParams.Site), config.InterfaceParams.Site);

                section.Remove(nameof(CheckInventoryAttributeConfig.InterfaceParams.User));
                section.Add(nameof(CheckInventoryAttributeConfig.InterfaceParams.User), config.InterfaceParams.User);

                section.Remove(nameof(CheckInventoryAttributeConfig.InterfaceParams.Operation));
                section.Add(nameof(CheckInventoryAttributeConfig.InterfaceParams.Operation), config.InterfaceParams.Operation);

                section.Remove(nameof(CheckInventoryAttributeConfig.InterfaceParams.OperationRevision));
                section.Add(nameof(CheckInventoryAttributeConfig.InterfaceParams.OperationRevision), config.InterfaceParams.OperationRevision);

                section.Remove(nameof(CheckInventoryAttributeConfig.InterfaceParams.ActivityId));
                section.Add(nameof(CheckInventoryAttributeConfig.InterfaceParams.ActivityId), config.InterfaceParams.ActivityId);

                section.Remove(nameof(CheckInventoryAttributeConfig.InterfaceParams.Sfc));
                section.Add(nameof(CheckInventoryAttributeConfig.InterfaceParams.Sfc), config.InterfaceParams.Sfc);

                section.Remove(nameof(CheckInventoryAttributeConfig.InterfaceParams.Mode));
                section.Add(nameof(CheckInventoryAttributeConfig.InterfaceParams.Mode), (int)config.InterfaceParams.Mode);

                section.Remove(nameof(CheckInventoryAttributeConfig.InterfaceParams.Resource));
                section.Add(nameof(CheckInventoryAttributeConfig.InterfaceParams.Resource), config.InterfaceParams.Resource);
            });
        }
        #endregion

        #region 释放模组码
        public ReleaseSfcConfig GetReleaseSfcConfig()
        {
            var section = GetSection(this.ReleaseSfcInterfaceName);
            var conn = this.GetMesConnectionParams(section);

            var site = section.TryGetValue(nameof(ReleaseSfcParams.Site), out var sitevalue) ? sitevalue.GetString() : null;
            var user = section.TryGetValue(nameof(ReleaseSfcParams.User), out var uservalue) ? uservalue.GetString() : null;
            var sfc = section.TryGetValue(nameof(ReleaseSfcParams.SfcQty), out var sfcvalue) ? sfcvalue.ToInt() : 1;
            var operation = section.TryGetValue(nameof(ReleaseSfcParams.Operation), out var operationvalue) ? operationvalue.GetString() : null;
            var operationRevision = section.TryGetValue(nameof(ReleaseSfcParams.OperationRevision), out var operationRevisionValue) ? operationRevisionValue.GetString() : "#";
            var activity = section.TryGetValue(nameof(ReleaseSfcParams.Activity), out var activityIdValue) ? activityIdValue.GetString() : null;
            var mode = section.TryGetValue(nameof(ReleaseSfcParams.Mode), out var modevalue) ? (Catl.WebServices.MiReleaseSfcWithActivityServiceService.modeProcessSFC)modevalue.ToInt() : Catl.WebServices.MiReleaseSfcWithActivityServiceService.modeProcessSFC.MODE_NONE;
            var processlot = section.TryGetValue(nameof(ReleaseSfcParams.Processlot), out var proceslotvalue) ? proceslotvalue.GetString() : null;
            var resource = section.TryGetValue(nameof(ReleaseSfcParams.Resource), out var resourcevalue) ? resourcevalue.GetString() : null;
            return new ReleaseSfcConfig
            {
                ConnectionParams = conn,
                InterfaceParams = new ReleaseSfcParams
                {
                    Site = site,
                    User = user,
                    SfcQty = sfc,
                    Operation = operation,
                    OperationRevision = operationRevision,
                    Activity = activity,
                    Mode = mode,
                    Processlot = processlot,
                    Resource = resource,
                },
            };
        }

        public void SetReleaseSfcConfig(ReleaseSfcConfig config)
        {
            var section = GetSection(this.ReleaseSfcInterfaceName);

            this.UpdateSection(this.ReleaseSfcInterfaceName, section =>
            {
                section.Remove(nameof(ReleaseSfcConfig.ConnectionParams.Url));
                section.Add(nameof(ReleaseSfcConfig.ConnectionParams.Url), config.ConnectionParams.Url);

                section.Remove(nameof(ReleaseSfcConfig.ConnectionParams.Timeout));
                section.Add(nameof(ReleaseSfcConfig.ConnectionParams.Timeout), config.ConnectionParams.Timeout);

                section.Remove(nameof(ReleaseSfcConfig.ConnectionParams.UserName));
                section.Add(nameof(ReleaseSfcConfig.ConnectionParams.UserName), config.ConnectionParams.UserName);

                section.Remove(nameof(ReleaseSfcConfig.ConnectionParams.Password));
                section.Add(nameof(ReleaseSfcConfig.ConnectionParams.Password), config.ConnectionParams.Password);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode), (int)config.ConnectionParams.BasicHttpSecurityMode);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.Site));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.Site), config.InterfaceParams.Site);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.User));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.User), config.InterfaceParams.User);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.Operation));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.Operation), config.InterfaceParams.Operation);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.OperationRevision));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.OperationRevision), config.InterfaceParams.OperationRevision);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.Activity));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.Activity), config.InterfaceParams.Activity);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.SfcQty));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.SfcQty), (int)config.InterfaceParams.SfcQty);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.Mode));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.Mode), (int)config.InterfaceParams.Mode);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.Processlot));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.Processlot), config.InterfaceParams.Processlot);

                section.Remove(nameof(ReleaseSfcConfig.InterfaceParams.Resource));
                section.Add(nameof(ReleaseSfcConfig.InterfaceParams.Resource), config.InterfaceParams.Resource);
            });
        }
        #endregion

        #region 收数出站接口
        public DataCollectForSfcExConfig GetDataCollectForSfcExConfig()
        {
            var section = GetSection(this.DataCollectForSfcExInterfaceName);
            var conn = this.GetMesConnectionParams(section);

            var site = section.TryGetValue(nameof(DataCollectForSfcExParams.Site), out var sitevalue) ? sitevalue.GetString() : null;
            var user = section.TryGetValue(nameof(DataCollectForSfcExParams.User), out var uservalue) ? uservalue.GetString() : null;
            var operation = section.TryGetValue(nameof(DataCollectForSfcExParams.Operation), out var operationvalue) ? operationvalue.GetString() : null;
            var operationRevision = section.TryGetValue(nameof(DataCollectForSfcExParams.OperationRevision), out var operationRevisionValue) ? operationRevisionValue.GetString() : "#";
            var activityId = section.TryGetValue(nameof(DataCollectForSfcExParams.ActivityId), out var activityIdValue) ? activityIdValue.GetString() : "EAP_WS";
            var mode = section.TryGetValue(nameof(DataCollectForSfcExParams.Mode), out var modevalue) ? (ModeProcessSfc)modevalue.ToInt() : ModeProcessSfc.MODE_NONE;
            var dggroup = section.TryGetValue(nameof(DataCollectForSfcExParams.DcGroup), out var dggroupvalue) ? dggroupvalue.GetString() : "*";
            var dggrouprevision = section.TryGetValue(nameof(DataCollectForSfcExParams.DcGroupRevision), out var dggrouprevisionvalue) ? dggrouprevisionvalue.GetString() : "#";
            var resource = section.TryGetValue(nameof(DataCollectForSfcExParams.Resource), out var resourcevalue) ? resourcevalue.GetString() : null;

            return new DataCollectForSfcExConfig
            {
                ConnectionParams = conn,
                InterfaceParams = new DataCollectForSfcExParams
                {
                    Site = site,
                    User = user,
                    Operation = operation,
                    OperationRevision = operationRevision,
                    ActivityId = activityId,
                    Mode = mode,
                    DcGroup = dggroup,
                    DcGroupRevision = dggrouprevision,
                    Resource = resource,
                },
            };
        }



        public void SetDataCollectForSfcExConfig(DataCollectForSfcExConfig config)
        {
            var section = GetSection(this.DataCollectForSfcExInterfaceName);

            this.UpdateSection(this.DataCollectForSfcExInterfaceName, section =>
            {
                section.Remove(nameof(DataCollectForSfcExConfig.ConnectionParams.Url));
                section.Add(nameof(DataCollectForSfcExConfig.ConnectionParams.Url), config.ConnectionParams.Url);

                section.Remove(nameof(DataCollectForSfcExConfig.ConnectionParams.Timeout));
                section.Add(nameof(DataCollectForSfcExConfig.ConnectionParams.Timeout), config.ConnectionParams.Timeout);

                section.Remove(nameof(DataCollectForSfcExConfig.ConnectionParams.UserName));
                section.Add(nameof(DataCollectForSfcExConfig.ConnectionParams.UserName), config.ConnectionParams.UserName);

                section.Remove(nameof(DataCollectForSfcExConfig.ConnectionParams.Password));
                section.Add(nameof(DataCollectForSfcExConfig.ConnectionParams.Password), config.ConnectionParams.Password);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode), (int)config.ConnectionParams.BasicHttpSecurityMode);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.Site));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.Site), config.InterfaceParams.Site);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.User));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.User), config.InterfaceParams.User);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.Operation));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.Operation), config.InterfaceParams.Operation);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.OperationRevision));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.OperationRevision), config.InterfaceParams.OperationRevision);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.ActivityId));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.ActivityId), config.InterfaceParams.ActivityId);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.DcGroup));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.DcGroup), config.InterfaceParams.DcGroup);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.DcGroupRevision));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.DcGroupRevision), config.InterfaceParams.DcGroupRevision);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.Mode));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.Mode), (int)config.InterfaceParams.Mode);

                section.Remove(nameof(DataCollectForSfcExConfig.InterfaceParams.Resource));
                section.Add(nameof(DataCollectForSfcExConfig.InterfaceParams.Resource), config.InterfaceParams.Resource);
            });
        }
        #endregion

        #region 装配模组接口
        public AssembleComponentsToSfcConfig GetAssembleComponentsForSfcsConfig()
        {
            var section = GetSection(this.AssembleComponentsForSfcsConfig);
            var conn = this.GetMesConnectionParams(section);

            var site = section.TryGetValue(nameof(AssembleComponentForSfcParams.Site), out var sitevalue) ? sitevalue.GetString() : null;
            var user = section.TryGetValue(nameof(AssembleComponentForSfcParams.User), out var uservalue) ? uservalue.GetString() : null;
            var operation = section.TryGetValue(nameof(AssembleComponentForSfcParams.Operation), out var operationvalue) ? operationvalue.GetString() : null;
            var operationRevision = section.TryGetValue(nameof(AssembleComponentForSfcParams.OperationRevision), out var operationRevisionValue) ? operationRevisionValue.GetString() : "#";
            var activity = section.TryGetValue(nameof(AssembleComponentForSfcParams.Activity), out var activityValue) ? activityValue.GetString() : "";
            var resource = section.TryGetValue(nameof(AssembleComponentForSfcParams.Resource), out var resourcevalue) ? resourcevalue.GetString() : null;
            return new AssembleComponentsToSfcConfig
            {
                ConnectionParams = conn,
                InterfaceParams = new AssembleComponentForSfcParams
                {
                    Site = site,
                    User = user,
                    Operation = operation,
                    OperationRevision = operationRevision,
                    Activity = activity,
                    Resource = resource,
                },
            };
        }

        public void SetAssembleComponentsForSfcsConfig(AssembleComponentsToSfcConfig config)
        {
            var section = GetSection(this.AssembleComponentsForSfcsConfig);

            this.UpdateSection(this.AssembleComponentsForSfcsConfig, section =>
            {
                section.Remove(nameof(AssembleComponentsToSfcConfig.ConnectionParams.Url));
                section.Add(nameof(AssembleComponentsToSfcConfig.ConnectionParams.Url), config.ConnectionParams.Url);

                section.Remove(nameof(AssembleComponentsToSfcConfig.ConnectionParams.Timeout));
                section.Add(nameof(AssembleComponentsToSfcConfig.ConnectionParams.Timeout), config.ConnectionParams.Timeout);

                section.Remove(nameof(AssembleComponentsToSfcConfig.ConnectionParams.UserName));
                section.Add(nameof(AssembleComponentsToSfcConfig.ConnectionParams.UserName), config.ConnectionParams.UserName);

                section.Remove(nameof(AssembleComponentsToSfcConfig.ConnectionParams.Password));
                section.Add(nameof(AssembleComponentsToSfcConfig.ConnectionParams.Password), config.ConnectionParams.Password);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode), (int)config.ConnectionParams.BasicHttpSecurityMode);

                section.Remove(nameof(AssembleComponentsToSfcConfig.InterfaceParams.Site));
                section.Add(nameof(AssembleComponentsToSfcConfig.InterfaceParams.Site), config.InterfaceParams.Site);

                section.Remove(nameof(AssembleComponentsToSfcConfig.InterfaceParams.User));
                section.Add(nameof(AssembleComponentsToSfcConfig.InterfaceParams.User), config.InterfaceParams.User);

                section.Remove(nameof(AssembleComponentsToSfcConfig.InterfaceParams.Operation));
                section.Add(nameof(AssembleComponentsToSfcConfig.InterfaceParams.Operation), config.InterfaceParams.Operation);

                section.Remove(nameof(AssembleComponentsToSfcConfig.InterfaceParams.OperationRevision));
                section.Add(nameof(AssembleComponentsToSfcConfig.InterfaceParams.OperationRevision), config.InterfaceParams.OperationRevision);

                section.Remove(nameof(AssembleComponentsToSfcConfig.InterfaceParams.Activity));
                section.Add(nameof(AssembleComponentsToSfcConfig.InterfaceParams.Activity), config.InterfaceParams.Activity);

                section.Remove(nameof(AssembleComponentsToSfcConfig.InterfaceParams.Resource));
                section.Add(nameof(AssembleComponentsToSfcConfig.InterfaceParams.Resource), config.InterfaceParams.Resource);
            });
        }

        #endregion

        #region 模组进站
        public MiFindCustomAndSfcDataConfig GetMiFindCustomAndSfcDataConfig()
        {
            var section = GetSection(this.MiFindCustomAndSfcDataInterfaceName);
            var conn = this.GetMesConnectionParams(section);

            var site = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.Site), out var sitevalue) ? sitevalue.GetString() : null;
            var user = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.User), out var uservalue) ? uservalue.GetString() : null;
            var operation = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.Operation), out var operationvalue) ? operationvalue.GetString() : null;
            var operationRevision = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.OperationRevision), out var operationRevisionValue) ? operationRevisionValue.GetString() : "#";
            var activity = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.Activity), out var activityIdValue) ? activityIdValue.GetString() : null;
            var mode = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.Mode), out var modevalue) ? (Catl.WebServices.MiFindCustomAndSfcDataServiceService.modeProcessSFC)modevalue.ToInt() : Catl.WebServices.MiFindCustomAndSfcDataServiceService.modeProcessSFC.MODE_NONE;
            var masterData = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.MasterData), out var masterDatavalue) ? (ObjectAliasEnum)masterDatavalue.ToInt() : ObjectAliasEnum.ITEM;
            var categoryData = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.CategoryData), out var categoryDatavalue) ? (ObjectAliasEnum)categoryDatavalue.ToInt() : ObjectAliasEnum.ITEM;
            var dataField = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.DataField), out var dataFieldvalue) ? dataFieldvalue.GetString() : null;
            var resource = section.TryGetValue(nameof(MiFindCustomAndSfcDataParamers.Resource), out var resourcevalue) ? resourcevalue.GetString() : null;
            return new MiFindCustomAndSfcDataConfig
            {
                ConnectionParams = conn,
                InterfaceParams = new MiFindCustomAndSfcDataParamers
                {
                    Site = site,
                    User = user,
                    Operation = operation,
                    OperationRevision = operationRevision,
                    Activity = activity,
                    Mode = mode,
                    MasterData = masterData,
                    CategoryData = categoryData,
                    DataField = dataField,
                    Resource = resource,
                }
            };
        }

        public void SetMiFindCustomAndSfcDataConfig(MiFindCustomAndSfcDataConfig config)
        {
            var section = GetSection(this.MiFindCustomAndSfcDataInterfaceName);

            this.UpdateSection(this.MiFindCustomAndSfcDataInterfaceName, section =>
            {
                section.Remove(nameof(MiFindCustomAndSfcDataConfig.ConnectionParams.Url));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.ConnectionParams.Url), config.ConnectionParams.Url);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.ConnectionParams.Timeout));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.ConnectionParams.Timeout), config.ConnectionParams.Timeout);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.ConnectionParams.UserName));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.ConnectionParams.UserName), config.ConnectionParams.UserName);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.ConnectionParams.Password));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.ConnectionParams.Password), config.ConnectionParams.Password);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode), (int)config.ConnectionParams.BasicHttpSecurityMode);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Site));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Site), config.InterfaceParams.Site);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.User));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.User), config.InterfaceParams.User);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Operation));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Operation), config.InterfaceParams.Operation);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.OperationRevision));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.OperationRevision), config.InterfaceParams.OperationRevision);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Activity));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Activity), config.InterfaceParams.Activity);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.CategoryData));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.CategoryData), (int)config.InterfaceParams.CategoryData);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.MasterData));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.MasterData), (int)config.InterfaceParams.MasterData);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.DataField));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.DataField), config.InterfaceParams.DataField);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Mode));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Mode), (int)config.InterfaceParams.Mode);

                section.Remove(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Resource));
                section.Add(nameof(MiFindCustomAndSfcDataConfig.InterfaceParams.Resource), config.InterfaceParams.Resource);
            });
        }
        #endregion

        #region Ocv测试
        public MiCustomDCForCellConfig GetMiCustomDCForCellConfig()
        {
            var section = GetSection(this.MiCustomDCForCellInterfaceName);
            var conn = this.GetMesConnectionParams(section);

            var site = section.TryGetValue(nameof(MiCustomDCForCellExParams.Site), out var sitevalue) ? sitevalue.GetString() : null;
            var user = section.TryGetValue(nameof(MiCustomDCForCellExParams.User), out var uservalue) ? uservalue.GetString() : null;
            var operation = section.TryGetValue(nameof(MiCustomDCForCellExParams.Operation), out var operationvalue) ? operationvalue.GetString() : null;
            var dcSequence = section.TryGetValue(nameof(MiCustomDCForCellExParams.DcSequence), out var dcSequenceValue) ? dcSequenceValue.GetString() : null;
            var multispec = section.TryGetValue(nameof(MiCustomDCForCellExParams.Multispec), out var multispecValue) ? multispecValue.GetString() : null;
            var resource = section.TryGetValue(nameof(MiCustomDCForCellExParams.Resource), out var resourceValue) ? resourceValue.GetString() : null;
            var marking = section.TryGetValue(nameof(MiCustomDCForCellExParams.Marking), out var markingValue) ? markingValue.GetString() : null;
            return new MiCustomDCForCellConfig
            {
                ConnectionParams = conn,
                InterfaceParams = new MiCustomDCForCellExParams
                {
                    Site = site,
                    User = user,
                    Operation = operation,
                    DcSequence = dcSequence,
                    Multispec = multispec,
                    Marking = marking,
                    Resource = resource,
                }
            };
        }

        public void SetMiCustomDCForCellConfig(MiCustomDCForCellConfig config)
        {
            var section = GetSection(this.MiCustomDCForCellInterfaceName);

            this.UpdateSection(this.MiCustomDCForCellInterfaceName, section =>
            {
                section.Remove(nameof(MiCustomDCForCellConfig.ConnectionParams.Url));
                section.Add(nameof(MiCustomDCForCellConfig.ConnectionParams.Url), config.ConnectionParams.Url);

                section.Remove(nameof(MiCustomDCForCellConfig.ConnectionParams.Timeout));
                section.Add(nameof(MiCustomDCForCellConfig.ConnectionParams.Timeout), config.ConnectionParams.Timeout);

                section.Remove(nameof(MiCustomDCForCellConfig.ConnectionParams.UserName));
                section.Add(nameof(MiCustomDCForCellConfig.ConnectionParams.UserName), config.ConnectionParams.UserName);

                section.Remove(nameof(MiCustomDCForCellConfig.ConnectionParams.Password));
                section.Add(nameof(MiCustomDCForCellConfig.ConnectionParams.Password), config.ConnectionParams.Password);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode), (int)config.ConnectionParams.BasicHttpSecurityMode);

                section.Remove(nameof(MiCustomDCForCellConfig.InterfaceParams.Site));
                section.Add(nameof(MiCustomDCForCellConfig.InterfaceParams.Site), config.InterfaceParams.Site);

                section.Remove(nameof(MiCustomDCForCellConfig.InterfaceParams.User));
                section.Add(nameof(MiCustomDCForCellConfig.InterfaceParams.User), config.InterfaceParams.User);

                section.Remove(nameof(MiCustomDCForCellConfig.InterfaceParams.Operation));
                section.Add(nameof(MiCustomDCForCellConfig.InterfaceParams.Operation), config.InterfaceParams.Operation);

                section.Remove(nameof(MiCustomDCForCellConfig.InterfaceParams.DcSequence));
                section.Add(nameof(MiCustomDCForCellConfig.InterfaceParams.DcSequence), config.InterfaceParams.DcSequence);

                section.Remove(nameof(MiCustomDCForCellConfig.InterfaceParams.Multispec));
                section.Add(nameof(MiCustomDCForCellConfig.InterfaceParams.Multispec), config.InterfaceParams.Multispec);

                section.Remove(nameof(MiCustomDCForCellConfig.InterfaceParams.Marking));
                section.Add(nameof(MiCustomDCForCellConfig.InterfaceParams.Marking), config.InterfaceParams.Marking);

                section.Remove(nameof(MiCustomDCForCellConfig.InterfaceParams.Resource));
                section.Add(nameof(MiCustomDCForCellConfig.InterfaceParams.Resource), config.InterfaceParams.Resource);
            });
        }
        #endregion

        #region 首件接口
        public DataCollectForResourceFAIConfig GetDataCollectForResourceFAIConfig()
        {
            var section = GetSection(this.DataCollectForResourceFAIInterfaceName);
            var conn = this.GetMesConnectionParams(section);

            var site = section.TryGetValue(nameof(DataCollectForResourceFAIParams.Site), out var sitevalue) ? sitevalue.GetString() : null;
            var user = section.TryGetValue(nameof(DataCollectForResourceFAIParams.User), out var uservalue) ? uservalue.GetString() : null;
            var operation = section.TryGetValue(nameof(DataCollectForResourceFAIParams.Operation), out var operationvalue) ? operationvalue.GetString() : null;
            var operationRevision = section.TryGetValue(nameof(DataCollectForResourceFAIParams.OperationRevision), out var operationRevisionValue) ? operationRevisionValue.GetString() : "#";
            var dggroup = section.TryGetValue(nameof(DataCollectForResourceFAIParams.DcGroup), out var dggroupvalue) ? dggroupvalue.GetString() : "*";
            var dggrouprevision = section.TryGetValue(nameof(DataCollectForResourceFAIParams.DcGroupRevision), out var dggrouprevisionvalue) ? dggrouprevisionvalue.GetString() : "#";
            var resource = section.TryGetValue(nameof(DataCollectForResourceFAIParams.Resource), out var resourcevalue) ? resourcevalue.GetString() : null;
            var dcmode = section.TryGetValue(nameof(DataCollectForResourceFAIParams.DcMode), out var dcmodevalue) ? dcmodevalue.GetString() : null;
            var material = section.TryGetValue(nameof(DataCollectForResourceFAIParams.Material), out var materialvalue) ? materialvalue.GetString() : null;
            var materialRevision = section.TryGetValue(nameof(DataCollectForResourceFAIParams.MaterialRevision), out var materialRevisionvalue) ? materialRevisionvalue.GetString() : null;
            var dcGroupSequence = section.TryGetValue(nameof(DataCollectForResourceFAIParams.DcGroupSequence), out var dcGroupSequencevalue) ? dcGroupSequencevalue.GetString() : null;

            return new DataCollectForResourceFAIConfig
            {
                ConnectionParams = conn,
                InterfaceParams = new DataCollectForResourceFAIParams
                {
                    Site = site,
                    User = user,
                    Operation = operation,
                    OperationRevision = operationRevision,
                    DcGroup = dggroup,
                    DcGroupRevision = dggrouprevision,
                    Resource = resource,
                    DcMode = dcmode,
                    Material = material,
                    MaterialRevision = materialRevision,
                    DcGroupSequence = dcGroupSequence,
                },
            };
        }

        public void SetDataCollectForResourceFAIConfig(DataCollectForResourceFAIConfig config)
        {
            var section = GetSection(this.DataCollectForResourceFAIInterfaceName);

            this.UpdateSection(this.DataCollectForResourceFAIInterfaceName, section =>
            {
                section.Remove(nameof(DataCollectForResourceFAIConfig.ConnectionParams.Url));
                section.Add(nameof(DataCollectForResourceFAIConfig.ConnectionParams.Url), config.ConnectionParams.Url);

                section.Remove(nameof(DataCollectForResourceFAIConfig.ConnectionParams.Timeout));
                section.Add(nameof(DataCollectForResourceFAIConfig.ConnectionParams.Timeout), config.ConnectionParams.Timeout);

                section.Remove(nameof(DataCollectForResourceFAIConfig.ConnectionParams.UserName));
                section.Add(nameof(DataCollectForResourceFAIConfig.ConnectionParams.UserName), config.ConnectionParams.UserName);

                section.Remove(nameof(DataCollectForResourceFAIConfig.ConnectionParams.Password));
                section.Add(nameof(DataCollectForResourceFAIConfig.ConnectionParams.Password), config.ConnectionParams.Password);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode), (int)config.ConnectionParams.BasicHttpSecurityMode);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.Site));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.Site), config.InterfaceParams.Site);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.User));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.User), config.InterfaceParams.User);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.Operation));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.Operation), config.InterfaceParams.Operation);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.OperationRevision));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.OperationRevision), config.InterfaceParams.OperationRevision);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.DcGroup));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.DcGroup), config.InterfaceParams.DcGroup);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.DcGroupRevision));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.DcGroupRevision), config.InterfaceParams.DcGroupRevision);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.Resource));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.Resource), config.InterfaceParams.Resource);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.DcMode));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.DcMode), config.InterfaceParams.DcMode);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.Material));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.Material), config.InterfaceParams.Material);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.MaterialRevision));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.MaterialRevision), config.InterfaceParams.MaterialRevision);

                section.Remove(nameof(DataCollectForResourceFAIConfig.InterfaceParams.DcGroupSequence));
                section.Add(nameof(DataCollectForResourceFAIConfig.InterfaceParams.DcGroupSequence), config.InterfaceParams.DcGroupSequence);
            });
        }
        #endregion

        #region 装配检查
        public MiSFCAttriDataEntryConfig GetMiSFCAttriDataEntryConfig()
        {
            var section = GetSection(this.MiSFCAttriDataEntryInterfaceName);
            var conn = this.GetMesConnectionParams(section);

            var site = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.Site), out var sitevalue) ? sitevalue.GetString() : null;
            var user = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.User), out var uservalue) ? uservalue.GetString() : null;
            var operation = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.Operation), out var operationvalue) ? operationvalue.GetString() : null;
            var operationRevision = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.OperationRevision), out var operationRevisionValue) ? operationRevisionValue.GetString() : "#";
            var dggroup = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.DcGroup), out var dggroupvalue) ? dggroupvalue.GetString() : "*";
            var dggrouprevision = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.DcGroupRevision), out var dggrouprevisionvalue) ? dggrouprevisionvalue.GetString() : "#";
            var sfcMode = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.SfcMode), out var sfcModevalue) ? sfcModevalue.GetString() : null;
            var itemGroup = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.ItemGroup), out var itemGroupvalue) ? itemGroupvalue.GetString() : null;
            var isCheckSequence = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.IsCheckSequence), out var isCheckSequencevalue) ? isCheckSequencevalue.GetString() : null;
            var attributes = section.TryGetValue(nameof(MiSFCAttriDataEntryParams.Attributes), out var attributesvalue) ? attributesvalue.GetString() : null;

            return new MiSFCAttriDataEntryConfig
            {
                ConnectionParams = conn,
                InterfaceParams = new MiSFCAttriDataEntryParams
                {
                    Site = site,
                    User = user,
                    Operation = operation,
                    OperationRevision = operationRevision,
                    DcGroup = dggroup,
                    DcGroupRevision = dggrouprevision,
                    SfcMode = sfcMode,
                    ItemGroup = itemGroup,
                    IsCheckSequence = isCheckSequence,
                    Attributes = attributes
                },
            };
        }

        public void SetMiSFCAttriDataEntryConfig(MiSFCAttriDataEntryConfig config)
        {
            var section = GetSection(this.MiSFCAttriDataEntryInterfaceName);

            this.UpdateSection(this.MiSFCAttriDataEntryInterfaceName, section =>
            {
                section.Remove(nameof(MiSFCAttriDataEntryConfig.ConnectionParams.Url));
                section.Add(nameof(MiSFCAttriDataEntryConfig.ConnectionParams.Url), config.ConnectionParams.Url);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.ConnectionParams.Timeout));
                section.Add(nameof(MiSFCAttriDataEntryConfig.ConnectionParams.Timeout), config.ConnectionParams.Timeout);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.ConnectionParams.UserName));
                section.Add(nameof(MiSFCAttriDataEntryConfig.ConnectionParams.UserName), config.ConnectionParams.UserName);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.ConnectionParams.Password));
                section.Add(nameof(MiSFCAttriDataEntryConfig.ConnectionParams.Password), config.ConnectionParams.Password);

                section.Remove(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode));
                section.Add(nameof(CheckInventoryAttributeConfig.ConnectionParams.BasicHttpSecurityMode), (int)config.ConnectionParams.BasicHttpSecurityMode);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.Site));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.Site), config.InterfaceParams.Site);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.User));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.User), config.InterfaceParams.User);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.Operation));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.Operation), config.InterfaceParams.Operation);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.OperationRevision));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.OperationRevision), config.InterfaceParams.OperationRevision);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.DcGroup));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.DcGroup), config.InterfaceParams.DcGroup);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.DcGroupRevision));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.DcGroupRevision), config.InterfaceParams.DcGroupRevision);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.SfcMode));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.SfcMode), config.InterfaceParams.SfcMode);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.ItemGroup));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.ItemGroup), config.InterfaceParams.ItemGroup);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.IsCheckSequence));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.IsCheckSequence), config.InterfaceParams.IsCheckSequence);

                section.Remove(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.Attributes));
                section.Add(nameof(MiSFCAttriDataEntryConfig.InterfaceParams.Attributes), config.InterfaceParams.Attributes);
            });
        }
        #endregion
    }
}
