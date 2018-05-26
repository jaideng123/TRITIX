using System.Collections.Specialized;
using UnityEngine;

public class Vector3Int
{
    public int x, y, z;

    public Vector3Int(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3Int()
    {
        x = 0;
        y = 0;
        z = 0;
    }

    public override string ToString()
    {
        return string.Format("[{0},{1},{2}]", x, y, z);
    }
    public override int GetHashCode()
    {
        unchecked
        {
            BitVector32 bit = new BitVector32(0);
            BitVector32.Section xSect = BitVector32.CreateSection(3);
            BitVector32.Section ySect = BitVector32.CreateSection(3, xSect);
            BitVector32.Section zSect = BitVector32.CreateSection(3, ySect);
            bit[xSect] = x;
            bit[zSect] = z;
            bit[ySect] = y;
            // Debug.Log(bit.ToString());
            return bit.Data;
        }
    }
}