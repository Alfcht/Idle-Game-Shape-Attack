using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{

    [Header("Settings Panel")]
    public GameObject SettingsPanel;
    public Slider MusicSlider;
    public Slider SFXSlider;

    void Start()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVol", 0.7f);
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVol", 0.85f);
    }

    public void OnPlayClicked()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnSettingsClicked()
    {
        SettingsPanel.SetActive(true);
    }

    public void OnCloseSettings()
    {
        SettingsPanel.SetActive(false);
    }

    public void OnMusicVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void OnSFXVolumeChanged(float value)
    {
        if (AudioManager.Instance != null)
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void OnExitClicked()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
}