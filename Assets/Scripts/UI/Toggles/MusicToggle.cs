using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class MusicToggle : MonoBehaviour
{
    private Toggle toggle;
    // Use this for initialization
    void Start()
    {
        toggle = this.GetComponent<Toggle>();
        toggle.isOn = !Managers.Audio.musicMuted;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnToggleMute(bool value)
    {
        if (value)
        {
            Managers.Audio.UnMuteBackgroundMusic();
        }
        else
        {
            Managers.Audio.MuteBackgroundMusic();
        }
    }
}
