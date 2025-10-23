using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Itminus.Protocols.Common
{
    /// <summary>
    /// flags (max 4 bytes)
    /// </summary>
    /// <typeparam name="TFlags"></typeparam>
    public class FlagsBuilder<TFlags>
        where TFlags : Enum
    {
        protected uint _wCmd;

        public FlagsBuilder(TFlags wCmd)
        {
            this._wCmd = Unsafe.As<TFlags, uint>(ref wCmd);
        }

        /// <summary>
        /// 构建命令字
        /// </summary>
        /// <returns></returns>
        public virtual TFlags Build() => Unsafe.As<uint, TFlags>(ref _wCmd);

        public virtual FlagsBuilder<TFlags> SetOnOff(TFlags bitIndicator, bool onoff)
        {
            var indicator = Unsafe.As<TFlags, uint>(ref bitIndicator);
            this._wCmd = onoff ? this._wCmd | indicator : this._wCmd & ~indicator;
            return this;
        }
    }
}
