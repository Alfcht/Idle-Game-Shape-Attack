using UnityEngine;
using UnityEngine.UI;

public class ColorNavigator : MonoBehaviour
{
    public Color[] Colors = new Color[]
    {
        new Color(1.0f, 0.31f, 0.48f),
        new Color(1.0f, 0.59f, 0.19f),
        new Color(1.0f, 0.88f, 0.40f),
        new Color(0.22f, 1.0f, 0.08f),
        new Color(0.0f, 0.90f, 1.0f),
        new Color(0.64f, 0.35f, 1.0f),
        new Color(1.0f, 1.0f, 1.0f),
        new Color(1.0f, 0.43f, 0.97f),
    };

    public Image ColorPreviewImage;
    private int CurrentIndex = 0;

    void Start()
    {
        UpdatePreview();
    }

    public void OnNext()
    {
        CurrentIndex++;
        if (CurrentIndex >= Colors.Length)
            CurrentIndex = 0;
        UpdatePreview();
    }

    public void OnPrev()
    {
        CurrentIndex--;
        if (CurrentIndex < 0)
            CurrentIndex = Colors.Length - 1;
        UpdatePreview();
    }

    void UpdatePreview()
    {
        ColorPreviewImage.color = Colors[CurrentIndex];
        GameManager.Instance.ChosenColor = Colors[CurrentIndex];
        GameObject ShapePreview = GameObject.Find("ShapePreview");
        if (ShapePreview != null)
            ShapePreview.GetComponent<Image>().color = Colors[CurrentIndex];
    }
}