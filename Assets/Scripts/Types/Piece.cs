using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
public class Piece
{
    public PieceType type;
    public int playerNum;

    public override int GetHashCode()
    {
        unchecked
        {
            BitVector32 bit = new BitVector32(0);
            BitVector32.Section numSect = BitVector32.CreateSection(3);
            BitVector32.Section pieceSect = BitVector32.CreateSection(5, numSect);
            bit[numSect] = playerNum;
            bit[pieceSect] = (int)type;
            return bit.Data;
        }
    }
}