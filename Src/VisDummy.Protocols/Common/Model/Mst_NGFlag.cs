using Itminus.Protocols.Common;

namespace VisDummy.Protocols.Common.Model
{
    public enum Mst_NGFlag : byte
    {
        None = 0,
        PLC参数NG = 1 << 0,
        找特征点NG = 1 << 1,
        偏移量NG = 1 << 2,
        视觉流程NG = 1 << 3,
    }
    public class Mst_NGFlagsBuilder : FlagsBuilder<Mst_NGFlag>
    {
        public Mst_NGFlagsBuilder(Mst_NGFlag wCmd) : base(wCmd)
        {
        }

        public Mst_NGFlagsBuilder SetOn(bool PLC参数NG, bool 找特征点NG, bool 偏移量NG, bool 视觉流程NG)
        {
            SetOnOff(Mst_NGFlag.PLC参数NG, PLC参数NG);
            SetOnOff(Mst_NGFlag.找特征点NG, 找特征点NG);
            SetOnOff(Mst_NGFlag.偏移量NG, 偏移量NG);
            SetOnOff(Mst_NGFlag.视觉流程NG, 视觉流程NG);
            return this;
        }
        public Mst_NGFlagsBuilder SetOff()
        {
            SetOnOff(Mst_NGFlag.PLC参数NG, false);
            SetOnOff(Mst_NGFlag.找特征点NG, false);
            SetOnOff(Mst_NGFlag.偏移量NG, false);
            SetOnOff(Mst_NGFlag.视觉流程NG, false);
            return this;
        }
    }


	public enum Mst_SideNGFlag : ushort
	{
		None = 0,
		特征点NG = 1 << 0,
		视觉检测流程NG = 1 << 1,
		其它NG = 1 << 2,
		PLC参数NG = 1 << 3,
	}
	public class Mst_SideNGFlagsBuilder : FlagsBuilder<Mst_SideNGFlag>
	{
		public Mst_SideNGFlagsBuilder(Mst_SideNGFlag wCmd) : base(wCmd)
		{
		}

		public Mst_SideNGFlagsBuilder SetOn(bool 特征点NG, bool 视觉检测流程NG, bool 其它NG, bool PLC参数NG)
		{
			SetOnOff(Mst_SideNGFlag.特征点NG, 特征点NG);
			SetOnOff(Mst_SideNGFlag.视觉检测流程NG, 视觉检测流程NG);
			SetOnOff(Mst_SideNGFlag.其它NG, 其它NG);
			SetOnOff(Mst_SideNGFlag.PLC参数NG, PLC参数NG);
			return this;
		}
		public Mst_SideNGFlagsBuilder SetOff()
		{
			SetOnOff(Mst_SideNGFlag.特征点NG, false);
			SetOnOff(Mst_SideNGFlag.视觉检测流程NG, false);
			SetOnOff(Mst_SideNGFlag.其它NG, false);
			SetOnOff(Mst_SideNGFlag.PLC参数NG, false);
			return this;
		}
	}
}
