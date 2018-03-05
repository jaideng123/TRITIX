using System;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    public string id;
    public Dictionary<PieceType, int> bank;
    public string pieceMaterialName;

    public Player()
    {
        bank = new Dictionary<PieceType, int>();
        PieceType[] pieces = Enum.GetValues(typeof(PieceType)) as PieceType[];
        foreach (PieceType type in pieces)
        {
            bank.Add(type, 3);
        }
        bank[PieceType.WILD] = 1;
    }
}