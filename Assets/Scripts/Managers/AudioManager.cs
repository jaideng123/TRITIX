using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour, IGameManager
{
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private string BGMusic;
    [SerializeField]
    private AudioMixer audioMixer;
    public ManagerStatus status
    {
        get; private set;
    }

    public bool musicMuted
    {
        get; private set;
    }

    private float musicVolume;

    public bool soundEffectsMuted
    {
        get; private set;
    }

    private float soundEffectsVolume;

    public void Startup()
    {
        Debug.Log("Starting Audio Manager");
        DontDestroyOnLoad(musicSource);
        musicSource.volume = .25f;
        StartBackgroundMusic();
        status = ManagerStatus.Started;
    }

    public void StartBackgroundMusic()
    {

        PlayMusic(Resources.Load("Music/" + BGMusic) as AudioClip);
    }

    public void MuteBackgroundMusic()
    {
        audioMixer.GetFloat("Music Volume", out musicVolume);

        audioMixer.SetFloat("Music Volume", -80f);
        musicMuted = true;
    }
    public void UnMuteBackgroundMusic()
    {
        audioMixer.SetFloat("Music Volume", musicVolume);
        musicMuted = false;
    }

    public void MuteSoundEffects()
    {
        audioMixer.GetFloat("Sound Effects Volume", out soundEffectsVolume);

        audioMixer.SetFloat("Sound Effects Volume", -80f);
        soundEffectsMuted = true;
    }
    public void UnMuteSoundEffects()
    {
        audioMixer.SetFloat("Sound Effects Volume", soundEffectsVolume);
        soundEffectsMuted = false;
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