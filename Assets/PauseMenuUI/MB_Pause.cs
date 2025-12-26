using UnityEngine;
using UnityEngine.SceneManagement;

public class MB_Pause : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject Panel;
    public GameObject OptionsPanel;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Panel.SetActive(!Panel.activeSelf);
            if (!Panel.activeSelf)
            {
                OptionsPanel.SetActive(false);
            }
        }
    }

    public void OpenOptionsPanel()
    {
        OptionsPanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadSceneAsync(BattleFieldMB.Instance.exitSceneName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
