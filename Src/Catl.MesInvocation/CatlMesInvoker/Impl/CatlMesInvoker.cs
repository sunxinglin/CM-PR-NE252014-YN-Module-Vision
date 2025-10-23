using Catl.HostComputer.CommonServices.Mes;
using Catl.WebServices.DataCollectForResourceFAI;
using Catl.WebServices.MachineIntegrationServiceService;
using Catl.WebServices.MiAssembleComponentsToSfcsServiceService;
using Catl.WebServices.MiCheckInventoryAttributesServiceService;
using Catl.WebServices.MiCustomDCForCellServiceService;
using Catl.WebServices.MiFindCustomAndSfcDataServiceService;
using Catl.WebServices.MiReleaseSfcWithActivityServiceService;
using Microsoft.Extensions.Logging;
using MiSFCAttriDataEntryServiceService;
using Newtonsoft.Json;
using System.ServiceModel;

namespace Catl.MesInvocation.CatlMesInvoker
{
    public class CatlMesInvoker(CatlMesIniConfigHelper iniHelper, ISfcInvocationLogger sfclogger, ILogger<CatlMesInvoker> logger) : ICatlMesInvoker
    {
        private async Task WriteExcelAsync(SfcInvocationLogging logging, string svcname, string svcdesc, string equipId)
        {
            try
            {
                var path = sfclogger.GetLogPath(logging.SentAt, svcname, svcdesc, equipId);
                await sfclogger.WriteLogAsync(logging, path);
            }
            catch (Exception ex)
            {
                logger.LogError($"写MES调用日志（Excel）出错：{ex.Message}\r\n{ex.StackTrace}\r\n消息内容={JsonConvert.SerializeObject(logging)}");
            }
        }
        public async Task<checkInventoryAttributesResponse> CheckInventoryAttributesAsync(string[] inventoryArray)
        {
            var config = iniHelper.GetCheckInventoryAttributeConfig();
            var resource = config.InterfaceParams.Resource;
            var sfc = config.InterfaceParams.Sfc;
            var checkreq = new ModuleCellMarkingOrTimeCheckRequest
            {
                operation = config.InterfaceParams.Operation,
                operationRevision = config.InterfaceParams.OperationRevision,
                sfc = sfc,
                site = config.InterfaceParams.Site,
                activityId = config.InterfaceParams.ActivityId,
                resource = resource,
                user = config.InterfaceParams.User,
                modeCheckInventory = config.InterfaceParams.Mode,
                inventoryArray = inventoryArray,
                requiredQuantity = inventoryArray.Length,
            };

            var binding = new BasicHttpBinding();
            binding.Security.Mode = config.ConnectionParams.BasicHttpSecurityMode;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.SendTimeout = TimeSpan.FromMilliseconds(config.ConnectionParams.Timeout);
            if (string.IsNullOrEmpty(config.ConnectionParams.Url))
            {
                throw new Exception($"The URL cannot be empty when calling CATL WebService, check the MESCFG.ini configuration file!");
            }
            var address = new EndpointAddress(config.ConnectionParams.Url);

            using (var scf = new ChannelFactory<MiCheckInventoryAttributesService>(binding, address))
            {
                scf.Credentials.UserName.UserName = config.ConnectionParams.UserName;
                scf.Credentials.UserName.Password = config.ConnectionParams.Password;
                var behavior = new UsePreAuthenticateHttpClientEndpointBehavior(scf.Credentials.UserName.UserName, scf.Credentials.UserName.Password);
                scf.Endpoint.EndpointBehaviors.Add(behavior);
                MiCheckInventoryAttributesService channel = scf.CreateChannel();
                var req = new miCheckInventoryAttributesRequest(new miCheckInventoryAttributes
                {
                    CheckInventoryAttributesRequest = checkreq,
                });
                var sentAt = DateTime.Now;
                try
                {
                    var resp = await channel.miCheckInventoryAttributesAsync(req);
                    var result = resp.miCheckInventoryAttributesResponse.@return;
                    var receivedAt = DateTime.Now;

                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = sfc,
                        Payload = JsonConvert.SerializeObject(checkreq),
                        ReceivedAt = receivedAt,
                        RespCode = result.code.ToString(),
                        RespInfo = JsonConvert.SerializeObject(result),
                    };
                    var svcname = iniHelper.CheckInventoryAttributesInterfaceName;
                    var svcdesc = "";
                    var equipId = resource;
                    await this.WriteExcelAsync(logging, svcname, svcdesc, equipId);

                    return result;
                }
                catch (Exception ex)
                {
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = sfc,
                        Payload = JsonConvert.SerializeObject(checkreq),
                        ReceivedAt = receivedAt,
                        RespCode = String.Empty,
                        RespInfo = JsonConvert.SerializeObject(ex.Message),
                    };
                    var svcname = iniHelper.CheckInventoryAttributesInterfaceName;
                    var svcdesc = "";
                    var equipId = resource;
                    await this.WriteExcelAsync(logging, svcname, svcdesc, equipId);
                    throw;
                }
            }
        }
        public async Task<releaseSfcWithActivityResponse> ReleaseSfcByShoporderAsync(string shoporder)
        {
            var config = iniHelper.GetReleaseSfcConfig();
            var resource = config.InterfaceParams.Resource;
            var releaseReq = new releaseSfcWithActivityRequest
            {
                operation = config.InterfaceParams.Operation,
                operationRevision = config.InterfaceParams.OperationRevision,
                site = config.InterfaceParams.Site,
                activity = config.InterfaceParams.Activity,
                resource = resource,
                user = config.InterfaceParams.User,
                modeProcessSFC = config.InterfaceParams.Mode,
                sfcQty = config.InterfaceParams.SfcQty,
                processlot = config.InterfaceParams.Processlot,
                shopOrder = shoporder,
            };

            var binding = new BasicHttpBinding();
            binding.Security.Mode = config.ConnectionParams.BasicHttpSecurityMode;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.SendTimeout = TimeSpan.FromMilliseconds(config.ConnectionParams.Timeout);
            if (string.IsNullOrEmpty(config.ConnectionParams.Url))
            {
                throw new Exception($"The URL cannot be empty when calling CATL WebService, check the MESCFG.ini configuration file!");
            }
            var address = new EndpointAddress(config.ConnectionParams.Url);

            using (var scf = new ChannelFactory<MiReleaseSfcWithActivityService>(binding, address))
            {
                scf.Credentials.UserName.UserName = config.ConnectionParams.UserName;
                scf.Credentials.UserName.Password = config.ConnectionParams.Password;
                var behavior = new UsePreAuthenticateHttpClientEndpointBehavior(scf.Credentials.UserName.UserName, scf.Credentials.UserName.Password);
                scf.Endpoint.EndpointBehaviors.Add(behavior);
                var channel = scf.CreateChannel();
                var req = new miReleaseSfcWithActivityByShoporderRequest(new miReleaseSfcWithActivityByShoporder
                {
                    ReleaseSfcWithActivityRequest = releaseReq
                });
                var sentAt = DateTime.Now;
                try
                {

                    var resp = await channel.miReleaseSfcWithActivityByShoporderAsync(req);
                    var result = resp.miReleaseSfcWithActivityByShoporderResponse.@return;
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = config.InterfaceParams.SfcQty.ToString(),
                        Payload = JsonConvert.SerializeObject(releaseReq),
                        ReceivedAt = receivedAt,
                        RespCode = result.code.ToString(),
                        RespInfo = JsonConvert.SerializeObject(result),
                    };
                    var svcname = iniHelper.ReleaseSfcInterfaceName;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    return result;
                }
                catch (Exception ex)
                {
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = config.InterfaceParams.SfcQty.ToString(),
                        Payload = JsonConvert.SerializeObject(releaseReq),
                        ReceivedAt = receivedAt,
                        RespCode = String.Empty,
                        RespInfo = JsonConvert.SerializeObject(ex.Message),
                    };
                    var svcname = iniHelper.ReleaseSfcInterfaceName;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    throw;
                }
            }
        }
        public async Task<sfcDcExResponse> DataCollectForSfcExAsync(string sfc, Action<IList<Catl.WebServices.MachineIntegrationServiceService.machineIntegrationParametricData>> updateParams)
        {
            var config = iniHelper.GetDataCollectForSfcExConfig();
            var resource = config.InterfaceParams.Resource;
            var parameters = new List<Catl.WebServices.MachineIntegrationServiceService.machineIntegrationParametricData>();
            updateParams(parameters);
            var payload = new sfcDcExRequest
            {
                operation = config.InterfaceParams.Operation,
                operationRevision = config.InterfaceParams.OperationRevision,
                site = config.InterfaceParams.Site,
                activityId = config.InterfaceParams.ActivityId,
                resource = resource,
                user = config.InterfaceParams.User,
                modeProcessSfc = config.InterfaceParams.Mode,
                dcGroup = config.InterfaceParams.DcGroup,
                dcGroupRevision = config.InterfaceParams.DcGroupRevision,
                sfc = sfc,
                parametricDataArray = parameters.ToArray(),
            };

            var binding = new BasicHttpBinding();
            binding.Security.Mode = config.ConnectionParams.BasicHttpSecurityMode;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.SendTimeout = TimeSpan.FromMilliseconds(config.ConnectionParams.Timeout);
            if (string.IsNullOrEmpty(config.ConnectionParams.Url))
            {
                throw new Exception($"The URL cannot be empty when calling CATL WebService, check the MESCFG.ini configuration file!");
            }
            var address = new EndpointAddress(config.ConnectionParams.Url);

            using (var scf = new ChannelFactory<MachineIntegrationService>(binding, address))
            {
                scf.Credentials.UserName.UserName = config.ConnectionParams.UserName;
                scf.Credentials.UserName.Password = config.ConnectionParams.Password;
                var behavior = new UsePreAuthenticateHttpClientEndpointBehavior(scf.Credentials.UserName.UserName, scf.Credentials.UserName.Password);
                scf.Endpoint.EndpointBehaviors.Add(behavior);
                var channel = scf.CreateChannel();
                var req = new dataCollectForSfcExRequest(new dataCollectForSfcEx { SfcDcExRequest = payload });
                var sentAt = DateTime.Now;
                try
                {
                    var resp = await channel.dataCollectForSfcExAsync(req);
                    var result = resp.dataCollectForSfcExResponse.@return;
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = sfc,
                        Payload = JsonConvert.SerializeObject(payload),
                        ReceivedAt = receivedAt,
                        RespCode = result.code.ToString(),
                        RespInfo = JsonConvert.SerializeObject(result),
                    };
                    var svcname = iniHelper.DataCollectForSfcExInterfaceName;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    return result;
                }
                catch (Exception ex)
                {
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = sfc,
                        Payload = JsonConvert.SerializeObject(payload),
                        ReceivedAt = receivedAt,
                        RespCode = String.Empty,
                        RespInfo = JsonConvert.SerializeObject(ex.Message),
                    };
                    var svcname = iniHelper.DataCollectForSfcExInterfaceName;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    throw;
                }
            }

        }
        public async Task<assembleComponentsToSfcsResponse> AssembleComponentsToSfcsAsync(string sfc, inventoryData[] inventoryArray)
        {
            var config = iniHelper.GetAssembleComponentsForSfcsConfig();
            var resource = config.InterfaceParams.Resource;
            var assemblereq = new assembleComponentsToSfcsRequest
            {
                operation = config.InterfaceParams.Operation,
                operationRev = config.InterfaceParams.OperationRevision,
                site = config.InterfaceParams.Site,
                activity = config.InterfaceParams.Activity,
                resource = resource,
                user = config.InterfaceParams.User,
                amount = "1",
                assembleDataArray = new assembleData[] {
                    new assembleData{
                        sfc = sfc,
                        inventoryArray = inventoryArray,
                    }
                },
            };

            var binding = new BasicHttpBinding();
            binding.Security.Mode = config.ConnectionParams.BasicHttpSecurityMode;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.SendTimeout = TimeSpan.FromMilliseconds(config.ConnectionParams.Timeout);
            if (string.IsNullOrEmpty(config.ConnectionParams.Url))
            {
                throw new Exception($"The URL cannot be empty when calling CATL WebService, check the MESCFG.ini configuration file!");
            }
            var address = new EndpointAddress(config.ConnectionParams.Url);

            using (var scf = new ChannelFactory<MiAssembleComponentsToSfcsService>(binding, address))
            {
                scf.Credentials.UserName.UserName = config.ConnectionParams.UserName;
                scf.Credentials.UserName.Password = config.ConnectionParams.Password;
                var behavior = new UsePreAuthenticateHttpClientEndpointBehavior(scf.Credentials.UserName.UserName, scf.Credentials.UserName.Password);
                scf.Endpoint.EndpointBehaviors.Add(behavior);
                var channel = scf.CreateChannel();
                var req = new miAssembleComponentsToSfcsRequest(new miAssembleComponentsToSfcs
                {
                    AssembleComponentsToSfcsRequest = assemblereq
                });
                var sentAt = DateTime.Now;
                try
                {
                    var resp = await channel.miAssembleComponentsToSfcsAsync(req);
                    var result = resp.miAssembleComponentsToSfcsResponse.@return;
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = sfc,
                        Payload = JsonConvert.SerializeObject(assemblereq),
                        ReceivedAt = receivedAt,
                        RespCode = result.code.ToString(),
                        RespInfo = JsonConvert.SerializeObject(result),
                    };
                    var svcname = iniHelper.AssembleComponentsForSfcsConfig;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    return result;
                }
                catch (Exception ex)
                {
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = sfc,
                        Payload = JsonConvert.SerializeObject(assemblereq),
                        ReceivedAt = receivedAt,
                        RespCode = String.Empty,
                        RespInfo = JsonConvert.SerializeObject(ex.Message),
                    };
                    var svcname = iniHelper.AssembleComponentsForSfcsConfig;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    throw;
                }
            }

        }
        public async Task<findCustomAndSfcDataResponse> FindCustomAndSfcDataAsync(string sfc)
        {
            var config = iniHelper.GetMiFindCustomAndSfcDataConfig();
            var resource = config.InterfaceParams.Resource;
            var findCustomReq = new findCustomAndSfcDataRequest
            {
                site = config.InterfaceParams.Site,
                operation = config.InterfaceParams.Operation,
                operationRevision = config.InterfaceParams.OperationRevision,
                resource = resource,
                masterDataArray = [config.InterfaceParams.MasterData],
                customDataArray = [new customDataInParametricData() { category = config.InterfaceParams.CategoryData, dataField = config.InterfaceParams.DataField }],
                user = config.InterfaceParams.User,
                activity = config.InterfaceParams.Activity,
                sfc = sfc,
                modeProcessSFC = config.InterfaceParams.Mode,
            };

            var binding = new BasicHttpBinding();
            binding.Security.Mode = config.ConnectionParams.BasicHttpSecurityMode;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.SendTimeout = TimeSpan.FromMilliseconds(config.ConnectionParams.Timeout);
            if (string.IsNullOrEmpty(config.ConnectionParams.Url))
            {
                throw new Exception($"The URL cannot be empty when calling CATL WebService, check the MESCFG.ini configuration file!");
            }
            var address = new EndpointAddress(config.ConnectionParams.Url);

            using var scf = new ChannelFactory<MiFindCustomAndSfcDataService>(binding, address);
            scf.Credentials.UserName.UserName = config.ConnectionParams.UserName;
            scf.Credentials.UserName.Password = config.ConnectionParams.Password;
            var behavior = new UsePreAuthenticateHttpClientEndpointBehavior(scf.Credentials.UserName.UserName, scf.Credentials.UserName.Password);
            scf.Endpoint.EndpointBehaviors.Add(behavior);
            var channel = scf.CreateChannel();
            var req = new miFindCustomAndSfcDataRequest(new miFindCustomAndSfcData
            {
                FindCustomAndSfcDataRequest = findCustomReq
            });
            var sentAt = DateTime.Now;
            try
            {
                var resp = await channel.miFindCustomAndSfcDataAsync(req);
                var result = resp.miFindCustomAndSfcDataResponse.@return;
                var receivedAt = DateTime.Now;
                var logging = new SfcInvocationLogging
                {
                    SentAt = sentAt,
                    SfcNumber = sfc,
                    Payload = JsonConvert.SerializeObject(findCustomReq),
                    ReceivedAt = receivedAt,
                    RespCode = result.code.ToString(),
                    RespInfo = JsonConvert.SerializeObject(result),
                };
                var svcname = iniHelper.MiFindCustomAndSfcDataInterfaceName;
                var svcdesc = "";
                await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                return result;
            }
            catch (Exception ex)
            {
                var receivedAt = DateTime.Now;
                var logging = new SfcInvocationLogging
                {
                    SentAt = sentAt,
                    SfcNumber = sfc,
                    Payload = JsonConvert.SerializeObject(findCustomReq),
                    ReceivedAt = receivedAt,
                    RespCode = String.Empty,
                    RespInfo = JsonConvert.SerializeObject(ex.Message),
                };
                var svcname = iniHelper.MiFindCustomAndSfcDataInterfaceName;
                var svcdesc = "";
                await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                throw;
            }

        }
        public async Task<miCustomDCForCellResponse> MiCustomDCForCellAsync(Action<IList<miCustomDCForCellInventory>> updateParams)
        {
            var config = iniHelper.GetMiCustomDCForCellConfig();
            var resource = config.InterfaceParams.Resource;
            var parameters = new List<miCustomDCForCellInventory>();
            updateParams(parameters);

            parameters.ForEach(p => p.marking = config.InterfaceParams.Marking);
            var payload = new miCustomDCForCellRequest
            {
                site = config.InterfaceParams.Site,
                user = config.InterfaceParams.User,
                dcSequence = config.InterfaceParams.DcSequence,
                Multispec = config.InterfaceParams.Multispec,
                operation = config.InterfaceParams.Operation,
                resource = resource,
                inventoryList = [.. parameters]
            };

            var binding = new BasicHttpBinding();
            binding.Security.Mode = config.ConnectionParams.BasicHttpSecurityMode;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.SendTimeout = TimeSpan.FromMilliseconds(config.ConnectionParams.Timeout);
            if (string.IsNullOrEmpty(config.ConnectionParams.Url))
            {
                throw new Exception($"The URL cannot be empty when calling CATL WebService, check the MESCFG.ini configuration file!");
            }
            var address = new EndpointAddress(config.ConnectionParams.Url);

            using (var scf = new ChannelFactory<MiCustomDCForCellService>(binding, address))
            {
                scf.Credentials.UserName.UserName = config.ConnectionParams.UserName;
                scf.Credentials.UserName.Password = config.ConnectionParams.Password;
                var behavior = new UsePreAuthenticateHttpClientEndpointBehavior(scf.Credentials.UserName.UserName, scf.Credentials.UserName.Password);
                scf.Endpoint.EndpointBehaviors.Add(behavior);
                var channel = scf.CreateChannel();
                var req = new cellCustomDCCheckRequest(new cellCustomDCCheck { Request = payload });
                var sentAt = DateTime.Now;
                try
                {
                    var resp = await channel.cellCustomDCCheckAsync(req);
                    var result = resp.cellCustomDCCheckResponse.@return;
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = parameters.First().inventoryId,
                        Payload = JsonConvert.SerializeObject(payload),
                        ReceivedAt = receivedAt,
                        RespCode = result.code.ToString(),
                        RespInfo = JsonConvert.SerializeObject(result),
                    };
                    var svcname = iniHelper.MiCustomDCForCellInterfaceName;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    return result;
                }
                catch (Exception ex)
                {
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = parameters.First().inventoryId,
                        Payload = JsonConvert.SerializeObject(payload),
                        ReceivedAt = receivedAt,
                        RespCode = String.Empty,
                        RespInfo = JsonConvert.SerializeObject(ex.Message),
                    };
                    var svcname = iniHelper.MiCustomDCForCellInterfaceName;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    throw;
                }
            }

        }
        public async Task<Catl.WebServices.DataCollectForResourceFAI.machineIntegrationResourceDcResponse> DataCollectForResourceFAIAsync(string sfc, Action<IList<Catl.WebServices.DataCollectForResourceFAI.machineIntegrationParametricData>> updateParams)
        {
            var config = iniHelper.GetDataCollectForResourceFAIConfig();
            var resource = config.InterfaceParams.Resource;
            var parameters = new List<Catl.WebServices.DataCollectForResourceFAI.machineIntegrationParametricData>();
            updateParams(parameters);
            var payload = new dataCollectForResourceFAIRequest
            {
                site = config.InterfaceParams.Site,
                dcGroup = config.InterfaceParams.DcGroup,
                dcMode = config.InterfaceParams.DcMode,
                sfc = sfc,
                material = config.InterfaceParams.Material,
                materialRevision = config.InterfaceParams.MaterialRevision,
                dcGroupRevision = config.InterfaceParams.DcGroupRevision,
                resource = resource,
                operation = config.InterfaceParams.Operation,
                operationRevision = config.InterfaceParams.OperationRevision,
                dcGroupSequence = config.InterfaceParams.DcGroupSequence,
                user = config.InterfaceParams.User,
                parametricDataArray = parameters.ToArray(),
            };

            var binding = new BasicHttpBinding();
            binding.Security.Mode = config.ConnectionParams.BasicHttpSecurityMode;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.SendTimeout = TimeSpan.FromMilliseconds(config.ConnectionParams.Timeout);
            if (string.IsNullOrEmpty(config.ConnectionParams.Url))
            {
                throw new Exception($"The URL cannot be empty when calling CATL WebService, check the MESCFG.ini configuration file!");
            }
            var address = new EndpointAddress(config.ConnectionParams.Url);

            using (var scf = new ChannelFactory<DataCollectForResourceFAIService>(binding, address))
            {
                scf.Credentials.UserName.UserName = config.ConnectionParams.UserName;
                scf.Credentials.UserName.Password = config.ConnectionParams.Password;
                var behavior = new UsePreAuthenticateHttpClientEndpointBehavior(scf.Credentials.UserName.UserName, scf.Credentials.UserName.Password);
                scf.Endpoint.EndpointBehaviors.Add(behavior);
                var channel = scf.CreateChannel();
                var req = new dataCollectForResourceFAIRequest1(new dataCollectForResourceFAI { resourceRequest = payload });
                var sentAt = DateTime.Now;
                try
                {
                    var resp = await channel.dataCollectForResourceFAIAsync(req);
                    var result = resp.dataCollectForResourceFAIResponse.@return;
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = sfc,
                        Payload = JsonConvert.SerializeObject(payload),
                        ReceivedAt = receivedAt,
                        RespCode = result.code.ToString(),
                        RespInfo = JsonConvert.SerializeObject(result),
                    };
                    var svcname = iniHelper.DataCollectForResourceFAIInterfaceName;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    return result;
                }
                catch (Exception ex)
                {
                    var receivedAt = DateTime.Now;
                    var logging = new SfcInvocationLogging
                    {
                        SentAt = sentAt,
                        SfcNumber = sfc,
                        Payload = JsonConvert.SerializeObject(payload),
                        ReceivedAt = receivedAt,
                        RespCode = String.Empty,
                        RespInfo = JsonConvert.SerializeObject(ex.Message),
                    };
                    var svcname = iniHelper.DataCollectForResourceFAIInterfaceName;
                    var svcdesc = "";
                    await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                    throw;
                }
            }
        }
        public async Task<miSFCAttriDataEntryResponse> MiSFCAttriDataEntryAsync(string sfc, string[] barCodes)
        {
            var config = iniHelper.GetMiSFCAttriDataEntryConfig();
            var resource = "";
            var payload = new miSFCAttriDataEntryRequest
            {
                site = config.InterfaceParams.Site,
                sfc = sfc,
                userId = config.InterfaceParams.User,
                sfcMode = config.InterfaceParams.SfcMode,
                itemGroup = config.InterfaceParams.ItemGroup,
                isCheckSequence = config.InterfaceParams.IsCheckSequence,
                sfcDatalist = barCodes.Select((s, index) => new sfcData
                {
                    attributes = config.InterfaceParams.Attributes,
                    value = s,
                    sequence = index.ToString(),
                }).ToArray(),
            };

            var binding = new BasicHttpBinding();
            binding.Security.Mode = config.ConnectionParams.BasicHttpSecurityMode;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic;
            binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.SendTimeout = TimeSpan.FromMilliseconds(config.ConnectionParams.Timeout);
            if (string.IsNullOrEmpty(config.ConnectionParams.Url))
            {
                throw new Exception($"The URL cannot be empty when calling CATL WebService, check the MESCFG.ini configuration file!");
            }
            var address = new EndpointAddress(config.ConnectionParams.Url);

            using var scf = new ChannelFactory<MiSFCAttriDataEntryService>(binding, address);
            scf.Credentials.UserName.UserName = config.ConnectionParams.UserName;
            scf.Credentials.UserName.Password = config.ConnectionParams.Password;
            var behavior = new UsePreAuthenticateHttpClientEndpointBehavior(scf.Credentials.UserName.UserName, scf.Credentials.UserName.Password);
            scf.Endpoint.EndpointBehaviors.Add(behavior);
            var channel = scf.CreateChannel();
            var req = new miSfcAttriDataEntryRequest1(new miSfcAttriDataEntry
            {
                MiSFCAttriDataEntryRequest = payload
            });
            var sentAt = DateTime.Now;
            try
            {
                var resp = await channel.miSfcAttriDataEntryAsync(req);
                var result = resp.miSfcAttriDataEntryResponse.@return;
                var receivedAt = DateTime.Now;
                var logging = new SfcInvocationLogging
                {
                    SentAt = sentAt,
                    SfcNumber = sfc,
                    Payload = JsonConvert.SerializeObject(payload),
                    ReceivedAt = receivedAt,
                    RespCode = result.code.ToString(),
                    RespInfo = JsonConvert.SerializeObject(result),
                };
                var svcname = iniHelper.MiSFCAttriDataEntryInterfaceName;
                var svcdesc = "";
                await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                return result;
            }
            catch (Exception ex)
            {
                var receivedAt = DateTime.Now;
                var logging = new SfcInvocationLogging
                {
                    SentAt = sentAt,
                    SfcNumber = sfc,
                    Payload = JsonConvert.SerializeObject(payload),
                    ReceivedAt = receivedAt,
                    RespCode = String.Empty,
                    RespInfo = JsonConvert.SerializeObject(ex.Message),
                };
                var svcname = iniHelper.MiSFCAttriDataEntryInterfaceName;
                var svcdesc = "";
                await this.WriteExcelAsync(logging, svcname, svcdesc, resource);
                throw;
            }
        }
    }
}
