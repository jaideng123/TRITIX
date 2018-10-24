using System.Collections.Generic;
using System.Collections.Specialized;
//Not currently being used ( but i want to keep it around just in case)
public class BoardState
{
    public int activePlayer;
    public Piece[][][] board;

    public BoardState()
    {
        activePlayer = 1;
        board = GetEmptyBoard();
    }
    public BoardState(List<Move> moves)
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
            p = original[move.from.layer][move.from.column][move.from.row];
            original[move.from.layer][move.from.column][move.from.row] = null;
        }
        original[move.to.layer][move.to.column][move.to.row] = p;
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