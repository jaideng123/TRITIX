using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private string BGMusic;
    public ManagerStatus status
    {
        get; private set;
    }

    public bool musicMuted
    {
        get; private set;
    }

    public bool soundEffectsMuted
    {
        get; private set;
    }

    public void Startup()
    {
        Debug.Log("Starting Audio Manager");
        DontDestroyOnLoad(musicSource);
        musicSource.volume = .25f;
        PlayBackgroundMusic();
        status = ManagerStatus.Started;
    }

    public void PlayBackgroundMusic()
    {
        PlayMusic(Resources.Load("Music/" + BGMusic) as AudioClip);
    }

    public void MuteBackgroundMusic()
    {
        musicSource.mute = true;
    }
    public void UnMuteBackgroundMusic()
    {
        musicSource.mute = false;
    }
    private void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.Play();
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
}