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
        currentGameMode = mode;
        if (mode == GameMode.NONE)
        {
            GetComponent<Fading>().LoadScene("main_menu", .1f);
        }
        if (mode == GameMode.LOCAL)
        {
            GetComponent<Fading>().LoadScene("local_game", .1f);
        }
        if (mode == GameMode.COM)
        {
            GetComponent<Fading>().LoadScene("ai_game", .1f);
        }
    }



}