using System;
using System.Collections.Generic;
using UnityEngine;

public static class BoardStateUtils
{
    public static Piece[][][] GenerateBoardState(List<Move> moves)
    {
        //initialize empty board
        Piece[][][] board = new Piece[3][][];
        for (int i = 0; i < board.Length; i++)
        {
            board[i] = new Piece[3][];
            for (int j = 0; j < board[i].Length; j++)
            {
                board[i][j] = new Piece[3];
            }
        }
        //replay moves over top of board
        foreach (Move move in moves)
        {
            board = AddToBoardState(board, move);
        }
        return board;
    }

    private static Piece[][][] AddToBoardState(Piece[][][] board, Move move)
    {
        Piece p = new Piece();
        if (move.from == null)
        {
            p.type = move.pieceType;
            p.playerNum = move.playerNum;
        }
        else
        {
            p = board[move.from.z][move.from.x][move.from.y];
            board[move.from.z][move.from.x][move.from.y] = null;
        }
        board[move.to.z][move.to.x][move.to.y] = p;
        return board;
    }

    public static List<Move> EnumerateAvailableMoves(List<Move> playedMoves)
    {
        List<Move> enumeratedMoves = new List<Move>();
        int currentPlayer = (playedMoves[playedMoves.Count - 1].playerNum % 2) + 1;
        Player[] players = getPieceBankFromMoves(playedMoves);
        List<Vector3Int> vacantSpaces = findVacantBoardSpaces(playedMoves);
        List<Vector3Int> ownedSpaces = findOwnedBoardSpaces(playedMoves, currentPlayer);
        bool bankEmpty = true;
        foreach (PieceType key in players[currentPlayer - 1].bank.Keys)
        {
            if (key == PieceType.NONE)
            {
                continue;
            }
            if (players[currentPlayer - 1].bank[key] > 0)
            {
                foreach (Vector3Int space in vacantSpaces)
                {
                    Move move = new Move();
                    move.playerNum = currentPlayer;
                    move.from = null;
                    move.pieceType = key;
                    move.to = space;
                    enumeratedMoves.Add(move);
                }
                bankEmpty = false;
            }
        }
        if (bankEmpty)
        {
            foreach (Vector3Int ownedSpace in ownedSpaces)
            {
                foreach (Vector3Int emptySpace in vacantSpaces)
                {
                    Move move = new Move();
                    move.playerNum = currentPlayer;
                    move.from = ownedSpace;
                    move.to = emptySpace;
                    enumeratedMoves.Add(move);
                }
            }
        }
        return enumeratedMoves;
    }

    private static List<Vector3Int> findVacantBoardSpaces(List<Move> playedMoves)
    {
        List<Vector3Int> spaces = new List<Vector3Int>();
        Piece[][][] board = GenerateBoardState(playedMoves);
        for (int z = 0; z < 3; z++)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board[z][x][y] == null)
                    {
                        spaces.Add(new Vector3Int(x, y, z));
                    }
                }
            }
        }
        return spaces;
    }

    private static List<Vector3Int> findOwnedBoardSpaces(List<Move> playedMoves, int owner)
    {
        List<Vector3Int> spaces = new List<Vector3Int>();
        Piece[][][] board = GenerateBoardState(playedMoves);
        for (int z = 0; z < 3; z++)
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board[z][x][y] != null && board[z][x][y].playerNum == owner)
                    {
                        spaces.Add(new Vector3Int(x, y, z));
                    }
                }
            }
        }
        return spaces;
    }

    private static Player[] getPieceBankFromMoves(List<Move> playedMoves)
    {
        Player[] players = new Player[] { new Player(), new Player() };
        foreach (Move move in playedMoves)
        {
            if (move.from == null)
            {
                players[move.playerNum - 1].bank[move.pieceType] -= 1;
            }
        }
        return players;
    }
}