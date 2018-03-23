using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

    [SerializeField]
    private HideableDrawer pieceDrawer;
    [SerializeField]
    private HideableDrawer confirmDrawer;
    [SerializeField]
    private HideableDrawer restartDrawer;
    [SerializeField]
    private HideablePanel winPanel;
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
        Messenger<int>.AddListener(GameEvent.PLAYER_INFO_CHANGED, OnPlayerInfoChange);
        Messenger<PieceType[], int>.AddListener(GameEvent.PIECES_MATCHED, onPiecesMatched);
        Messenger<int>.AddListener(GameEvent.GAME_OVER, OnGameOver);

    }
    void OnDestroy()
    {
        Messenger<bool, int>.RemoveListener(GameEvent.TOGGLE_PIECE_DRAWER, OnPieceDrawerToggle);
        Messenger<bool>.RemoveListener(GameEvent.TOGGLE_CONFIRM_DRAWER, OnConfirmDrawerToggle);
        Messenger<int>.RemoveListener(GameEvent.ACTIVE_PLAYER_CHANGED, OnActivePlayerChanged);
        Messenger<int>.RemoveListener(GameEvent.PLAYER_INFO_CHANGED, OnPlayerInfoChange);
        Messenger<PieceType[], int>.RemoveListener(GameEvent.PIECES_MATCHED, onPiecesMatched);
        Messenger<int>.RemoveListener(GameEvent.GAME_OVER, OnGameOver);

    }
    void Start()
    {
        winPanel.SetActive(false);
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
            Color playerColor = Managers.Player.GetPlayer(activePlayer).pieceColor;
            pieceDrawer.gameObject.GetComponent<PieceColorer>().SetButtonColor(playerColor);
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

    private void OnPlayerInfoChange(int playerNum)
    {
        if (playerNum == 1)
        {
            Debug.Log(Managers.Player.GetPlayer(1).id);
            p1Name.SetName(Managers.Player.GetPlayer(1).id);
            UpdateBankValues(Managers.Player.GetPieceBank(1), p1Pieces);
        }
        else if (playerNum == 2)
        {
            p2Name.SetName(Managers.Player.GetPlayer(2).id);
            UpdateBankValues(Managers.Player.GetPieceBank(2), p2Pieces);
        }
    }

    private void onPiecesMatched(PieceType[] types, int playerNum)
    {
        PieceButton[] pieces = playerNum == 1 ? p1Pieces : p2Pieces;
        foreach (PieceButton pieceIndicator in pieces)
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

    private void OnGameOver(int winner)
    {
        Debug.Log("Winner is " + Managers.Player.GetPlayer(winner).id);
        Text winText = winPanel.GetComponentInChildren<Text>();
        winText.text = Managers.Player.GetPlayer(winner).id + "\n Wins!";
        winPanel.SetActive(true);
    }

    public void OnViewBoard()
    {
        winPanel.SetActive(false);
        restartDrawer.Open();
    }

    public void OnRestartGame()
    {
        Managers.Board.Reset();
        Managers.Player.Reset();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnToggleMute(bool value)
    {
        if (value)
        {
            Managers.Audio.UnMuteBackgroundMusic();
        }
        else
        {
            Managers.Audio.MuteBackgroundMusic();
        }
    }
}
