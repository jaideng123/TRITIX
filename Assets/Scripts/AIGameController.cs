using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AIGameController : GameController
{
    private bool planning = false;
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
        p2.id = "Computer";
        ColorUtility.TryParseHtmlString("#000000B4", out pieceColor);
        p2.pieceColor = pieceColor;
        p2.pieceMaterialName = "Piece-Black";
        p2.isLocal = false;
        playerController.SetPlayer(2, p2);
        playerController.SetActivePlayer(1);
    }

    public void Update()
    {
        if (playerController.currentPlayer == 2 && !planning)
        {
            planning = true;
            StartCoroutine(PlanMove());
        }
    }

    private IEnumerator PlanMove()
    {
        yield return new WaitForSeconds(.5f);
        ApplyMove(createRandomMove());
        planning = false;
    }

    private Move createRandomMove()
    {
        Debug.LogWarning("Creating Random Move");
        Piece[][][] board = GetBoardState(_moves);
        if (!playerController.PieceBankEmpty(playerController.currentPlayer))
        {
            // Find Random Piece that is not used up
            Dictionary<PieceType, int> bank = playerController.GetPieceBank(2);
            PieceType selectedPiece = PieceType.NONE;
            System.Random random = new System.Random();
            Array values = Enum.GetValues(typeof(PieceType));
            while (selectedPiece == PieceType.NONE || bank[selectedPiece] == 0)
            {
                selectedPiece = (PieceType)values.GetValue(random.Next(values.Length));
            }
            // find random space on board
            Vector3Int to = new Vector3Int(random.Next(3), random.Next(3), random.Next(3));
            while (board[to.z][to.x][to.y] != null)
            {
                to = new Vector3Int(random.Next(3), random.Next(3), random.Next(3));
            }
            Debug.Log(to);
            foreach (Move m in _moves)
            {
                Debug.Log(m);
            }
            for (int x = 0; x < board.Length; x++)
            {
                for (int y = 0; y < board[x].Length; y++)
                {
                    for (int z = 0; z < board[x][y].Length; z++)
                    {
                        Debug.Log("[" + x + "," + y + "," + z + "] : " + board[x][y][z]);
                    }
                }
            }
            Debug.Log(board[to.z][to.x][to.y]);
            Move move = new Move();
            move.playerNum = playerController.currentPlayer;
            move.to = to;
            move.pieceType = selectedPiece;
            return move;
        }
        else
        {
            System.Random random = new System.Random();

            Vector3Int from = new Vector3Int(random.Next(3), random.Next(3), random.Next(3));
            while (board[from.z][from.x][from.y] == null || board[from.z][from.x][from.y].playerNum != playerController.currentPlayer)
            {
                from = new Vector3Int(random.Next(3), random.Next(3), random.Next(3));
            }

            Vector3Int to = new Vector3Int(random.Next(3), random.Next(3), random.Next(3));
            while (board[to.z][to.x][to.y] != null)
            {
                to = new Vector3Int(random.Next(3), random.Next(3), random.Next(3));
            }
            Move move = new Move();
            move.playerNum = playerController.currentPlayer;
            move.to = to;
            move.from = from;
            return move;
        }
    }
}