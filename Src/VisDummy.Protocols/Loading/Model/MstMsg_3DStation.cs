using System.Runtime.InteropServices;
using FutureTech.Protocols;
using VisDummy.Abstractions;
using VisDummy.Protocols.Common.Model;

namespace VisDummy.Protocols.Loading.Model
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class MstMsg_3DStation
    {
        public Mst_CmdReplyFlag flag;

        [Endian(Endianness.BigEndian)]
        public ushort result;

        [Endian(Endianness.BigEndian)]
        public ushort foam;

        [Endian(Endianness.BigEndian)]
        public ushort floor;
        [Endian(Endianness.BigEndian)]
        public ushort column;
        [Endian(Endianness.BigEndian)]
        public ushort direction;

        [Endian(Endianness.BigEndian)]
        public float x;
        [Endian(Endianness.BigEndian)]
        public float y;
        [Endian(Endianness.BigEndian)]
        public float z;
        [Endian(Endianness.BigEndian)]
        public float a;
        [Endian(Endianness.BigEndian)]
        public float b;
        [Endian(Endianness.BigEndian)]
        public float c;

        public bool Ack => flag.HasFlag(Mst_CmdReplyFlag.Ack);
        public bool AckOk => flag.HasFlag(Mst_CmdReplyFlag.Ack_Ok);
        public bool AckNg => flag.HasFlag(Mst_CmdReplyFlag.Ack_Ng);
        public ushort Result => result;
        public string ResultStatus => WarpHelper.ResultConvert(result);
        public ushort Foam => foam;
        public ushort Floor => floor;
        public ushort Column => column;
        public ushort Direction => direction;
        public float X => x;
        public float Y => y;
        public float Z => z;
        public float A => a;
        public float B => b;
        public float C => c;

        public void SetOn(bool isok, CmdArg_Reply3D cmd)
        {
            flag = new MstMsg_CmdReplyFlagsBuilder(flag).SetOn(isok).Build();
            result = cmd.Result;
            foam = cmd.Foam;
            floor = cmd.Floor;
            column = cmd.Column;
            direction = cmd.Direction;
            x = cmd.X;
            y = cmd.Y;
            z = cmd.Z;
            a = cmd.A;
            b = cmd.B;
            c = cmd.C;
        }
        public void SetOff()
        {
            flag = new MstMsg_CmdReplyFlagsBuilder(flag).SetOff().Build();
            result = 0;
            foam = 0;
            floor = 0;
            column = 0;
            direction = 0;
            x = 0;
            y = 0;
            z = 0;
            a = 0;
            b = 0;
            c = 0;
        }
    }
}
