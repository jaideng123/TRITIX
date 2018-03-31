using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
public class Move
{
    public int playerNum;
    public PieceType pieceType;
    public Vector3Int from;
    public Vector3Int to;

    public override string ToString()
    {
        return string.Format("Player:{0} , Piece:{1}, From:{2} To:{3}", playerNum, pieceType.ToString(), from, to);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            BitVector32 bit = new BitVector32(0);
            BitVector32.Section numSect = BitVector32.CreateSection(3);
            BitVector32.Section pieceSect = BitVector32.CreateSection(5, numSect);
            BitVector32.Section fromSect = BitVector32.CreateSection(63, pieceSect);
            BitVector32.Section toSect = BitVector32.CreateSection(63, fromSect);
            bit[numSect] = playerNum;

            bit[pieceSect] = (int)pieceType;
            if (from != null)
            {
                bit[fromSect] = from.GetHashCode();
            }
            bit[toSect] = to.GetHashCode();
            Debug.Log(bit.ToString());
            return bit.Data;
        }
    }


}