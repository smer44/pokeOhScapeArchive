using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStarterOnEnter :MonoBehaviour
///This class starts scene with given name if it gets interacted with something
/// 
{
    public string sceneName;

    public string targetLayer;
    void Start()
    {

    }


    void Update()
    {

    }

    private  void OnTriggerEnter(Collider  other)
    {
        int  targetLayer = LayerMask.NameToLayer(this.targetLayer);

        if (other.gameObject.layer == targetLayer)
        {
            SceneMemoryManager.instance.LoadScene(sceneName);
        }
        
    }
    

}
