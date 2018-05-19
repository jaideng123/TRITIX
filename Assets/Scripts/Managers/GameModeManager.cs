using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour, IGameManager
{
    public GameMode currentGameMode { get; private set; }

    public Dictionary<string, string> modeParams;
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

    public void StartGame(GameMode mode, Dictionary<string, string> param = null)
    {
        currentGameMode = mode;
        modeParams = param;
        if (mode == GameMode.NONE)
        {
            GetComponent<Fading>().LoadScene("main_menu", .1f);
        }
        if (mode == GameMode.LOCAL)
        {
            GetComponent<Fading>().LoadScene("local_game", .1f);
        }
        if (mode == GameMode.COM_EASY || mode == GameMode.COM_MEDIUM || mode == GameMode.COM_HARD)
        {
            GetComponent<Fading>().LoadScene("ai_game", .1f);
        }
        if (mode == GameMode.ONLINE)
        {
            GetComponent<Fading>().LoadScene("online_game", .1f);
        }
    }



}