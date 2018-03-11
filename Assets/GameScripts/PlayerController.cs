using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentPlayer = 1;

    void Start()
    {
        Player p = new Player();
        p.id = "Player 1";
        Color pieceColor = new Color();
        ColorUtility.TryParseHtmlString("#FFFFFFDC", out pieceColor);
        p.pieceColor = pieceColor;
        p.pieceMaterialName = "Piece-White";
        Managers.Player.SetPlayer(1, p);
        p = new Player();
        p.id = "Player 2";
        ColorUtility.TryParseHtmlString("#000000B4", out pieceColor);
        p.pieceColor = pieceColor;
        p.pieceMaterialName = "Piece-Black";
        Managers.Player.SetPlayer(2, p);
    }
}
