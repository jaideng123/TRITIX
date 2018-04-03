using System.Collections.Generic;
using System.Collections.Specialized;

public class BoardState
{
    public int activePlayer;
    public Piece[][][] board;

    BoardState()
    {
        activePlayer = 1;
        board = GetEmptyBoard();
    }
    BoardState(List<Move> moves)
    {
        activePlayer = 1;
        board = GetEmptyBoard();
        foreach (Move move in moves)
        {
            ApplyMove(move);
        }
    }



    public void ApplyMove(Move move)
    {
        board = AddToBoard(board, move);
        activePlayer = (move.playerNum % 2) + 1;
    }

    private Piece[][][] AddToBoard(Piece[][][] original, Move move)
    {
        Piece p = new Piece();
        if (move.from == null)
        {
            p.type = move.pieceType;
            p.playerNum = move.playerNum;
        }
        else
        {
            p = original[move.from.z][move.from.x][move.from.y];
            original[move.from.z][move.from.x][move.from.y] = null;
        }
        original[move.to.z][move.to.x][move.to.y] = p;
        return original;
    }

    private Piece[][][] GetEmptyBoard()
    {
        Piece[][][] empty = new Piece[3][][];
        for (int i = 0; i < empty.Length; i++)
        {
            empty[i] = new Piece[3][];
            for (int j = 0; j < empty[i].Length; j++)
            {
                empty[i][j] = null;
            }
        }
        return empty;
    }
}