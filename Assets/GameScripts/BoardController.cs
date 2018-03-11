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
    private List<Move> appliedMoves;
    // Use this for initialization
    void Awake()
    {
        Messenger<Vector3Int>.AddListener(GameEvent.SPACE_SELECTED, OnSpaceSelected);
        Messenger<PieceType>.AddListener(GameEvent.PIECE_SELECTED, OnPieceSelected);
        Messenger<Move>.AddListener(GameEvent.MOVE_APPLIED, ApplyMove);
        Messenger.AddListener(GameEvent.MOVE_CONFIRMED, OnMoveConfirmed);
        Messenger.AddListener(GameEvent.ALL_MANAGERS_STARTED, OnManagersStarted);
    }
    void OnDestroy()
    {
        Messenger<Vector3Int>.RemoveListener(GameEvent.SPACE_SELECTED, OnSpaceSelected);
        Messenger<PieceType>.RemoveListener(GameEvent.PIECE_SELECTED, OnPieceSelected);
        Messenger<Move>.RemoveListener(GameEvent.MOVE_APPLIED, ApplyMove);
        Messenger.RemoveListener(GameEvent.MOVE_CONFIRMED, OnMoveConfirmed);
        Messenger.RemoveListener(GameEvent.ALL_MANAGERS_STARTED, OnManagersStarted);
    }
    void Start()
    {
        board = boardObject.GetComponent<Board>();
        appliedMoves = new List<Move>();
    }

    void OnManagersStarted()
    {
        Messenger<int>.Broadcast(GameEvent.ACTIVE_PLAYER_CHANGED, Managers.Player.currentPlayer);
    }

    private void OnSpaceSelected(Vector3Int coordinates)
    {
        if (Managers.Board.gameOver)
        {
            return;
        }
        Space selectedSpace = board.GetSpace(coordinates);
        if (Managers.Player.PieceBankEmpty(Managers.Player.currentPlayer))
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
                if (selectedSpace.piece.playerNum != Managers.Player.currentPlayer)
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
            Messenger<bool, int>.Broadcast(GameEvent.TOGGLE_PIECE_DRAWER, true, Managers.Player.currentPlayer);
        }
    }

    private void OnPieceSelected(PieceType type)
    {
        if (Managers.Player.GetPieceBank(Managers.Player.currentPlayer)[type] <= 0)
        {
            Debug.Log("No More Pieces Left Of Type " + type.ToString() + " For Player " + Managers.Player.currentPlayer);
            return;
        }
        Debug.Log(type.ToString());
        Move move = new Move();
        move.pieceType = type;
        move.from = null;
        move.to = selectedDestSpace;
        move.playerNum = Managers.Player.currentPlayer;
        Managers.Board.ApplyMove(move);
        Messenger<bool, int>.Broadcast(GameEvent.TOGGLE_PIECE_DRAWER, false, Managers.Player.currentPlayer);
        CleanUpSelections();
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
        move.playerNum = Managers.Player.currentPlayer;
        Managers.Board.ApplyMove(move);
        Messenger<bool>.Broadcast(GameEvent.TOGGLE_CONFIRM_DRAWER, false);
        CleanUpSelections();
    }

    private void CleanUpSelections()
    {
        if (selectedDestSpace != null)
        {
            board.GetSpace(selectedDestSpace).SetActive(false);
            board.GetSpace(selectedDestSpace).ClearPieceTemp();
        }
        if (selectedOriginSpace != null)
        {
            board.GetSpace(selectedOriginSpace).SetActive(false);
        }
        selectedDestSpace = null;
        selectedOriginSpace = null;
    }

    private void ApplyMove(Move move)
    {
        if (move.playerNum != Managers.Player.currentPlayer)
        {
            Debug.LogWarning("Managers.Player.currentPlayer, playernum mismatch");
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
    }
}
