using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayMenuManager : MonoBehaviour
{
    public GameObject playMenuPanel;         // PlayMenuPanel
    public GameObject creatureInfoPanel;     // CreatureInfo
    public GameObject creatureDetailsPanel;  // CreatureDetailsPanel
    public GameObject howToPlayPanel;       // HowToPlayPanel

    public void ShowMainMenu()
    {
        playMenuPanel.SetActive(true);
        creatureInfoPanel.SetActive(false);
        creatureDetailsPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        //This will allow the Player to return to the Play menu, that is on another scene.
        SceneManager.LoadScene("StartMenu");
    }

    public void ShowCreatureButtons()
    {
        playMenuPanel.SetActive(false);
        creatureInfoPanel.SetActive(true);
        creatureDetailsPanel.SetActive(false);
    }

    public void ShowCreatureDetails()
    {
        playMenuPanel.SetActive(false);
        creatureInfoPanel.SetActive(true); 
        creatureDetailsPanel.SetActive(true);
    }

    public void CloseCreatureDetails()
    {
        playMenuPanel.SetActive(false);
        creatureDetailsPanel.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }
}
