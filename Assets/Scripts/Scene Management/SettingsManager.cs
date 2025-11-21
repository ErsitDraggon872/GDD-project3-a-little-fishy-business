using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SettingsManager : MonoBehaviour
{
    public Scrollbar musicScrollbar;
    //public Scrollbar sfxScrollbar;

    public GameObject settingsPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicScrollbar.value = SoundManager.Instance.musicSource.volume;
        //sfxScrollbar.value = SoundManager.Instance.sfxSource.volume;

        musicScrollbar.onValueChanged.AddListener(SetMusicVolume);
        //sfxScrollbar.onValueChanged.AddListener(SetSFXVolume);

        settingsPanel.SetActive(false);
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            float newVolume = Mathf.Clamp01(SoundManager.Instance.musicSource.volume + scroll * 0.05f);
            SoundManager.Instance.SetMusicVolume(newVolume);
            musicScrollbar.value = newVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        SoundManager.Instance.SetMusicVolume(volume);
    }

    //public void SetSFXVolume(float volume)
    //{
    //    SoundManager.Instance.SetSFXVolume(volume);
    //}

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    public void TutorialSceneButton()
    {
               UnityEngine.SceneManagement.SceneManager.LoadScene("TutorialScene");
        SoundManager.Instance.Play("click1");
    }
}

