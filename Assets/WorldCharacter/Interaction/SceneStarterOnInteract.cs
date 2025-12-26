using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStarterOnInteract : Interactable
///This class starts scene with given name if it gets interacted with something
/// 
{
    public string sceneName;
    public bool enableCursor = true;

    void Start()
    {

    }


    void Update()
    {

    }

    public override void OnInteract(GameObject other)
    {
        if (enableCursor) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;            
        }
        //Debug.Log($"SceneStarterOnInteract: starting scene {sceneName}");
        SceneMemoryManager.instance.LoadScene(sceneName);
    }
    

}
