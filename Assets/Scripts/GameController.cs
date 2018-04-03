using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    protected PlayerController playerController;
    protected List<Move> _moves;
    public bool gameOver { get; protected set; }
    public ManagerStatus status
    {
        get; private set;
    }

    public void Awake()
    {
        gameOver = false;
        _moves = new List<Move>();
    }

    public void Start()
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
        p2.id = "Player 2";
        ColorUtility.TryParseHtmlString("#000000B4", out pieceColor);
        p2.pieceColor = pieceColor;
        p2.pieceMaterialName = "Piece-Black";
        p2.isLocal = true;
        playerController.SetPlayer(2, p2);
        playerController.SetActivePlayer(1);
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
            board = AddToBoardState(board, move);
        }
        return board;
    }

    public Piece[][][] AddToBoardState(Piece[][][] board, Move move)
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
}