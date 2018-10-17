using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MiniMax
{
    public static int minimax(int depth, List<Move> movesPlayed, int alpha, int beta, bool isMaximisingPlayer)
    {
        if (depth == 0 || IsTerminalState(BoardStateUtils.GenerateBoardState(movesPlayed)))
        {
            return -GetBoardValue(BoardStateUtils.GenerateBoardState(movesPlayed));
        }
        List<Move> shuffledMoves = BoardStateUtils.EnumerateAvailableMoves(movesPlayed);
        if (isMaximisingPlayer)
        {
            int bestMove = -9999;
            foreach (Move move in shuffledMoves)
            {
                List<Move> newMoves = new List<Move>(movesPlayed);
                newMoves.Add(move);
                bestMove = Math.Max(bestMove, minimax(depth - 1, newMoves, alpha, beta, !isMaximisingPlayer));
                alpha = Math.Max(alpha, bestMove);
                if (beta <= alpha)
                {
                    return bestMove;
                }
            }
            return bestMove;
        }
        else
        {
            int bestMove = 9999;
            foreach (Move move in shuffledMoves)
            {
                List<Move> newMoves = new List<Move>(movesPlayed);
                newMoves.Add(move);
                bestMove = Math.Min(bestMove, minimax(depth - 1, newMoves, alpha, beta, !isMaximisingPlayer));
                beta = Math.Min(beta, bestMove);
                if (beta <= alpha)
                {
                    return bestMove;
                }
            }
            return bestMove;
        }
    }

    private static int GetBoardValue(Piece[][][] board)
    {
        Match[] matches = BoardChecker.FindMatches(board);
        int p1value = matches.Where(match => match.playerNum == 1).Count() * 3;
        int p2value = matches.Where(match => match.playerNum == 2).Count() * 3;
        p2value = p2value == 9 ? 100 : p2value;
        p1value = p1value == 9 ? 100 : p1value;
        return p1value - p2value;
    }

    private static bool IsTerminalState(Piece[][][] board)
    {
        Match[] matches = BoardChecker.FindMatches(board);
        int p1Score = matches.Where(match => match.playerNum == 1).Count();
        int p2Score = matches.Where(match => match.playerNum == 2).Count();

        return (p1Score == 3 || p2Score == 3);
    }
}