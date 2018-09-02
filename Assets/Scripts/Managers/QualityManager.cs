using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour, IGameManager
{
    public ManagerStatus status
    {
        get; private set;
    }

    public bool batterySaverOn
    {
        get; private set;
    }

    public void Startup()
    {
        Debug.Log("Starting Quality Manager");
        status = ManagerStatus.Started;
    }

    public void SetBatterySaver(bool toggle)
    {
        batterySaverOn = toggle;
    }
}