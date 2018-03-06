using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private HideableDrawer pieceDrawer;
    [SerializeField]
    private HideableDrawer confirmDrawer;
    [SerializeField]
    private PlayerName p1Name;
    [SerializeField]
    private PieceButton[] p1Pieces;
    [SerializeField]
    private PlayerName p2Name;
    [SerializeField]
    private PieceButton[] p2Pieces;

    [SerializeField]
    private PieceButton[] pieceButtons;

    void Awake()
    {
        Messenger<bool, int>.AddListener(GameEvent.TOGGLE_PIECE_DRAWER, OnPieceDrawerToggle);
        Messenger<bool>.AddListener(GameEvent.TOGGLE_CONFIRM_DRAWER, OnConfirmDrawerToggle);
        Messenger<int>.AddListener(GameEvent.ACTIVE_PLAYER_CHANGED, OnActivePlayerChanged);
        Messenger.AddListener(GameEvent.ALL_MANAGERS_STARTED, OnManagersStarted);
        Messenger<PieceType[], int>.AddListener(GameEvent.PIECES_MATCHED, onPiecesMatched);

    }
    void OnDestroy()
    {
        Messenger<bool, int>.RemoveListener(GameEvent.TOGGLE_PIECE_DRAWER, OnPieceDrawerToggle);
        Messenger<bool>.RemoveListener(GameEvent.TOGGLE_CONFIRM_DRAWER, OnConfirmDrawerToggle);
        Messenger<int>.RemoveListener(GameEvent.ACTIVE_PLAYER_CHANGED, OnActivePlayerChanged);
        Messenger.RemoveListener(GameEvent.ALL_MANAGERS_STARTED, OnManagersStarted);
        Messenger<PieceType[], int>.RemoveListener(GameEvent.PIECES_MATCHED, onPiecesMatched);

    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnToggleView()
    {
        Messenger.Broadcast(GameEvent.TOGGLE_VIEW);
    }

    public void OnPieceSelect(string type)
    {
        PieceType realType = (PieceType)Enum.Parse(typeof(PieceType), type);
        Messenger<PieceType>.Broadcast(GameEvent.PIECE_SELECTED, realType);
    }

    public void OnConfirmMove()
    {
        Messenger.Broadcast(GameEvent.MOVE_CONFIRMED);
    }

    private void UpdateBankValues(Dictionary<PieceType, int> bank, PieceButton[] pieces)
    {
        foreach (PieceButton btn in pieces)
        {
            btn.SetQuantity(bank[btn.pieceType]);
        }
    }

    private void OnPieceDrawerToggle(bool active, int activePlayer)
    {
        if (active)
        {
            UpdateBankValues(Managers.Player.GetPieceBank(activePlayer), pieceButtons);
            pieceDrawer.Open();
        }
        else
        {
            pieceDrawer.Close();
        }
    }

    private void OnConfirmDrawerToggle(bool active)
    {
        if (active)
        {
            confirmDrawer.Open();
        }
        else
        {
            confirmDrawer.Close();
        }
    }

    private void OnActivePlayerChanged(int playerNum)
    {
        if (playerNum == 1)
        {
            p1Name.SetActive(true);
            p2Name.SetActive(false);
        }
        else if (playerNum == 2)
        {
            p2Name.SetActive(true);
            p1Name.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Active Player Is Not 1 or 2");
        }
        UpdateBankValues(Managers.Player.GetPieceBank(1), p1Pieces);
        UpdateBankValues(Managers.Player.GetPieceBank(2), p2Pieces);
    }

    private void OnManagersStarted()
    {
        p1Name.SetName(Managers.Player.GetPlayer(1).id);
        p2Name.SetName(Managers.Player.GetPlayer(2).id);
        UpdateBankValues(Managers.Player.GetPieceBank(1), p1Pieces);
        UpdateBankValues(Managers.Player.GetPieceBank(2), p2Pieces);
    }

    private void onPiecesMatched(PieceType[] types, int playerNum)
    {
        Debug.Log(types.Length);
        if (playerNum == 1)
        {
            foreach (PieceButton pieceIndicator in p1Pieces)
            {
                if (Array.IndexOf(types, pieceIndicator.pieceType) != -1)
                {
                    pieceIndicator.SetMatched(true);
                }
                else
                {
                    if (pieceIndicator.pieceType != PieceType.WILD)
                    {
                        pieceIndicator.SetMatched(false);
                    }
                }
            }
        }
        else if (playerNum == 2)
        {
            foreach (PieceButton pieceIndicator in p2Pieces)
            {
                if (Array.IndexOf(types, pieceIndicator.pieceType) != -1)
                {
                    pieceIndicator.SetMatched(true);
                }
                else
                {
                    if (pieceIndicator.pieceType != PieceType.WILD)
                    {
                        pieceIndicator.SetMatched(false);
                    }
                }
            }
        }
    }
}
