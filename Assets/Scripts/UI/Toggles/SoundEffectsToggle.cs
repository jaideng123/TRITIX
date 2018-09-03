using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class SoundEffectsToggle : MonoBehaviour
{

    private Toggle toggle;
    // Use this for initialization
    void Start()
    {
        toggle = this.GetComponent<Toggle>();
        toggle.isOn = !Managers.Audio.soundEffectsMuted;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnToggleMute(bool value)
    {
        if (value)
        {
            Managers.Audio.UnMuteSoundEffects();
        }
        else
        {
            Managers.Audio.MuteSoundEffects();
        }
    }
}
