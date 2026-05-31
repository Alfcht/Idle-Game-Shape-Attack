using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class HUDManager : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI WaveText;
    public TextMeshProUGUI EnemiesText;
    public TextMeshProUGUI HpText;

    public Slider TowerHpSlider;

    public TextMeshProUGUI WaveBanner;
    public float BannerDuration = 2f;

    private int LastWave = 0;

    void Update()
    {
        if (GameManager.Instance == null) return;

        var Gm = GameManager.Instance;

        GoldText.text = $"Gold: {Gm.Gold}";
        WaveText.text = $"Wave: WAVE {Gm.WaveNumber}";
        EnemiesText.text = $"Enemies Left: {Mathf.Max(0, Gm.EnemiesLeft)}";

        int Hp = Mathf.Max(0, (int)Gm.TowerHP);
        int MaxHp = Mathf.Max(1, (int)Gm.TowerMaxHP);
        HpText.text = $"HP: {Hp} / {MaxHp}";

        if (TowerHpSlider != null)
            TowerHpSlider.value = Mathf.Clamp01((float)Hp / MaxHp);

        if (Gm.WaveNumber != LastWave)
        {
            LastWave = Gm.WaveNumber;
            StartCoroutine(ShowWaveBanner(Gm.WaveNumber));
        }
    }

    IEnumerator ShowWaveBanner(int Wave)
    {
        WaveBanner.text = $"WAVE {Wave}";
        WaveBanner.gameObject.SetActive(true);
        Color C = WaveBanner.color;

        float T = 0f;

        while (T < 0.3f)
        {
            T += Time.deltaTime;
            C.a = Mathf.Lerp(0f, 1f, T / 0.3f);
            WaveBanner.color = C;
            yield return null;
        }

        yield return new WaitForSeconds(BannerDuration);

        T = 0f;

        while (T < 0.5f)
        {
            T += Time.deltaTime;
            C.a = Mathf.Lerp(1f, 0f, T / 0.5f);
            WaveBanner.color = C;
            yield return null;
        }

        WaveBanner.gameObject.SetActive(false);
    }
}