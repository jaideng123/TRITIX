using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    [SerializeField]
    private GameObject boardObject;
    private Board board;
    private Vector3Int selectedDestSpace;
    private Vector3Int selectedOriginSpace;
    private int currentPlayer = 1;
    private List<Move> moves;
    // Use this for initialization
    void Awake()
    {
        Messenger<Vector3Int>.AddListener(GameEvent.SPACE_SELECTED, OnSpaceSelected);
        Messenger<PieceType>.AddListener(GameEvent.PIECE_SELECTED, OnPieceSelected);
        Messenger.AddListener(GameEvent.MOVE_CONFIRMED, OnMoveConfirmed);
        Messenger.AddListener(GameEvent.ALL_MANAGERS_STARTED, OnManagersStarted);
    }
    void OnDestroy()
    {
        Messenger<Vector3Int>.RemoveListener(GameEvent.SPACE_SELECTED, OnSpaceSelected);
        Messenger<PieceType>.RemoveListener(GameEvent.PIECE_SELECTED, OnPieceSelected);
        Messenger.RemoveListener(GameEvent.MOVE_CONFIRMED, OnMoveConfirmed);
        Messenger.RemoveListener(GameEvent.ALL_MANAGERS_STARTED, OnManagersStarted);
    }
    void Start()
    {
        board = boardObject.GetComponent<Board>();
        moves = new List<Move>();
        Player p = new Player();
        p.id = "Player 1";
        p.pieceMaterialName = "Piece-White";
        Managers.Player.SetPlayer(1, p);
        p = new Player();
        p.id = "Player 2";
        p.pieceMaterialName = "Piece-Black";
        Managers.Player.SetPlayer(2, p);
    }

    void OnManagersStarted()
    {
        Messenger<int>.Broadcast(GameEvent.ACTIVE_PLAYER_CHANGED, currentPlayer);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSpaceSelected(Vector3Int coordinates)
    {
        Space selectedSpace = board.GetSpace(coordinates);
        if (Managers.Player.PieceBankEmpty(currentPlayer))
        {
            if (selectedSpace.piece == null)
            {
                if (selectedDestSpace != null)
                {
                    board.GetSpace(selectedDestSpace).SetActive(false);
                    board.GetSpace(selectedDestSpace).ClearPieceTemp();
                }
                selectedDestSpace = coordinates;
                selectedSpace.SetActive(true);
            }
            else
            {
                if (selectedSpace.piece.playerNum != currentPlayer)
                {
                    return;
                }
                if (selectedOriginSpace != null)
                {
                    board.GetSpace(selectedOriginSpace).SetActive(false);
                }
                selectedOriginSpace = coordinates;
                selectedSpace.SetActive(true);
            }
            if (selectedDestSpace != null && selectedOriginSpace != null)
            {
                PieceType tempType = board.GetSpace(selectedOriginSpace).piece.type;
                board.GetSpace(selectedDestSpace).ApplyPieceTemp(tempType);
                Messenger<bool>.Broadcast(GameEvent.TOGGLE_CONFIRM_DRAWER, true);
            }
        }
        else
        {
            if (selectedSpace.piece != null)
            {
                return;
            }
            if (selectedDestSpace != null)
            {
                board.GetSpace(selectedDestSpace).SetActive(false);
            }
            selectedDestSpace = coordinates;
            selectedSpace.SetActive(true);
            Messenger<bool, int>.Broadcast(GameEvent.TOGGLE_PIECE_DRAWER, true, currentPlayer);
        }
    }

    private void OnPieceSelected(PieceType type)
    {
        if (Managers.Player.GetPieceBank(currentPlayer)[type] <= 0)
        {
            Debug.Log("No More Pieces Left Of Type " + type.ToString() + " For Player " + currentPlayer);
            return;
        }
        Debug.Log(type.ToString());
        Move move = new Move();
        move.pieceType = type;
        move.from = null;
        move.to = selectedDestSpace;
        move.playerNum = currentPlayer;
        ApplyMove(move);
        Messenger<bool, int>.Broadcast(GameEvent.TOGGLE_PIECE_DRAWER, false, currentPlayer);
        board.GetSpace(selectedDestSpace).SetActive(false);
        selectedDestSpace = null;
    }

    private void OnMoveConfirmed()
    {
        if (selectedDestSpace == null || selectedOriginSpace == null)
        {
            Debug.LogWarning("Destination and Origin not selected");
            return;
        }
        Move move = new Move();
        move.from = selectedOriginSpace;
        move.to = selectedDestSpace;
        move.playerNum = currentPlayer;
        ApplyMove(move);

        Messenger<bool>.Broadcast(GameEvent.TOGGLE_CONFIRM_DRAWER, false);
        board.GetSpace(selectedDestSpace).SetActive(false);
        board.GetSpace(selectedOriginSpace).SetActive(false);
        board.GetSpace(selectedDestSpace).ClearPieceTemp();
        selectedDestSpace = null;
        selectedOriginSpace = null;
    }
    public void CheckMatches(int playerNum)
    {
        Piece[][][] b = board.GetBoardModel();
        HashSet<PieceType> matches = new HashSet<PieceType>();
        Debug.Log("Checking For " + playerNum);
        for (int i = 0; i < 3; i++)
        {
            //Check vertically
            Piece[][] layer = b[i];
            for (int j = 0; j < 3; j++)
            {
                Debug.Log("Checking Column " + j);
                if (Array.IndexOf(layer[j], null) != -1)
                {
                    Debug.Log("Null Piece");
                    continue;
                }
                if (Array.Find(layer[j], p => p.playerNum != playerNum) != null)
                {
                    Debug.Log("MisMatched Players");
                    continue;
                }
                PieceType type = Array.Find(layer[j], p => p.type != PieceType.WILD).type;
                if (Array.Find(layer[j], p => (p.type != type && p.type != PieceType.WILD)) != null)
                {
                    Debug.Log("Pieces Dont Match");
                    continue;
                }
                Debug.Log("Found Vertically!");
                matches.Add(type);
            }
            // check horizontally
            layer = transposeLayer(layer);
            for (int j = 0; j < 3; j++)
            {
                Debug.Log("Checking Row " + j);
                if (Array.IndexOf(layer[j], null) != -1)
                {
                    Debug.Log("Null Piece");
                    continue;
                }
                if (Array.Find(layer[j], p => p.playerNum != playerNum) != null)
                {
                    Debug.Log("MisMatched Players");
                    continue;
                }
                PieceType type = Array.Find(layer[j], p => p.type != PieceType.WILD).type;
                if (Array.Find(layer[j], p => (p.type != type && p.type != PieceType.WILD)) != null)
                {
                    Debug.Log("Pieces Dont Match");
                    continue;
                }
                Debug.Log("Found Horizontally!");
                matches.Add(type);
            }
        }

        PieceType[] matchArray = new PieceType[matches.Count];
        matches.CopyTo(matchArray);
        foreach (PieceType match in matchArray)
        {
            Debug.Log("Match Found For " + playerNum + " " + match.ToString());
        }
        Messenger<PieceType[], int>.Broadcast(GameEvent.PIECES_MATCHED, matchArray, playerNum);
    }

    private Piece[][] transposeLayer(Piece[][] layer)
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


    public void ApplyMove(Move move)
    {
        if (move.playerNum != currentPlayer)
        {
            Debug.LogWarning("currentPlayer, playernum mismatch");
        }
        Piece p;
        if (move.from == null)
        {
            p = Managers.Player.GetPieceFromBank(move.playerNum, move.pieceType);
        }
        else
        {
            p = board.GetSpace(move.from).piece;
            board.GetSpace(move.from).ClearPiece();
        }
        board.GetSpace(move.to).ApplyPiece(p);
        currentPlayer = (currentPlayer % 2) + 1;
        moves.Add(move);
        CheckMatches(move.playerNum);
        Messenger<int>.Broadcast(GameEvent.ACTIVE_PLAYER_CHANGED, currentPlayer);
    }
}
