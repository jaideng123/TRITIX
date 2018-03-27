using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;
    private List<Move> _moves;
    public bool gameOver { get; private set; }
    public ManagerStatus status
    {
        get; private set;
    }

    public void Awake()
    {
        gameOver = false;
        _moves = new List<Move>();
    }

    public void ApplyMove(Move move)
    {
        Messenger<Move>.Broadcast(GameEvent.MOVE_APPLIED, move);
        _moves.Add(move);
        CheckMatches(move.playerNum);
        if (!gameOver)
        {
            playerController.SwitchActivePlayer();
        }
    }

    public void CheckMatches(int playerNum)
    {
        Piece[][][] b = GetBoardState(_moves);
        PieceType[] matchArray = BoardChecker.FindMatches(b, playerNum);
        foreach (PieceType match in matchArray)
        {
            Debug.Log("Match Found For " + playerNum + " " + match.ToString());
        }
        Messenger<PieceType[], int>.Broadcast(GameEvent.PIECES_MATCHED, matchArray, playerNum);
        if (matchArray.Length == 3)
        {
            Debug.Log("All Pieces Matched!");
            gameOver = true;
            Messenger<int>.Broadcast(GameEvent.GAME_OVER, playerNum);
        }
    }

    public Piece[][][] GetBoardState(List<Move> moves)
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
        }
        return board;
    }
}