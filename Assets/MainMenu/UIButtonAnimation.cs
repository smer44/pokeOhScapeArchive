using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image buttonImage; // Fundo do botão
    public TMP_Text buttonText; // Texto do botão

    public Color normalColor = Color.white;
    public Color hoverColor = new Color(0.9f, 0.9f, 1f);

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;

        if (buttonImage != null)
            buttonImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(originalScale * 1.1f, 0.25f).SetEase(Ease.OutBack);
        if (buttonImage != null)
            buttonImage.DOColor(hoverColor, 0.25f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack);
        if (buttonImage != null)
            buttonImage.DOColor(normalColor, 0.25f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5, 0.5f);
    }
}
