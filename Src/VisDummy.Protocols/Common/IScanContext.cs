using System;

namespace Itminus.Protocols.Common
{
    public interface IScanContextWithHeartBeat
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTimeOffset CreatedAt { get; }

        /// <summary>
        /// MST和DEV之间心跳是否已经同步
        /// </summary>
        public bool HeartBeatSynced { get; }
    }
}
