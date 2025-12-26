using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("References")]
    public AudioSource backgroundMusic;
    public Slider musicSlider;

    private void Start()
    {
        if (musicSlider != null)
        {
            musicSlider.value = backgroundMusic.volume / 0.4f; //#Raz Just to balance the volume scale. 
            musicSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    public void SetVolume(float volume)
    {
        backgroundMusic.volume = volume * 0.4f;
    }
}
