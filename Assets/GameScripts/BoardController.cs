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
    }
    void OnDestroy()
    {
        Messenger<Vector3Int>.RemoveListener(GameEvent.SPACE_SELECTED, OnSpaceSelected);
        Messenger<PieceType>.RemoveListener(GameEvent.PIECE_SELECTED, OnPieceSelected);
    }
    void Start()
    {
        board = boardObject.GetComponent<Board>();
        moves = new List<Move>();
        Player p = new Player();
        p.id = "Jaiden";
        p.pieceMaterialName = "Piece-White";
        Managers.Player.SetPlayer(1, p);
        p = new Player();
        p.id = "Sydney";
        p.pieceMaterialName = "Piece-Black";
        Managers.Player.SetPlayer(2, p);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnSpaceSelected(Vector3Int coordinates)
    {
        if (selectedDestSpace != null)
        {
            board.GetSpace(selectedDestSpace).SetActive(false);
        }
        selectedDestSpace = coordinates;
        board.GetSpace(selectedDestSpace).SetActive(true);
        Messenger<bool>.Broadcast(GameEvent.TOGGLE_PIECE_DRAWER, true);
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
        Messenger<bool>.Broadcast(GameEvent.TOGGLE_PIECE_DRAWER, false);
        board.GetSpace(selectedDestSpace).SetActive(false);
        selectedDestSpace = null;
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
    }
}
