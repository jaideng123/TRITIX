using System;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string id;
    public Dictionary<PieceType, int> bank;
    public string pieceMaterialName;

    public Color pieceColor;

    public Player()
    {
        bank = new Dictionary<PieceType, int>();
        PieceType[] pieces = Enum.GetValues(typeof(PieceType)) as PieceType[];
        foreach (PieceType type in pieces)
        {
            if (type == PieceType.NONE)
            {
                continue;
            }
            bank.Add(type, 0);
        }
        bank[PieceType.WILD] = 1;
    }
}