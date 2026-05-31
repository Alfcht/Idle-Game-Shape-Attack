using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI GoldText;
    public TextMeshProUGUI WaveText;

    void Update()
    {
        GoldText.text = $" {GameManager.Instance.Gold}";
        WaveText.text = $"WAVE {GameManager.Instance.WaveNumber}";
    }

    public void BuyUpgrade(string Key)
    {
        int Level = GetLevel(Key);
        int Cost = (int)(GetBaseCost(Key) * Mathf.Pow(1.4f, Level - 1));

        if (GameManager.Instance.SpendGold(Cost))
            IncrementLevel(Key);
    }

    int GetBaseCost(string Key) => Key switch
    {
        "damage" => 10,
        "speed" => 15,
        "hp" => 20,
        "critChance" => 25,
        "critDamage" => 30,
        "attribute" => 35,
        _ => 10
    };

    int GetLevel(string Key) => Key switch
    {
        "damage" => GameManager.Instance.LvlDamage,
        "speed" => GameManager.Instance.LvlSpeed,
        "hp" => GameManager.Instance.LvlHP,
        "critChance" => GameManager.Instance.LvlCritChance,
        "critDamage" => GameManager.Instance.LvlCritDamage,
        "attribute" => GameManager.Instance.LvlAttribute,
        _ => 1
    };

    void IncrementLevel(string Key)
    {
        switch (Key)
        {
            case "damage":
                GameManager.Instance.LvlDamage++;
                break;

            case "speed":
                GameManager.Instance.LvlSpeed++;
                break;

            case "hp":
                GameManager.Instance.LvlHP++;
                break;

            case "critChance":
                GameManager.Instance.LvlCritChance++;
                break;

            case "critDamage":
                GameManager.Instance.LvlCritDamage++;
                break;

            case "attribute":
                GameManager.Instance.LvlAttribute++;
                break;
        }
    }

    public void RefreshAttributeButtons()
    {
    }
}