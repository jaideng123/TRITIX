using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerManager))]
public class Managers : MonoBehaviour
{
    public static PlayerManager Player;
    public static AudioManager Audio;
    public static BoardManager Board;
    public static GameModeManager GameMode;
    public static BackdropManager Backdrop;
    private List<IGameManager> _startSequence;


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Player = GetComponent<PlayerManager>();
        Audio = GetComponent<AudioManager>();
        Board = GetComponent<BoardManager>();
        GameMode = GetComponent<GameModeManager>();
        Backdrop = GetComponent<BackdropManager>();
        _startSequence = new List<IGameManager>();
        _startSequence.Add(Player);
        _startSequence.Add(Audio);
        _startSequence.Add(Board);
        _startSequence.Add(GameMode);
        _startSequence.Add(Backdrop);
        StartCoroutine(StartupManagers());
    }

    private IEnumerator StartupManagers()
    {
        foreach (IGameManager manager in _startSequence)
        {
            manager.Startup();
        }

        yield return null;

        int numModules = _startSequence.Count;
        int numReady = 0;

        while (numReady < numModules)
        {
            int lastReady = numReady;
            numReady = 0;

            foreach (IGameManager manager in _startSequence)
            {
                if (manager.status == ManagerStatus.Started)
                {
                    numReady++;
                }
            }
            if (numReady > lastReady)
            {
                Debug.Log("Progress: " + numReady + "/" + numModules);
            }
            yield return null;
        }
        Debug.Log("All Managers Started!");
        Messenger.Broadcast(GameEvent.ALL_MANAGERS_STARTED);

    }
}
