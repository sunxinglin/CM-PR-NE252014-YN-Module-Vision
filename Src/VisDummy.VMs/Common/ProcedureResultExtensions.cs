using Itminus.FSharpExtensions;
using Microsoft.FSharp.Core;
using System;
using VM.Core;
using VM.PlatformSDKCS;

namespace VisDummy.VMs.Common
{
    public static class ProcedureResultExtensions
    {

        public static FSharpResult<float, string> GetFloat(this ProcedureResult mres, string name)
        {
            try
            {
                var output = mres.GetOutputFloat(name);
                var ok = output.pFloatVal[0];
                return ok.ToOkResult<float, string>();
            }
            catch (Exception ex)
            {
                return $"Failed To Parse {name}:{ex.Message}".ToErrResult<float, string>();
            }
        }

        public static FSharpResult<int, string> GetInt(this ProcedureResult mres, string name)
        {
            try
            {
                var output = mres.GetOutputInt(name);
                var ok = output.pIntVal[0];
                return ok.ToOkResult<int, string>();
            }
            catch (Exception ex)
            {
                return $"Failed To Parse {name}:{ex.Message}".ToErrResult<int, string>();
            }
        }

        public static FSharpResult<string, string> GetString(this ProcedureResult mres, string name)
        {
            try
            {
                var output = mres.GetOutputString(name);
                if (output.astStringVal == null)
                {
                    return string.Empty.ToOkResult<string, string>();
                }
                var ok = output.astStringVal[0];
                return ok.strValue.ToOkResult<string, string>();
            }
            catch (Exception ex)
            {
                return $"Failed To Parse {name}:{ex.Message}".ToErrResult<string, string>();
            }
        }
        public static FSharpResult<ImageBaseData, string> GetImageV2(this ProcedureResult mres, string name)
        {
            try
            {
                var output = mres.GetOutputImageV2(name);

                return output.ToOkResult<ImageBaseData, string>();
            }
            catch (Exception ex)
            {
                return $"Failed To Parse {name}:{ex.Message}".ToErrResult<ImageBaseData, string>();
            }
        }
    }
}