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
        if (plannedMove != null && !planning)
        {
            ApplyMove(plannedMove);
            plannedMove = null;
        }
        if (playerController.currentPlayer == 2 && !planning && plannedMove == null)
        {
            Debug.Log("Lets Start");
            planning = true;
            _thread = new Thread(PlanMove);
            _thread.Start();
        }
    }

    private void PlanMove()
    {
        Debug.Log("Planning Move");
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Move bestMove = null;
        int bestValue = -9999;
        List<Move> shuffledMoves = FisherYatesCardDeckShuffle(enumerateAvailableMoves(_moves));
        if (useRandom)
        {
            plannedMove = shuffledMoves[0];
            planning = false;
            watch.Stop();
            Debug.Log("Time Taken: " + watch.ElapsedMilliseconds);
            return;
        }
        foreach (Move move in shuffledMoves)
        {
            List<Move> moves = new List<Move>(_moves);
            moves.Add(move);
            int value = minimax(minimaxDepth, moves, -1000, 1000, false);
            if (value > bestValue)
            {
                Debug.Log(value);
                Debug.Log(move);

                bestMove = move;
                bestValue = value;
            }
        }
        watch.Stop();
        Debug.Log("Time Taken: " + watch.ElapsedMilliseconds);
        plannedMove = bestMove;
        planning = false;
    }

    private int minimax(int depth, List<Move> movesPlayed, int alpha, int beta, bool isMaximisingPlayer)
    {
        if (depth == 0)
        {
            return -GetBoardValue(GetBoardState(movesPlayed));
        }
        List<Move> shuffledMoves = enumerateAvailableMoves(movesPlayed);
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
    public static List<Move> FisherYatesCardDeckShuffle(List<Move> aList)
    {

        System.Random _random = new System.Random();

        Move myGO;

        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = aList[r];
            aList[r] = aList[i];
            aList[i] = myGO;
        }

        return aList;
    }

    private int GetBoardValue(Piece[][][] board)
    {
        int p1value = 0;
        foreach (PieceType match in BoardChecker.FindMatches(board, 1))
        {
            p1value += 3;
        }
        int p2value = 0;
        foreach (PieceType match in BoardChecker.FindMatches(board, 2))
        {
            p2value += 3;
        }
        p2value = p2value == 9 ? 100 : p2value;
        p1value = p1value == 9 ? 100 : p1value;
        return p1value - p2value;
    }

    private List<Move> enumerateAvailableMoves(List<Move> playedMoves)
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

    private List<Vector3Int> findVacantBoardSpaces(List<Move> playedMoves)
    {
        List<Vector3Int> spaces = new List<Vector3Int>();
        Piece[][][] board = GetBoardState(playedMoves);
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

    private List<Vector3Int> findOwnedBoardSpaces(List<Move> playedMoves, int owner)
    {
        List<Vector3Int> spaces = new List<Vector3Int>();
        Piece[][][] board = GetBoardState(playedMoves);
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

    private Player[] getPieceBankFromMoves(List<Move> playedMoves)
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