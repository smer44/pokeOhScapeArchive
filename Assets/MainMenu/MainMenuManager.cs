using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour 
{
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject panelMainMenu;
    [SerializeField] private GameObject battleMenuPanel;
    [SerializeField] private GameObject panelOptions;
    [SerializeField] private GameObject combatFieldPanel;
    [SerializeField] private GameObject combatTurnsPanel;
    [SerializeField] private GameObject moveTypesPanel;
    [SerializeField] private GameObject movesListPanel;
    [SerializeField] private GameObject creatureInfoPanel;
    [SerializeField] private GameObject howToPlayPanel;

    public void Play()
    {
        panelMainMenu.SetActive(false);
        battleMenuPanel.SetActive(true);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowMainMenu()
    {
        battleMenuPanel.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    public void ShowCreatureInfo()
    {
        creatureInfoPanel.SetActive(true);
    }

    public void CloseCreatureInfo()
    {
        creatureInfoPanel.SetActive(false);
    }

    public void OpenHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    public void OpenCombatFieldPanel()
    {
        combatFieldPanel.SetActive(true);
    }

    public void CloseCombatFieldPanel()
    {
        combatFieldPanel.SetActive(false);
    }

    public void OpenCombatTurnsPanel()
    {
        combatTurnsPanel.SetActive(true);
    }

    public void CloseCombatTurnsPanel()
    {
        combatTurnsPanel.SetActive(false);
    }

    public void OpenMoveTypesPanel()
    {
        moveTypesPanel.SetActive(true);
    }

    public void CloseMoveTypesPanel()
    {
        moveTypesPanel.SetActive(false);
    }

    public void OpenMoveListPanel()
    {
        movesListPanel.SetActive(true);
    }

    public void CloseMoveListPanel()
    {
        movesListPanel.SetActive(false);
    }

    public void OpenOptions()
    {
        panelMainMenu.SetActive(false);
        panelOptions.SetActive(true);
    }

    public void CloseOptions()
    {
        panelOptions.SetActive(false);
        panelMainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }
}
