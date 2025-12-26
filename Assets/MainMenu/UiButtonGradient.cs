using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIButtonGradient : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public TMP_Text buttonText;
    public Image buttonBackground;
    public Color normalTextColor = Color.black;
    public Color hoverTextColor = new Color(1f, 0.85f, 0.2f); // dourado

    [Header("Gradient Colors")]
    public Color topColor = new Color(1f, 0.95f, 0.6f);  // Amarelo claro
    public Color midColor = new Color(1f, 0.84f, 0f);   // Ouro vivo
    public Color bottomColor = new Color(0.83f, 0.69f, 0.22f); // Ouro queimado

    [Header("Background Colors")]
    public Color normalBG = Color.white;
    public Color hoverBG = new Color(0.95f, 0.95f, 1f);

    private Vector3 originalScale;
    private Tween gradientTween;

    void Start()
    {
        if (buttonText == null || buttonBackground == null)
        {
            Debug.LogError("UIButtonGradient: referências não atribuídas no inspector!");
            return;
        }

        originalScale = transform.localScale;
        buttonBackground.color = normalBG;
    }

    void SetGradient(Color top, Color mid, Color bottom)
    {
        // VertexGradient usa 4 cores (topLeft, topRight, bottomLeft, bottomRight)
        buttonText.colorGradient = new VertexGradient(top, mid, bottom, mid);
    }

    void StartGradientAnimation()
    {
        gradientTween = DOTween.To(
            () => 0f,
            t =>
            {
                // Cria efeito "pulsante" no gradiente
                float lerp = Mathf.PingPong(t, 1f);
                Color newTop = Color.Lerp(topColor, midColor, lerp);
                Color newBottom = Color.Lerp(bottomColor, topColor, lerp);
                SetGradient(newTop, midColor, newBottom);
            },
            1f, 2f
        ).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetGradient(topColor, midColor, bottomColor);
        StartGradientAnimation();
        transform.DOScale(originalScale * 1.1f, 0.25f).SetEase(Ease.OutBack);
        buttonText.color = hoverTextColor;

        // Acelera o gradiente no hover
        gradientTween.timeScale = 2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(originalScale, 0.25f).SetEase(Ease.OutBack);
        buttonBackground.DOColor(normalBG, 0.25f);
        buttonText.color = normalTextColor;


        // Volta a velocidade normal
        gradientTween.timeScale = 1f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 5, 0.5f);

        // Flash rápido de brilho sem perder o gradiente
        var originalGradient = buttonText.colorGradient;
        DOTween.To(
            () => 0f,
            t =>
            {
                // Interpola para um gradiente branco
                Color flashTop = Color.Lerp(originalGradient.topLeft, Color.white, t);
                Color flashBottom = Color.Lerp(originalGradient.bottomLeft, Color.white, t);
                buttonText.colorGradient = new VertexGradient(flashTop, flashTop, flashBottom, flashBottom);
            },
            1f, 0.05f
        ).OnComplete(() =>
        {
            // Restaura gradiente original
            buttonText.colorGradient = originalGradient;
        });
    }

}

