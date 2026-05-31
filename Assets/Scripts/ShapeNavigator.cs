using UnityEngine;
using UnityEngine.UI;

public class ShapeNavigator : MonoBehaviour
{
    public Sprite[] ShapeSprites;
    public Image PreviewImage;
    private int CurrentIndex = 0;

    void Start()
    {
        UpdatePreview();
    }

    public void OnNext()
    {
        CurrentIndex++;
        if (CurrentIndex >= ShapeSprites.Length)
            CurrentIndex = 0;
        UpdatePreview();
    }

    public void OnPrev()
    {
        CurrentIndex--;
        if (CurrentIndex < 0)
            CurrentIndex = ShapeSprites.Length - 1;
        UpdatePreview();
    }

    void UpdatePreview()
    {
        PreviewImage.sprite = ShapeSprites[CurrentIndex];
        PreviewImage.color = GameManager.Instance.ChosenColor;
        GameManager.Instance.ChosenShape = CurrentIndex;
    }
}