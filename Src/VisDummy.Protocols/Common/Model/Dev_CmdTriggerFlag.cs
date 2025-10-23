namespace VisDummy.Protocols.Common.Model
{
    [Flags]
    public enum Dev_CmdTriggerFlag : ushort
    {
        None = 0,
        Trigger1 = 1 << 0,
        Trigger2 = 1 << 1,
    }
}
