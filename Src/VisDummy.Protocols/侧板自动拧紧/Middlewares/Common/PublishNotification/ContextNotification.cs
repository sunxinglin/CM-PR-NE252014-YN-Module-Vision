using MediatR;
using VisDummy.Protocols.侧板自动拧紧;
using VisDummy.Protocols.侧板自动拧紧.Model;

namespace VisDummy.Protocols.侧板自动拧紧.Middlewares.Common.PublishNotification
{
    /// <summary>
    /// 扫描PLC的上下文通知。注意：Notification应该是一个POCO，必须可序列化、可反序列化
    /// </summary>
    public class ScanContextNotification : INotification
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ScanContextNotification()
        {
        }

        /// <summary>
        /// 从ScanContext构建通知
        /// </summary>
        /// <param name="ctx"></param>
        public ScanContextNotification(ScanContext ctx)
        {
            DevMsg = ctx.DevMsg;
            MstMsg = ctx.MstMsg;
            HeartBeated = ctx.HeartBeatSynced;
            CreatedAt = ctx.CreatedAt;
        }

        /// <summary>
        /// 只读属性
        /// </summary>
        public DevMsg DevMsg { get; set; }

        /// <summary>
        /// 只读属性
        /// </summary>
        public MstMsg MstMsg { get; set; }

        /// <summary>
        /// 心跳是否同步？
        /// </summary>
        public bool HeartBeated { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; }
    }
}
