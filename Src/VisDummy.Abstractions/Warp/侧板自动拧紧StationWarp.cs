using System.Net.NetworkInformation;

namespace VisDummy.Abstractions.Warp.侧板自动拧紧
{
	public class StationOkWrap_侧板自动拧紧
	{
		public string? ImagePath { get; set; }
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public float PositionZ { get; set; }
		public float PositionA { get; set; }
		public int Status { get; set; }
		public string ToMsg()
		{
			return $"ImagePath:{ImagePath}";
		}
	}

	public class StationErrWrap_侧板自动拧紧
	{
		public bool Flag { get; set; }
		public bool 特征点NG { get; set; }
		public bool 视觉检测流程NG { get; set; }
		public bool 其它NG { get; set; }
		public bool PLC参数NG { get; set; }

		public string ImagePath { get; set; }
		public string ErrMsg { get; set; } = string.Empty;
		public uint ErrorCode { get; set; }
		public string ToMsg()
		{
			return $"ImagePath:{ImagePath};ErrMsg:{ErrMsg};ErrorCode:{ErrorCode}";
		}
	}

	public enum PhotoNumbenEnum
	{
		单枪拧紧点检拍照每次完成后视觉反馈偏移量 = 1,
		双枪拧紧第二次拍照完成后视觉反馈偏移量 = 2,
		相机标定1到12位置号自动标定 = 3
	}
}
