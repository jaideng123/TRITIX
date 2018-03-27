using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int currentPlayer { get; private set; }
    private Player[] players;
    private bool initialized = false;


    void Start()
    {
        players = new Player[] { null, null };
        Player p = new Player();
        p.id = "Player 1";
        Color pieceColor = new Color();
        ColorUtility.TryParseHtmlString("#FFFFFFDC", out pieceColor);
        p.pieceColor = pieceColor;
        p.pieceMaterialName = "Piece-White";
        SetPlayer(1, p);
        Player p2 = new Player();
        p2.id = "Player 2";
        ColorUtility.TryParseHtmlString("#000000B4", out pieceColor);
        p2.pieceColor = pieceColor;
        p2.pieceMaterialName = "Piece-Black";
        SetPlayer(2, p2);
        SetActivePlayer(1);
    }

    public void SetPlayer(int playerNum, Player player)
    {
        if (playerNum != 1 && playerNum != 2)
        {
            Debug.LogWarning("Player Does Not Exist!");
            return;
        }
        if (players[playerNum - 1] != null)
        {
            Debug.LogWarning("Player Already Set");
        }
        players[playerNum - 1] = player;
        Messenger<int>.Broadcast(GameEvent.PLAYER_INFO_CHANGED, playerNum);
        Debug.Log("Player " + player.id + " Registered");
    }

    public Player GetPlayer(int playerNum)
    {
        if (playerNum != 1 && playerNum != 2)
        {
            Debug.LogWarning("Player Does Not Exist!");
            return null;
        }
        return players[playerNum - 1];
    }

    public Piece GetPieceFromBank(int playerNum, PieceType type)
    {
        if (playerNum != 1 && playerNum != 2)
        {
            Debug.LogWarning("Player Does Not Exist!");
            return null;
        }
        if (players[playerNum - 1].bank[type] < 1)
        {
            Debug.LogWarning("Bank Is Out Of " + type.ToString());
            return null;
        }
        Piece piece = new Piece();
        piece.type = type;
        piece.playerNum = playerNum;
        players[playerNum - 1].bank[type]--;
        return piece;
    }

    public Dictionary<PieceType, int> GetPieceBank(int playerNum)
    {
        if (playerNum != 1 && playerNum != 2)
        {
            Debug.LogWarning("Player Does Not Exist!");
            return null;
        }
        //Copy to a new Dictionary so that we cant mutate the values of the bank
        Dictionary<PieceType, int> dict = new Dictionary<PieceType, int>();
        foreach (PieceType key in players[playerNum - 1].bank.Keys)
        {
            dict.Add(key, players[playerNum - 1].bank[key]);
        }
        return dict;
    }

    public bool PieceBankEmpty(int playerNum)
    {
        if (playerNum != 1 && playerNum != 2)
        {
            Debug.LogWarning("Player Does Not Exist!");
            return false;
        }
        foreach (PieceType key in players[playerNum - 1].bank.Keys)
        {
            if (key == PieceType.NONE)
            {
                continue;
            }
            if (players[playerNum - 1].bank[key] > 0)
            {
                return false;
            }
        }
        return true;
    }

    public void SetActivePlayer(int playerNum)
    {
        if (playerNum > 0 && playerNum <= 2)
        {
            currentPlayer = playerNum;
            Messenger<int>.Broadcast(GameEvent.ACTIVE_PLAYER_CHANGED, currentPlayer, MessengerMode.DONT_REQUIRE_LISTENER);
        }
    }

    public void SwitchActivePlayer()
    {
        currentPlayer = (currentPlayer % 2) + 1;
        Messenger<int>.Broadcast(GameEvent.ACTIVE_PLAYER_CHANGED, currentPlayer);
    }
}
