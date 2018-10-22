using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class OnlineGameController : GameController
{
    private string gameId;
    private PublicGame game;

    private bool syncing;

    private int localPlayerNum;

    new void Awake()
    {
        gameOver = false;
        _moves = new List<Move>();
        Messenger<Move>.AddListener(GameEvent.MOVE_APPLIED, SyncMove);
    }
    void OnDestroy()
    {
        Messenger<Move>.RemoveListener(GameEvent.MOVE_APPLIED, SyncMove);
    }

    public new void Start()
    {
        gameId = Managers.GameMode.modeParams["game-id"];
        localPlayerNum = Convert.ToInt32(Managers.GameMode.modeParams["local-player"]);
        Debug.Log(gameId);
        Player p = new Player();
        if (localPlayerNum == 1)
        {
            p.id = "You";
        }
        else
        {
            p.id = "Opponent";
        }
        Color pieceColor = new Color();
        ColorUtility.TryParseHtmlString("#FFFFFFDC", out pieceColor);
        p.pieceColor = pieceColor;
        p.pieceMaterialName = "Piece-White";
        p.isLocal = Managers.GameMode.modeParams["local-player"] == "1";
        playerController.SetPlayer(1, p);
        Player p2 = new Player();
        if (localPlayerNum == 2)
        {
            p2.id = "You";
        }
        else
        {
            p2.id = "Opponent";
        }
        ColorUtility.TryParseHtmlString("#000000B4", out pieceColor);
        p2.pieceColor = pieceColor;
        p2.pieceMaterialName = "Piece-Black";
        p2.isLocal = Managers.GameMode.modeParams["local-player"] == "2";
        playerController.SetPlayer(2, p2);
        playerController.SetActivePlayer(1);
        Managers.Online.SetMoveHandler(MoveAdded);
        Managers.Online.SetForfeitHandler(ExitGame);
    }

    public void LeaveGame()
    {
        Managers.Online.ForfeitGame(gameId);
    }

    private void ExitGame()
    {
        Managers.GameMode.StartGame(GameMode.NONE);
    }

    private void SyncMove(Move move)
    {
        if (move.playerNum != localPlayerNum)
        {
            return;
        }
        Managers.Online.MakeMove(gameId, move);
    }

    private void MoveAdded(Move[] newMoves)
    {
        while (_moves.Count < newMoves.Length)
        {
            Debug.Log("Catching Up");
            ApplyMove(newMoves[_moves.Count]);
        }
    }


}