using System;
using System.Collections.Generic;
using UnityEngine;

public static class BoardChecker
{
    public static Match[] FindMatches(Piece[][][] board)
    {
        List<Match> matches = new List<Match>();
        for (int i = 0; i < 3; i++)
        {
            Piece[][] layer = board[i];
            Piece[][] transLayer = transposeLayer(layer);
            for (int j = 0; j < 3; j++)
            {
                //Check vertically
                Piece[] row = layer[j];
                PieceType type = checkRow(row, 1);
                if (type != PieceType.NONE)
                {
                    Match match = new Match();
                    match.pieceType = type;
                    match.playerNum = 1;
                    match.coordinates = new List<BoardCoordinates> { new BoardCoordinates(i, j, 0), new BoardCoordinates(i, j, 1), new BoardCoordinates(i, j, 2) };
                    matches.Add(match);
                }
                type = checkRow(row, 2);
                if (type != PieceType.NONE)
                {
                    Match match = new Match();
                    match.pieceType = type;
                    match.playerNum = 2;
                    match.coordinates = new List<BoardCoordinates> { new BoardCoordinates(i, j, 0), new BoardCoordinates(i, j, 1), new BoardCoordinates(i, j, 2) };
                    matches.Add(match);
                }
                // check horizontally
                row = transLayer[j];
                type = checkRow(row, 1);
                if (type != PieceType.NONE)
                {
                    Match match = new Match();
                    match.pieceType = type;
                    match.playerNum = 1;
                    match.coordinates = new List<BoardCoordinates> { new BoardCoordinates(i, 0, j), new BoardCoordinates(i, 0, j), new BoardCoordinates(i, 0, j) };
                    matches.Add(match);
                }
                type = checkRow(row, 2);
                if (type != PieceType.NONE)
                {
                    Match match = new Match();
                    match.pieceType = type;
                    match.playerNum = 2;
                    match.coordinates = new List<BoardCoordinates> { new BoardCoordinates(i, 0, j), new BoardCoordinates(i, 0, j), new BoardCoordinates(i, 0, j) };
                    matches.Add(match);
                }
            }
        }
        // check between layers
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Piece[] row = { board[0][i][j], board[1][i][j], board[2][i][j] };
                PieceType type = checkRow(row, 1);
                if (type != PieceType.NONE)
                {
                    Match match = new Match();
                    match.pieceType = type;
                    match.playerNum = 1;
                    match.coordinates = new List<BoardCoordinates> { new BoardCoordinates(0, 1, j), new BoardCoordinates(1, i, j), new BoardCoordinates(2, i, j) };
                    matches.Add(match);
                }
                type = checkRow(row, 2);
                if (type != PieceType.NONE)
                {
                    Match match = new Match();
                    match.pieceType = type;
                    match.playerNum = 2;
                    match.coordinates = new List<BoardCoordinates> { new BoardCoordinates(0, 1, j), new BoardCoordinates(1, i, j), new BoardCoordinates(2, i, j) };
                    matches.Add(match);
                }
            }
        }
        return matches.ToArray();
    }

    private static Piece[][] transposeLayer(Piece[][] layer)
    {
        Piece[][] newLayer = { new Piece[layer[0].Length], new Piece[layer[0].Length], new Piece[layer[0].Length] };
        for (int i = 0; i < layer.Length; i++)
        {
            for (int j = 0; j < layer[i].Length; j++)
            {
                newLayer[i][j] = layer[j][i];
            }
        }
        return newLayer;
    }

    private static PieceType checkRow(Piece[] row, int playerNum)
    {
        if (Array.IndexOf(row, null) != -1)
        {
            // Debug.Log("Null Piece");
            return PieceType.NONE;
        }
        if (Array.Find(row, p => p.playerNum != playerNum) != null)
        {
            // Debug.Log("MisMatched Players");
            return PieceType.NONE;
        }
        PieceType type = Array.Find(row, p => p.type != PieceType.WILD).type;
        if (Array.Find(row, p => (p.type != type && p.type != PieceType.WILD)) != null)
        {
            // Debug.Log("Pieces Dont Match");
            return PieceType.NONE;
        }
        // Debug.Log("Found Cross Board!");
        return type;
    }
}