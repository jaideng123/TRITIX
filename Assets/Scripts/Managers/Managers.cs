using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Managers : MonoBehaviour
{
    public static AudioManager Audio;
    public static GameModeManager GameMode;
    public static BackdropManager Backdrop;
    public static AuthManager Auth;
    private List<IGameManager> _startSequence;

    public bool initialized = false;


    void Awake()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Manager"))
        {
            Managers mgr = obj.GetComponent<Managers>();
            if (mgr != null && mgr.initialized)
            {
                Destroy(this.gameObject);
                return;
            }
        }
        DontDestroyOnLoad(gameObject);
        Audio = GetComponent<AudioManager>();
        GameMode = GetComponent<GameModeManager>();
        Backdrop = GetComponent<BackdropManager>();
        Auth = GetComponent<AuthManager>();
        _startSequence = new List<IGameManager>();
        _startSequence.Add(Audio);
        _startSequence.Add(GameMode);
        _startSequence.Add(Backdrop);
        _startSequence.Add(Auth);
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
        initialized = true;
        Messenger.Broadcast(GameEvent.ALL_MANAGERS_STARTED, MessengerMode.DONT_REQUIRE_LISTENER);

    }
}
