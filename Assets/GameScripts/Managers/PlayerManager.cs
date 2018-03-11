using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager
{
    public int currentPlayer { get; private set; }
    private Player[] players;
    public ManagerStatus status
    {
        get; private set;
    }

    public void Startup()
    {
        Debug.Log("Starting Player Manager");
        players = new Player[] { null, null };
        currentPlayer = 1;
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
            Messenger<int>.Broadcast(GameEvent.ACTIVE_PLAYER_CHANGED, Managers.Player.currentPlayer);
        }
    }

    public void SwitchActivePlayer()
    {
        currentPlayer = (currentPlayer % 2) + 1;
        Messenger<int>.Broadcast(GameEvent.ACTIVE_PLAYER_CHANGED, Managers.Player.currentPlayer);
    }
}