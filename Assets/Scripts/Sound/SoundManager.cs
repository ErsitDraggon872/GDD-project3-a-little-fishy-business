using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Add your sounds here")]
    public AudioClip[] sounds_list;

    public AudioSource audioSource;

    //public AudioSource sfxSource;
    public AudioSource musicSource;


    void Awake()
    {
        // one soundmanager (singleton)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();

        //sfxSource = gameObject.AddComponent<AudioSource>();
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        LoadSettings();
        PlayMusic("Theme_Song");
    }




    // play sound by index
    public void Play(int index)
    {
        if (index < 0 || index >= sounds_list.Length)
        {
            Debug.LogWarning("Sound index out of range!");
            return;
        }

        audioSource.PlayOneShot(sounds_list[index]);
    }

    // play the sound by name
    public void Play(string soundName)
    {
        foreach (var clip in sounds_list)
        {
            if (clip.name == soundName)
            {
                audioSource.PlayOneShot(clip);
                return;
            }
        }

        Debug.LogWarning("Sound '" + soundName + "' not found!");
    }
    public void PlayMusic(string soundName)
    {
        foreach (var clip in sounds_list)
        {
            if (clip.name == soundName)
            {
                musicSource.clip = clip;
                musicSource.Play();
                return;
            }
        }
        Debug.LogWarning("Music '" + soundName + "' not found!");
    }

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
        SaveSettings();
    }
    //public void SetSFXVolume(float volume)
    //{
    //    sfxSource.volume = volume;
    //    SaveSettings();
    //}

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicSource.volume);
        //PlayerPrefs.SetFloat("SFXVolume", sfxSource.volume);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        //if (PlayerPrefs.HasKey("SFXVolume"))
        //    sfxSource.volume = PlayerPrefs.GetFloat("SFXVolume");
    }
}
