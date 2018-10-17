using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Piece[][][] b = BoardStateUtils.GenerateBoardState(_moves);

        Match[] matches = BoardChecker.FindMatches(b);
        foreach (Match match in matches)
        {
            Debug.Log("Match Found For " + playerNum + " " + match.ToString());
        }
        Messenger<Match[]>.Broadcast(GameEvent.PIECES_MATCHED, matches);
        Match[] currentPlayerMatches = matches.Where(match => match.playerNum == playerNum).ToArray();
        if (currentPlayerMatches.Length == 3)
        {
            Debug.Log("All Pieces Matched!");
            gameOver = true;
            Messenger<int>.Broadcast(GameEvent.GAME_OVER, playerNum);
        }
    }
}