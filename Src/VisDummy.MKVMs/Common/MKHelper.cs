using Itminus.FSharpExtensions;
using Microsoft.FSharp.Core;

namespace VisDummy.MKVMs.Common
{
    public class Vision3D_102_DTO
    {
        /// <summary>
        /// 结果状态码
        /// </summary>
        public string ResultStatus { get; set; } = string.Empty;
    }
    public class Vision3D_110_DTO
    {
        /// <summary>
        /// 流程状态
        /// </summary>
        public string ProcessStatus { get; set; } = string.Empty;

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        /// <summary>
        /// 结果状态码
        /// </summary>
        public ushort ResultStatus { get; set; }

        public ushort Column { get; set; }
        public ushort Postion { get; set; }
        public ushort Floor { get; set; }
    }

    public static class Vision3DExtension
    {
        public static FSharpResult<bool, string> Analyze101(this string cmd)
        {
            var cmds = cmd.Split(',');
            if (cmds.Length < 2)
            {
                return $"Failed To Parse 101：数组长度={cmds.Length},不足2".ToErrResult<bool, string>();
            }
            if (cmds[0] != "101")
            {
                return $"Failed To Parse 101：非101命令字符集".ToErrResult<bool, string>();
            }
            if (cmds[1] != "1102")
            {
                return $"Failed To Parse 101：状态码异常值:{cmds[1]}".ToErrResult<bool, string>();
            }
            return true.ToOkResult<bool, string>();
        }
        public static FSharpResult<Vision3D_102_DTO, string> Analyze102(this string cmd)
        {
            var cmds = cmd.Split(',');
            if (cmds.Length < 12)
            {
                return $"Failed To Parse 102：数组长度={cmds.Length},不足12".ToErrResult<Vision3D_102_DTO, string>();
            }
            if (cmds[0] != "102")
            {
                return $"Failed To Parse 102：非102命令字符集".ToErrResult<Vision3D_102_DTO, string>();
            }
            if (cmds[1] != "1100")
            {
                return $"Failed To Parse 102：状态码异常值:{cmds[1]}".ToErrResult<Vision3D_102_DTO, string>();
            }
            return new Vision3D_102_DTO { ResultStatus = cmds[11] }.ToOkResult<Vision3D_102_DTO, string>();
        }
        public static FSharpResult<bool, string> Analyze103(this string cmd)
        {
            var cmds = cmd.Split(',');
            if (cmds.Length < 2)
            {
                return $"Failed To Parse 103：数组长度={cmds.Length},不足2".ToErrResult<bool, string>();
            }
            if (cmds[0] != "103")
            {
                return $"Failed To Parse 103：非103命令字符集".ToErrResult<bool, string>();
            }
            if (cmds[1] != "1107")
            {
                return $"Failed To Parse 103：状态码异常值:{cmds[1]}".ToErrResult<bool, string>();
            }
            return true.ToOkResult<bool, string>();
        }
        public static FSharpResult<Vision3D_110_DTO, string> Analyze110(this string cmd)
        {
            var cmds = cmd.Split(',');
            if (cmds.Length < 14)
            {
                return $"Failed To Parse 110：数组长度={cmds.Length},不足14".ToErrResult<Vision3D_110_DTO, string>();
            }
            if (cmds[0] != "110")
            {
                return $"Failed To Parse 110：非110命令字符集".ToErrResult<Vision3D_110_DTO, string>();
            }
            if (cmds[1] != "1100")
            {
                return $"Failed To Parse 110：状态码异常值:{cmds[1]}".ToErrResult<Vision3D_110_DTO, string>();
            }

            var r1 = float.TryParse(cmds[4], out var x);
            if (!r1)
                return $"Failed To Parse 110：X非Float类型".ToErrResult<Vision3D_110_DTO, string>();
            var r2 = float.TryParse(cmds[5], out var y);
            if (!r2)
                return $"Failed To Parse 110：Y非Float类型".ToErrResult<Vision3D_110_DTO, string>();
            var r3 = float.TryParse(cmds[6], out var z);
            if (!r3)
                return $"Failed To Parse 110：Z非Float类型".ToErrResult<Vision3D_110_DTO, string>();

            var r4 = float.TryParse(cmds[7], out var a);
            if (!r4)
                return $"Failed To Parse 110：A非Float类型".ToErrResult<Vision3D_110_DTO, string>();
            var r5 = float.TryParse(cmds[8], out var b);
            if (!r5)
                return $"Failed To Parse 110：B非Float类型".ToErrResult<Vision3D_110_DTO, string>();
            var r6 = float.TryParse(cmds[9], out var c);
            if (!r6)
                return $"Failed To Parse 110：C非Float类型".ToErrResult<Vision3D_110_DTO, string>();

            var r7 = ushort.TryParse(cmds[10], out var status);
            if (!r7)
                return $"Failed To Parse 110：工程状态码非ushort类型".ToErrResult<Vision3D_110_DTO, string>();

            var r8 = ushort.TryParse(cmds[11], out var col);
            if (!r8)
                return $"Failed To Parse 110：电芯列数非ushort类型".ToErrResult<Vision3D_110_DTO, string>();

            var r9 = ushort.TryParse(cmds[12], out var postion);
            if (!r9)
                return $"Failed To Parse 110：电芯来料方向非ushort类型".ToErrResult<Vision3D_110_DTO, string>();

            var r10 = ushort.TryParse(cmds[13], out var floor);
            if (!r10)
                return $"Failed To Parse 110：泡棉层高非ushort类型".ToErrResult<Vision3D_110_DTO, string>();

            return new Vision3D_110_DTO { ProcessStatus = cmds[1], X = x, Y = y, Z = z, A = a, B = b, C = c, ResultStatus = status, Column = col, Postion = postion, Floor = floor }.ToOkResult<Vision3D_110_DTO, string>();
        }
    }
}
