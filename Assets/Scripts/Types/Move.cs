using System;
using System.Collections;
using System.Collections.Generic;
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
}