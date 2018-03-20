using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour, IGameManager
{
    public GameMode currentGameMode { get; private set; }
    public ManagerStatus status
    {
        get; private set;
    }

    public void Startup()
    {
        Debug.Log("Starting Game Mode Manager");
        currentGameMode = GameMode.NONE;
        status = ManagerStatus.Started;
    }

    public void StartGame(GameMode mode)
    {
        Managers.Board.Reset();
        Managers.Player.Reset();
        currentGameMode = mode;
        if (mode == GameMode.LOCAL)
        {
            SceneManager.LoadScene("local_game");
        }
    }


}