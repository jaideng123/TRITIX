using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentPlayer = 1;
    private bool initialized = false;

    void Start()
    {
    }

    void Update()
    {
        if (!initialized)
        {
            Player p = new Player();
            p.id = "Player 1";
            Color pieceColor = new Color();
            ColorUtility.TryParseHtmlString("#FFFFFFDC", out pieceColor);
            p.pieceColor = pieceColor;
            p.pieceMaterialName = "Piece-White";
            Managers.Player.SetPlayer(1, p);
            Player p2 = new Player();
            p2.id = "Player 2";
            ColorUtility.TryParseHtmlString("#000000B4", out pieceColor);
            p2.pieceColor = pieceColor;
            p2.pieceMaterialName = "Piece-Black";
            Managers.Player.SetPlayer(2, p2);
            initialized = true;
        }
    }
}
