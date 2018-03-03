﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PlayerManager))]
public class Managers : MonoBehaviour
{
    public static PlayerManager Player;
    private List<IGameManager> _startSequence;


    void Awake()
    {
        Player = GetComponent<PlayerManager>();
        _startSequence = new List<IGameManager>();
        _startSequence.Add(Player);
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