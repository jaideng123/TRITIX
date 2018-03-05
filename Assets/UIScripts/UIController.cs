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
    private PieceButton[] pieceButtons;

    void Awake()
    {
        Messenger<bool, int>.AddListener(GameEvent.TOGGLE_PIECE_DRAWER, OnPieceDrawerToggle);
        Messenger<bool>.AddListener(GameEvent.TOGGLE_CONFIRM_DRAWER, OnConfirmDrawerToggle);
    }
    void OnDestroy()
    {
        Messenger<bool, int>.RemoveListener(GameEvent.TOGGLE_PIECE_DRAWER, OnPieceDrawerToggle);
        Messenger<bool>.RemoveListener(GameEvent.TOGGLE_CONFIRM_DRAWER, OnConfirmDrawerToggle);
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

    private void UpdateBankValues(Dictionary<PieceType, int> bank)
    {
        foreach (PieceButton btn in pieceButtons)
        {
            btn.SetQuantity(bank[btn.pieceType]);
        }
    }

    private void OnPieceDrawerToggle(bool active, int activePlayer)
    {
        if (active)
        {
            UpdateBankValues(Managers.Player.GetPieceBank(activePlayer));
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
}
