using GlobalVariableModuleCs;
using VM.Core;
using System;
using Itminus.FSharpExtensions;
using Microsoft.FSharp.Core;
using System.Collections.Generic;
using VisDummy.Abstractions.VmSolutionParams;
using VisDummy.Lang.Resources;

namespace VisDummy.VMs.Services.VmSolutionParams
{
    public static class VmGlobalSetExtension
    {
        public static FSharpResult<string[], string> SetGlobalValue(this GlobalVariableModuleTool global, IEnumerable<SolutionParam> solutionParams)
        {
            List<string> tmps = [];
            foreach (var param in solutionParams)
            {
                var r = global.SetGlobalValue(param);
                if (r.IsError)
                    return r.ErrorValue.ToErrResult<string[], string>();
                tmps.Add(r.ResultValue);
            }
            return tmps.ToArray().ToOkResult<string[], string>();
        }

        public static FSharpResult<string, string> SetGlobalValue(this GlobalVariableModuleTool global, SolutionParam solutionParam)
        {
            try
            {
                global.SetGlobalVar(solutionParam.Name, solutionParam.Value);
                var tmp = global.GetGlobalVar(solutionParam.Name);
                return $"{solutionParam.Name}:{tmp}".ToOkResult<string, string>();
            }
            catch (Exception ex)
            {
                return $"{Language.Msg_设置全局变量失败}：{solutionParam.Name}:{ex.Message}".ToErrResult<string, string>();
            }
        }

        public static FSharpResult<GlobalVariableModuleTool, string> GetGlobalVariableModuleTool()
        {
            try
            {
                var cn = (GlobalVariableModuleTool)VmSolution.Instance["全局变量1"];
                if (cn != null)
                {
                    return cn.ToOkResult<GlobalVariableModuleTool, string>();
                }
                var en = (GlobalVariableModuleTool)VmSolution.Instance["Global Variable1"];
                if (en != null)
                {
                    return en.ToOkResult<GlobalVariableModuleTool, string>();
                }
                return Language.Msg_获取全局变量功能类失败.ToErrResult<GlobalVariableModuleTool, string>();
            }
            catch (Exception ex)
            {
                return $"{Language.Msg_获取全局变量功能类失败}：{ex.Message}".ToErrResult<GlobalVariableModuleTool, string>();
            }
        }
    }
}
