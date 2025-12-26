using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class SceneMemoryManager
{
    public readonly Dictionary<string, SceneState> visitedScenes = new();
    public string currentSceneName;
    public SceneState currentSceneState;

    public SceneMemoryManager(string firstScene)
    {
        currentSceneName = firstScene;
        currentSceneState = new SceneState();
        visitedScenes.Add(currentSceneName, currentSceneState);

    }

    //static instance survive scene repoading 
    public static SceneMemoryManager instance = new SceneMemoryManager("Episode1");

    public void LoadScene(string sceneName)
    {


        currentSceneName = sceneName;
        if (!visitedScenes.ContainsKey(currentSceneName))
        {
            currentSceneState = new SceneState();
            visitedScenes.Add(currentSceneName, currentSceneState);
        }
        else
        {
            currentSceneState = visitedScenes[currentSceneName];
            currentSceneState.timesVisited += 1;
        }
        SceneManager.LoadScene(currentSceneName);
    }

}

[System.Serializable]
public class SceneState
{
    public int timesVisited = 0;
}



