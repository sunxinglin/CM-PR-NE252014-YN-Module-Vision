using System;
using System.Threading;
using System.Threading.Tasks;

namespace VisDummy.Protocols.Common
{
    public class InMemorySimpleSignal<TValue>
    {
        public string Name { get; }

        private TValue _value = default;
        private readonly SemaphoreSlim _sema = new(1, 1);

        public InMemorySimpleSignal(string name, TValue init)
        {
            this.Name = name;
            this._value = init;
        }

        public TValue GetValue() => _value;

        /// <summary>
        /// 设置信号值
        /// </summary>
        /// <param name="v"></param>
        /// <param name="timeout">/ms</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SetValue(TValue v, int timeout)
        {
            var entered = await this._sema.WaitAsync(timeout);
            if (entered)
            {
                try
                {
                    this._value = v;
                }
                finally
                {
                    this._sema.Release();
                }
            }
            else
            {
                throw new Exception($"设置信号{this.Name}值={v}超时");
            }
        }
    }
}
