using DG.Tweening;
using UnityEngine;
using TMPro;

public class MenuIntroAnimation : MonoBehaviour
{
    public TMP_Text title;
    public RectTransform[] buttons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        title.alpha = 0;
        title.rectTransform.anchoredPosition += new Vector2(0, 100);
        title.DOFade(1, 1f);
        title.rectTransform.DOAnchorPosY(title.rectTransform.anchoredPosition.y - 100, 1f).SetEase(Ease.OutBack);

        float delay = 0.3f;
        foreach (var btn in buttons)
        {
            btn.DOScale(1, 0.5f).SetEase(Ease.OutBack).SetDelay(delay);
            delay += 0.15f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
