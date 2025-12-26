using UnityEngine;
using UnityEngine.Playables;

public class PlayIfFirstTime : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private PlayableDirector director;
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        if (SceneMemoryManager.instance.currentSceneState.timesVisited == 0)
        {
            director.Play();
        }

    }

}
