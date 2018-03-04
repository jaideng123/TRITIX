using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private PieceDrawer pieceDrawer;

    void Awake()
    {
        Messenger<bool>.AddListener(GameEvent.TOGGLE_PIECE_DRAWER, OnPieceDrawerToggle);
    }
    void OnDestroy()
    {
        Messenger<bool>.RemoveListener(GameEvent.TOGGLE_PIECE_DRAWER, OnPieceDrawerToggle);
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

    private void OnPieceDrawerToggle(bool active)
    {
        if (active)
        {
            pieceDrawer.Open();
        }
        else
        {
            pieceDrawer.Close();
        }
    }
}
