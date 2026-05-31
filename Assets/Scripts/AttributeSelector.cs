using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class AttributeSelector : MonoBehaviour
{
    public Button ExplosionButton;
    public Button PoisonButton;

    public Color ExplosionSelected = new Color(1f,0.44f,0.19f,0.4f);
    public Color PoisonSelected = new Color(0.31f,0.86f,0.31f,0.4f);
    public Color UnselectedColor = new Color(1f,1f,1f,0.05f);

    public GameObject TooltipPanel;
    public TextMeshProUGUI TooltipText;
    public float TooltipDelay = 1f;

    private const string EXPLOSION_DESC = "Bullets trigger an AoE blast on hit.";
    private const string POISON_DESC = "Bullets apply damage over time";

    private Coroutine TooltipCoroutine;

    void Start()
    {
        SelectExplosion();
        AddHoverListeners(ExplosionButton, EXPLOSION_DESC);
        AddHoverListeners(PoisonButton, POISON_DESC);
    }

    public void SelectExplosion()
    {
        GameManager.Instance.ChosenAttribute = "Explosion";
        ExplosionButton.GetComponent<Image>().color = ExplosionSelected;
        PoisonButton.GetComponent<Image>().color = UnselectedColor;
    }

    public void SelectPoison()
    {
        GameManager.Instance.ChosenAttribute = "Poison";
        PoisonButton.GetComponent<Image>().color = PoisonSelected;
        ExplosionButton.GetComponent<Image>().color = UnselectedColor;
    }

    void AddHoverListeners(Button Btn, string Description)
    {
        EventTrigger Trigger = Btn.gameObject.GetComponent<EventTrigger>();
        if (Trigger == null)
            Trigger = Btn.gameObject.AddComponent<EventTrigger>();

        var EnterEntry = new EventTrigger.Entry();
        EnterEntry.eventID = EventTriggerType.PointerEnter;
        EnterEntry.callback.AddListener((_) => OnHoverEnter(Btn, Description));
        Trigger.triggers.Add(EnterEntry);

        var ExitEntry = new EventTrigger.Entry();
        ExitEntry.eventID = EventTriggerType.PointerExit;
        ExitEntry.callback.AddListener((_) => OnHoverExit());
        Trigger.triggers.Add(ExitEntry);
    }

    void OnHoverEnter(Button Btn, string Description)
    {
        if (TooltipCoroutine != null)
            StopCoroutine(TooltipCoroutine);
        TooltipCoroutine = StartCoroutine(ShowTooltipAfterDelay(Btn, Description));
    }

    void OnHoverExit()
    {
        if (TooltipCoroutine != null)
            StopCoroutine(TooltipCoroutine);
        TooltipPanel.SetActive(false);
    }

    IEnumerator ShowTooltipAfterDelay(Button Btn, string Description)
    {
        yield return new WaitForSeconds(TooltipDelay);
        TooltipText.text = Description;

        RectTransform BtnRect     = Btn.GetComponent<RectTransform>();
        RectTransform TooltipRect = TooltipPanel.GetComponent<RectTransform>();
        TooltipRect.position = BtnRect.position + new Vector3(0, BtnRect.rect.height * 0.6f, 0);

        TooltipPanel.SetActive(true);
    }
}