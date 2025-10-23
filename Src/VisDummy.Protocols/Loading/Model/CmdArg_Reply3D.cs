namespace VisDummy.Protocols.Loading.Model;

public record CmdArg_Reply3D
{
    public CmdArg_Reply3D(ushort result)
    {
        Result = result;
    }
    public CmdArg_Reply3D(ushort result, ushort foam, ushort floor, ushort column, ushort direction, float x, float y, float z, float a, float b, float c)
    {
        Result = result;
        Foam = foam;
        Floor = floor;
        Column = column;
        Direction = direction;
        X = x;
        Y = y;
        Z = z;
        A = a;
        B = b;
        C = c;
    }
    public ushort Result;
    public ushort Foam;
    public ushort Floor;
    public ushort Column;
    public ushort Direction;
    public float X;
    public float Y;
    public float Z;
    public float A;
    public float B;
    public float C;
}
