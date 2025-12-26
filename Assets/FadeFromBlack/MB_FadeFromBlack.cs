using UnityEngine;

public class MB_FadeFromBlack : MonoBehaviour
{
    public float fadeTime = 0.5f;
    private float startTime = 0.0f;

    public GameObject fadeFromBlackPanel;
    private CanvasGroup canvasGroup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fadeFromBlackPanel.SetActive(true);
        startTime = Time.time;
        canvasGroup = fadeFromBlackPanel.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime <= fadeTime)
        {
            canvasGroup.alpha = Mathf.Clamp01(1 - (Time.time - startTime) / fadeTime);
        }
        else
        {
            canvasGroup.alpha = 0;
            fadeFromBlackPanel.SetActive(false);
        }
    }
}
