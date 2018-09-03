using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField]
    private Button logoutButton;
    [SerializeField]
    private Toggle batterySaverToggle;
    // Use this for initialization
    void Start()
    {
        SetInteractableElements();
    }

    private void SetInteractableElements()
    {
        logoutButton.gameObject.SetActive(Managers.Auth.loggedIn);
        batterySaverToggle.isOn = Managers.Quality.batterySaverOn;
    }

    // Update is called once per frame
    void Update()
    {
        // SetInteractableElements();
    }

    public void ToggleBatterySaver(bool toggle)
    {
        if (toggle)
        {
            Managers.Quality.TurnBatterySaverOn();
        }
        else
        {
            Managers.Quality.TurnBatterySaverOff();
        }
    }

    public void LogOut()
    {
        Managers.Auth.LogOut();
    }
}