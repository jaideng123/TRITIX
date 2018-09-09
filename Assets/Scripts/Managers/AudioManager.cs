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

    private const float musicVolume = 0f;

    public bool soundEffectsMuted
    {
        get; private set;
    }

    private const float soundEffectsVolume = 0f;

    public void Startup()
    {
        Debug.Log("Starting Audio Manager");
        DontDestroyOnLoad(musicSource);
        musicSource.volume = .25f;
        StartBackgroundMusic();
        StartCoroutine(InitializePreviousSettings());
        status = ManagerStatus.Started;
    }

    private IEnumerator InitializePreviousSettings()
    {
        // We have to wait at least a frame so audio mixer can get set up
        yield return null;
        if (PlayerPrefs.HasKey("MUSIC_MUTED") && PlayerPrefs.GetInt("MUSIC_MUTED") == 1)
        {
            Debug.Log("Muting Background Music");
            MuteBackgroundMusic();
        }
        if (PlayerPrefs.HasKey("SOUND_EFFECTS_MUTED") && PlayerPrefs.GetInt("SOUND_EFFECTS_MUTED") == 1)
        {
            Debug.Log("Muting Sound Effects");
            MuteSoundEffects();
        }
    }

    public void StartBackgroundMusic()
    {

        PlayMusic(Resources.Load("Music/" + BGMusic) as AudioClip);
    }

    public void MuteBackgroundMusic()
    {
        audioMixer.SetFloat("Music Volume", -80f);
        musicMuted = true;
        PlayerPrefs.SetInt("MUSIC_MUTED", 1);
    }
    public void UnMuteBackgroundMusic()
    {
        audioMixer.SetFloat("Music Volume", musicVolume);
        musicMuted = false;
        PlayerPrefs.SetInt("MUSIC_MUTED", 0);
    }

    public void MuteSoundEffects()
    {
        audioMixer.SetFloat("Sound Effects Volume", -80f);
        soundEffectsMuted = true;
        PlayerPrefs.SetInt("SOUND_EFFECTS_MUTED", 1);
    }
    public void UnMuteSoundEffects()
    {
        audioMixer.SetFloat("Sound Effects Volume", soundEffectsVolume);
        soundEffectsMuted = false;
        PlayerPrefs.SetInt("SOUND_EFFECTS_MUTED", 0);
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