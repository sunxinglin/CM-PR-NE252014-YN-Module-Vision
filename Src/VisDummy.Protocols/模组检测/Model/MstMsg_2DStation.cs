using FutureTech.Protocols;
using System.Runtime.InteropServices;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.模组检测.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_2DStation
    {
        public Mst_CmdReply CmdReply;

        [Endian(Endianness.BigEndian)]
        public ushort component;

        [Endian(Endianness.BigEndian)]
        public ushort polarity1;
        [Endian(Endianness.BigEndian)]
        public ushort polarity2;
        [Endian(Endianness.BigEndian)]
        public ushort polarity3;
        [Endian(Endianness.BigEndian)]
        public ushort polarity4;
        [Endian(Endianness.BigEndian)]
        public ushort polarity5;
        [Endian(Endianness.BigEndian)]
        public ushort polarity6;
        [Endian(Endianness.BigEndian)]
        public ushort polarity7;
        [Endian(Endianness.BigEndian)]
        public ushort polarity8;
        [Endian(Endianness.BigEndian)]
        public ushort polarity9;
        [Endian(Endianness.BigEndian)]
        public ushort polarity10;
        [Endian(Endianness.BigEndian)]
        public ushort polarity11;
        [Endian(Endianness.BigEndian)]
        public ushort polarity12;
        [Endian(Endianness.BigEndian)]
        public ushort polarity13;
        [Endian(Endianness.BigEndian)]
        public ushort polarity14;
        [Endian(Endianness.BigEndian)]
        public ushort polarity15;
        [Endian(Endianness.BigEndian)]
        public ushort polarity16;
        [Endian(Endianness.BigEndian)]
        public ushort polarity17;
        [Endian(Endianness.BigEndian)]
        public ushort polarity18;
        [Endian(Endianness.BigEndian)]
        public ushort polarity19;
        [Endian(Endianness.BigEndian)]
        public ushort polarity20;

        public ushort Component => component;

        public ushort[] Polaritys => new ushort[] {
            polarity1,polarity2,polarity3,polarity4,polarity5,polarity6,polarity7,polarity8,polarity9,polarity10,
            polarity11,polarity12,polarity13,polarity14,polarity15,polarity16,polarity17,polarity18,polarity19,polarity20
        };

        public void SetOff()
        {
            CmdReply.SetOff();
            component = 0;
            polarity1 = 0;
            polarity2 = 0;
            polarity3 = 0;
            polarity4 = 0;
            polarity5 = 0;
            polarity6 = 0;
            polarity7 = 0;
            polarity8 = 0;
            polarity9 = 0;
            polarity10 = 0;
            polarity11 = 0;
            polarity12 = 0;
            polarity13 = 0;
            polarity14 = 0;
            polarity15 = 0;
            polarity16 = 0;
            polarity17 = 0;
            polarity18 = 0;
            polarity19 = 0;
            polarity20 = 0;
        }
        public void SetPolaritys(ushort[] polaritys)
        {
            if (polaritys.Length < 20)
            {
                throw new Exception("待写入的极性数组长度必须大于20");
            }
            polarity1 = polaritys[0];
            polarity2 = polaritys[1];
            polarity3 = polaritys[2];
            polarity4 = polaritys[3];
            polarity5 = polaritys[4];
            polarity6 = polaritys[5];
            polarity7 = polaritys[6];
            polarity8 = polaritys[7];
            polarity9 = polaritys[8];
            polarity10 = polaritys[9];
            polarity11 = polaritys[10];
            polarity12 = polaritys[11];
            polarity13 = polaritys[12];
            polarity14 = polaritys[13];
            polarity15 = polaritys[14];
            polarity16 = polaritys[15];
            polarity17 = polaritys[16];
            polarity18 = polaritys[17];
            polarity19 = polaritys[18];
            polarity20 = polaritys[19];
        }
    }
}
