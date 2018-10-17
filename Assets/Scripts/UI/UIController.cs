using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private HideableDrawer pieceDrawer;
    [SerializeField]
    private HideableDrawer confirmDrawer;
    [SerializeField]
    private HideableDrawer restartDrawer;
    [SerializeField]
    private HideablePanel winPanel;
    [SerializeField]
    private HideablePanel pauseMenuPanel;
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
        Messenger<Match[]>.AddListener(GameEvent.PIECES_MATCHED, onPiecesMatched);
        Messenger<int>.AddListener(GameEvent.GAME_OVER, OnGameOver);

    }
    void OnDestroy()
    {
        Messenger<bool, int>.RemoveListener(GameEvent.TOGGLE_PIECE_DRAWER, OnPieceDrawerToggle);
        Messenger<bool>.RemoveListener(GameEvent.TOGGLE_CONFIRM_DRAWER, OnConfirmDrawerToggle);
        Messenger<int>.RemoveListener(GameEvent.ACTIVE_PLAYER_CHANGED, OnActivePlayerChanged);
        Messenger<int>.RemoveListener(GameEvent.PLAYER_INFO_CHANGED, OnPlayerInfoChange);
        Messenger<Match[]>.RemoveListener(GameEvent.PIECES_MATCHED, onPiecesMatched);
        Messenger<int>.RemoveListener(GameEvent.GAME_OVER, OnGameOver);

    }
    void Start()
    {
        winPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
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
            UpdateBankValues(playerController.GetPieceBank(activePlayer), pieceButtons);
            Color playerColor = playerController.GetPlayer(activePlayer).pieceColor;
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
        if (playerController.GetPlayer(1) != null)
        {
            UpdateBankValues(playerController.GetPieceBank(1), p1Pieces);
        }
        if (playerController.GetPlayer(2) != null)
        {
            UpdateBankValues(playerController.GetPieceBank(2), p2Pieces);
        }
    }

    private void OnPlayerInfoChange(int playerNum)
    {
        if (playerNum == 1)
        {
            p1Name.SetName(playerController.GetPlayer(1).id);
            UpdateBankValues(playerController.GetPieceBank(1), p1Pieces);
        }
        else if (playerNum == 2)
        {
            p2Name.SetName(playerController.GetPlayer(2).id);
            UpdateBankValues(playerController.GetPieceBank(2), p2Pieces);
        }
    }

    private void onPiecesMatched(Match[] matches)
    {
        PieceType[] p1MatchedTypes = matches.Where(match => match.playerNum == 1).Select(match => match.pieceType).ToArray();
        foreach (PieceButton pieceIndicator in p1Pieces)
        {
            if (Array.IndexOf<PieceType>(p1MatchedTypes, pieceIndicator.pieceType) != -1)
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

        PieceType[] p2MatchedTypes = matches.Where(match => match.playerNum == 2).Select(match => match.pieceType).ToArray();
        foreach (PieceButton pieceIndicator in p2Pieces)
        {
            if (Array.IndexOf<PieceType>(p2MatchedTypes, pieceIndicator.pieceType) != -1)
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
        Debug.Log("Winner is " + playerController.GetPlayer(winner).id);
        Text winText = winPanel.GetComponentInChildren<Text>();
        winText.text = playerController.GetPlayer(winner).id + "\n Wins!";
        winPanel.SetActive(true);
        Messenger<bool>.Broadcast(GameEvent.GAME_PAUSED, true);
    }

    public void OnViewBoard()
    {
        winPanel.SetActive(false);
        restartDrawer.Open();
        Messenger<bool>.Broadcast(GameEvent.GAME_PAUSED, false);
    }

    public void OnRestartGame()
    {
        Managers.GameMode.StartGame(Managers.GameMode.currentGameMode);
    }

    public void OnExitGame()
    {
        Managers.GameMode.StartGame(GameMode.NONE);
    }

    public void OnOpenPauseMenu(bool open)
    {
        pauseMenuPanel.SetActive(open);
        Messenger<bool>.Broadcast(GameEvent.GAME_PAUSED, open);
    }
}
