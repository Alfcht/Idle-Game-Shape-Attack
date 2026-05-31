using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        } else {
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [Header("Audio Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip mainMenuBGM;
    public AudioClip gameBGM;

    [Header("SFX Clips")]
    public AudioClip bulletLaunchSFX;
    public AudioClip enemyDeathSFX;

    [Header("Fade Settings")]
    public float fadeDuration  = 1.0f;
    public float bgmVolume     = 0.5f;
    public float sfxVolume     = 0.8f;

    void Start()
    {
        PlayBGMForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeToBGM(GetBGMForScene(scene.name)));

        if (GameManager.Instance != null) {
            bgmSource.volume = PlayerPrefs.GetFloat("MusicVol", bgmVolume);
            sfxSource.volume = PlayerPrefs.GetFloat("SFXVol",   sfxVolume);
        }
    }

    AudioClip GetBGMForScene(string sceneName)
    {
        return sceneName == "Game" ? gameBGM : mainMenuBGM;
    }

    void PlayBGMForScene(string sceneName)
    {
        AudioClip clip = GetBGMForScene(sceneName);
        if (clip == null) return;
        bgmSource.clip   = clip;
        bgmSource.volume = PlayerPrefs.GetFloat("MusicVol", bgmVolume);
        bgmSource.Play();
    }

    IEnumerator FadeToBGM(AudioClip newClip)
    {
        if (newClip == null) yield break;

        if (bgmSource.clip == newClip && bgmSource.isPlaying)
            yield break;

        float startVol = bgmSource.volume;

        float t = 0f;
        while (t < fadeDuration) {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(startVol, 0f, t / fadeDuration);
            yield return null;
        }

        bgmSource.Stop();
        bgmSource.clip   = newClip;
        bgmSource.volume = 0f;
        bgmSource.Play();

        float targetVol = PlayerPrefs.GetFloat("MusicVol", bgmVolume);
        t = 0f;
        while (t < fadeDuration) {
            t += Time.deltaTime;
            bgmSource.volume = Mathf.Lerp(0f, targetVol, t / fadeDuration);
            yield return null;
        }
    }

    public void PlayBulletLaunch()
    {
        if (bulletLaunchSFX != null)
            sfxSource.PlayOneShot(bulletLaunchSFX, sfxSource.volume);
    }

    public void PlayEnemyDeath()
    {
        if (enemyDeathSFX != null)
            sfxSource.PlayOneShot(enemyDeathSFX, sfxSource.volume);
    }

    public void SetMusicVolume(float vol)
    {
        bgmSource.volume = vol;
        PlayerPrefs.SetFloat("MusicVol", vol);
    }

    public void SetSFXVolume(float vol)
    {
        sfxSource.volume = vol;
        PlayerPrefs.SetFloat("SFXVol", vol);
    }
}