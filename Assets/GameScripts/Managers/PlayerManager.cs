using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    private Player[] players;
    public ManagerStatus status
    {
        get; private set;
    }

    public void Startup()
    {
        Debug.Log("Starting Player Manager");
        players = new Player[] { null, null };
        status = ManagerStatus.Started;
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

    public IReadOnlyDictionary<PieceType, int> GetPieceBank(int playerNum)
    {
        if (playerNum != 1 && playerNum != 2)
        {
            Debug.LogWarning("Player Does Not Exist!");
            return null;
        }
        return players[playerNum - 1].bank.ToReadOnlyDictionary();
    }
}