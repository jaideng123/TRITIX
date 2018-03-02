using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Managers : MonoBehaviour
{
    private List<IGameManager> _startSequence;

    void Awake()
    {

        _startSequence = new List<IGameManager>();
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

    }
}
