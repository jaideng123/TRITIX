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

    public void Start()
    {
        gameId = Managers.GameMode.modeParams["game-id"];

        Debug.Log(gameId);
        Player p = new Player();
        p.id = "Player 1";
        Color pieceColor = new Color();
        ColorUtility.TryParseHtmlString("#FFFFFFDC", out pieceColor);
        p.pieceColor = pieceColor;
        p.pieceMaterialName = "Piece-White";
        p.isLocal = Managers.GameMode.modeParams["local-player"] == "1";
        playerController.SetPlayer(1, p);
        Player p2 = new Player();
        p2.id = "Player 2";
        ColorUtility.TryParseHtmlString("#000000B4", out pieceColor);
        p2.pieceColor = pieceColor;
        p2.pieceMaterialName = "Piece-Black";
        p2.isLocal = Managers.GameMode.modeParams["local-player"] == "2";
        playerController.SetPlayer(2, p2);
        playerController.SetActivePlayer(1);
        SyncGame();
    }

    public void Update()
    {
        if (syncing)
        {
            return;
        }
        StartCoroutine(SyncGame());
    }
    private IEnumerator SyncGame()
    {
        syncing = true;
        yield return new WaitForSeconds(1);
        Managers.Online.FindGameById(gameId, UpdateGame);
    }
    private void UpdateGame(PublicGame newGame)
    {
        game = newGame;
        while (_moves.Count < game.moves.Count)
        {
            Debug.Log("Catching Up");
            ApplyMove(game.moves[_moves.Count]);
        }
        if (_moves.Count > game.moves.Count)
        {
            Debug.Log("Sending Moves To Server");
            game.moves = _moves;
            Managers.Online.UpdateGame(game, game => { syncing = false; });
        }
        else
        {
            syncing = false;
        }
    }

}