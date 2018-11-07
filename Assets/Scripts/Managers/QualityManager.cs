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
        Application.targetFrameRate = 60;
        if (PlayerPrefs.HasKey("BATTERY_SAVER_ON") && PlayerPrefs.GetInt("BATTERY_SAVER_ON") == 1)
        {
            TurnBatterySaverOn();
        }
        status = ManagerStatus.Started;
    }

    public void TurnBatterySaverOn()
    {
        Application.targetFrameRate = 30;
        batterySaverOn = true;
        PlayerPrefs.SetInt("BATTERY_SAVER_ON", 1);
    }
    public void TurnBatterySaverOff()
    {
        Application.targetFrameRate = 60;
        batterySaverOn = false;
        PlayerPrefs.SetInt("BATTERY_SAVER_ON", 0);
    }
}