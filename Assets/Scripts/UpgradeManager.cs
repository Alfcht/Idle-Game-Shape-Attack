using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public Button BtnDamage;
    public Button BtnSpeed;
    public Button BtnHP;
    public Button BtnCritChance;
    public Button BtnCritDamage;
    public Button BtnAttribute;

    readonly int[] BaseCosts = { 10, 15, 20, 25, 30, 35 };

    void Update()
    {
        if (GameManager.Instance == null) return;
        RefreshButtons();
    }

    void RefreshButtons()
    {
        UpdateButton(BtnDamage, "DMG", GameManager.Instance.LvlDamage, 0);
        UpdateButton(BtnSpeed, "SPD", GameManager.Instance.LvlSpeed, 1);
        UpdateButton(BtnHP, "HP", GameManager.Instance.LvlHP, 2);
        UpdateButton(BtnCritChance, "CRIT%", GameManager.Instance.LvlCritChance, 3);
        UpdateButton(BtnCritDamage, "CRITDMG", GameManager.Instance.LvlCritDamage, 4);
        UpdateButton(BtnAttribute, "ATTR", GameManager.Instance.LvlAttribute, 5);
    }

    void UpdateButton(Button Btn, string Label, int Level, int CostIndex)
    {
        int Cost = GetCost(CostIndex, Level);
        bool CanAfford = GameManager.Instance.Gold >= Cost;

        TextMeshProUGUI Txt = Btn.GetComponentInChildren<TextMeshProUGUI>();
        if (Txt != null)
            Txt.text = $"{Label}\nLv {Level}  {Cost}";

        Btn.interactable = CanAfford;

        var Colors = Btn.colors;
        Colors.normalColor = CanAfford ?
            new Color(0.1f, 0.15f, 0.1f) :
            new Color(0.08f, 0.08f, 0.1f);
        Btn.colors = Colors;
    }

    int GetCost(int Index, int Level) =>
        (int)(BaseCosts[Index] * Mathf.Pow(1.4f, Level - 1));

    public void BuyDamage() => Buy(0, ref GameManager.Instance.LvlDamage);
    public void BuySpeed() => Buy(1, ref GameManager.Instance.LvlSpeed);
    public void BuyHP() => Buy(2, ref GameManager.Instance.LvlHP);
    public void BuyCritChance() => Buy(3, ref GameManager.Instance.LvlCritChance);
    public void BuyCritDamage() => Buy(4, ref GameManager.Instance.LvlCritDamage);
    public void BuyAttribute() => Buy(5, ref GameManager.Instance.LvlAttribute);

    void Buy(int CostIndex, ref int Level)
    {
        int Cost = GetCost(CostIndex, Level);
        if (GameManager.Instance.SpendGold(Cost))
            Level++;
    }
}