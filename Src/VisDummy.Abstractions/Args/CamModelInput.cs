using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisDummy.Abstractions.Args
{
	public class CamModelInput
	{
		public string Batch { get; set; }

		public string ModuleCode { get; set; }

		public ushort Function { get; set; }

		public ushort Position { get; set; }

		public float PositionX { get; set; }

		public float PositionY { get; set; }

		public float PositionZ { get; set; }

		public float PositionA { get; set; }

		public string UID { get; set; }

	}
}
