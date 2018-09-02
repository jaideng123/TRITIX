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
    [SerializeField]
    private Toggle musicToggle;
    [SerializeField]
    private Toggle soundEffectsToggle;

    // Use this for initialization
    void Start()
    {
        SetInteractableElements();
    }

    private void SetInteractableElements()
    {
        logoutButton.gameObject.SetActive(Managers.Auth.loggedIn);
        batterySaverToggle.isOn = Managers.Quality.batterySaverOn;
        musicToggle.isOn = !Managers.Audio.musicMuted;
        soundEffectsToggle.isOn = !Managers.Audio.soundEffectsMuted;
    }

    // Update is called once per frame
    void Update()
    {
        SetInteractableElements();
    }

    public void ToggleBatterySaver(bool toggle)
    {
        Managers.Quality.SetBatterySaver(toggle);
    }

    public void ToggleBackgroundMusic(bool toggle)
    {
        if (toggle)
        {
            Managers.Audio.UnMuteBackgroundMusic();
        }
        else
        {
            Managers.Audio.MuteBackgroundMusic();
        }
    }

    public void ToggleSoundEffects(bool toggle)
    {
        // TODO: Implement This
    }

    public void LogOut()
    {
        Managers.Auth.LogOut();
    }
}