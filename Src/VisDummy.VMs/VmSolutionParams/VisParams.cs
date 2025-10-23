using Itminus.FSharpExtensions;
using Microsoft.FSharp.Core;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VisDummy.Abstractions.Infra;
using VisDummy.Abstractions.VmSolutionParams;

namespace VisDummy.VMs.Services.VmSolutionParams
{

    public class VisParams : IVisParams
    {
        private readonly string GlobalParamsFileName = string.Empty;
        private readonly string FileName = "GlobalParams.json";
        public VisParams(string folder)
        {
            GlobalParamsFileName = Path.Combine(folder, FileName);
        }

        #region 文件读写
        private Task<T?> ReadFileAsync<T>(string path)
        {
            if (!File.Exists(path))
                return Task.FromResult(default(T));
            var file = File.ReadAllText(path);
            var r = JsonConvert.DeserializeObject<T>(file);
            return Task.FromResult(r);
        }
        private Task WriteFileAsync<T>(T value, string path)
        {
            var txt = JsonConvert.SerializeObject(value);
            File.WriteAllText(path, txt);
            return Task.CompletedTask;
        }
        #endregion

        public async Task<FSharpResult<GlobalParams, string>> GetGlobalParams()
        {
            try
            {
                var p = await ReadFileAsync<GlobalParams>(GlobalParamsFileName);
                if (p == null)
                {
                    return new GlobalParams().ToOkResult<GlobalParams, string>();
                }
                return p.ToOkResult<GlobalParams, string>();
            }
            catch (Exception ex)
            {
                return ex.Message.ToErrResult<GlobalParams, string>();
            }
        }
        public async Task<FSharpResult<GlobalParams, string>> SaveGlobalParams(GlobalParams globalParams)
        {
            try
            {
                var read = await GetGlobalParams();
                if (read.IsError)
                    return read;
                await WriteFileAsync(globalParams, GlobalParamsFileName);
                return read.ResultValue.ToOkResult<GlobalParams, string>();
            }
            catch (Exception ex)
            {
                return ex.Message.ToErrResult<GlobalParams, string>();
            }
        }

        public async Task<FSharpResult<string[], string>> SetTriggerGlobalParams()
        {
            try
            {
                var r = from r1 in GetGlobalParams().SelectOk(s => s.SolutionParams.Where(s => s.Type == SolutionParamEnum.Trigger))
                        .Bind(rs =>
                        {
                            if (rs.Count() == 0)
                                return Array.Empty<string>().ToOkResult<string[], string>().ToTask();
                            var r = from r2 in VmGlobalSetExtension.GetGlobalVariableModuleTool()
                                    from r3 in r2.SetGlobalValue(rs)
                                    select r3;
                            return r.ToTask();
                        })
                        select r1;
                return await r;
            }
            catch (Exception ex)
            {
                return ex.Message.ToErrResult<string[], string>();
            }
        }
        public async Task<FSharpResult<string[], string>> SetSingleGlobalParams()
        {
            try
            {
                var r = from r1 in GetGlobalParams().SelectOk(s => s.SolutionParams.Where(s => s.Type == SolutionParamEnum.Single))
                        .Bind(rs =>
                        {
                            if (rs.Count() == 0)
                                return Array.Empty<string>().ToOkResult<string[], string>().ToTask();
                            var r = from r2 in VmGlobalSetExtension.GetGlobalVariableModuleTool()
                                    from r3 in r2.SetGlobalValue(rs)
                                    select r3;
                            return r.ToTask();
                        })
                        select r1;
                return await r;
            }
            catch (Exception ex)
            {
                return ex.Message.ToErrResult<string[], string>();
            }
        }
        public async Task<FSharpResult<string[], string>> SetTimingGlobalParams()
        {
            try
            {
                var r = from r1 in GetGlobalParams().SelectOk(s => s.SolutionParams.Where(s => s.Type == SolutionParamEnum.Timing))
                        .Bind(rs =>
                        {
                            if (rs.Count() == 0)
                                return Array.Empty<string>().ToOkResult<string[], string>().ToTask();
                            var r = from r2 in VmGlobalSetExtension.GetGlobalVariableModuleTool()
                                    from r3 in r2.SetGlobalValue(rs)
                                    select r3;
                            return r.ToTask();
                        })
                        select r1;
                return await r;
            }
            catch (Exception ex)
            {
                return ex.Message.ToErrResult<string[], string>();
            }
        }
    }
}
