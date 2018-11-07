using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
public class AIGameController : GameController
{
    [SerializeField]
    private int minimaxDepth = 2;
    [SerializeField]
    private bool useRandom = false;
    private bool planning = false;

    private bool _threadRunning;
    private Thread _thread;
    private Move plannedMove = null;
    public new void Start()
    {
        Player p = new Player();
        p.id = "Player 1";
        Color pieceColor = new Color();
        ColorUtility.TryParseHtmlString("#FFFFFFDC", out pieceColor);
        p.pieceColor = pieceColor;
        p.pieceMaterialName = "Piece-White";
        p.isLocal = true;
        playerController.SetPlayer(1, p);
        Player p2 = new Player();
        p2.id = "Computer";
        ColorUtility.TryParseHtmlString("#000000B4", out pieceColor);
        p2.pieceColor = pieceColor;
        p2.pieceMaterialName = "Piece-Black";
        p2.isLocal = false;
        playerController.SetPlayer(2, p2);
        playerController.SetActivePlayer(1);

        switch (Managers.GameMode.currentGameMode)
        {
            case (GameMode.COM_EASY):
                useRandom = true;
                break;
            case (GameMode.COM_MEDIUM):
                minimaxDepth = 0;
                break;
            case (GameMode.COM_HARD):
                minimaxDepth = 1;
                break;
            default:
                break;
        }
    }

    public void Update()
    {
        if (plannedMove != null && !planning)
        {
            ApplyMove(plannedMove);
            plannedMove = null;
        }
        if (playerController.currentPlayer == 2 && !planning && plannedMove == null && !gameOver)
        {
            planning = true;
            if (useRandom)
            {
                _thread = new Thread(PlanRandomMove);
            }
            else
            {
                _thread = new Thread(PlanMove);
            }
            _thread.Start();
        }
    }

    private void PlanMove()
    {
        Debug.Log("Planning Move");
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Move bestMove = null;
        int bestValue = -9999;
        List<Move> possibleMoves = BoardStateUtils.EnumerateAvailableMoves(_moves);
        List<Move> shuffledMoves = Shuffle.FisherYatesCardDeckShuffle<Move>(possibleMoves);
        foreach (Move move in shuffledMoves)
        {
            List<Move> moves = new List<Move>(_moves);
            moves.Add(move);
            int value = MiniMax.minimax(minimaxDepth, moves, -1000, 1000, false);
            if (value > bestValue)
            {
                bestMove = move;
                bestValue = value;
            }
        }
        watch.Stop();
        Debug.Log("Time Taken: " + watch.ElapsedMilliseconds);
        if (watch.Elapsed.Milliseconds < 1000)
        {
            int delay = 1000 - watch.Elapsed.Milliseconds;
            Thread.Sleep(delay);
        }
        plannedMove = bestMove;
        planning = false;
    }

    private void PlanRandomMove()
    {
        Debug.Log("Planning Random Move");
        var watch = System.Diagnostics.Stopwatch.StartNew();
        List<Move> possibleMoves = BoardStateUtils.EnumerateAvailableMoves(_moves);
        List<Move> shuffledMoves = Shuffle.FisherYatesCardDeckShuffle<Move>(possibleMoves);
        plannedMove = shuffledMoves[0];
        watch.Stop();
        Debug.Log("Time Taken: " + watch.Elapsed.Milliseconds);
        if (watch.Elapsed.Milliseconds < 1000)
        {
            int delay = 1000 - watch.Elapsed.Milliseconds;
            Thread.Sleep(delay);
        }
        planning = false;
        return;
    }
}