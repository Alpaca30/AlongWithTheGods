using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneEventsManager : MonoBehaviour
{
    [SerializeField]
    private PortalEvents portalEvents = null;
    [SerializeField]
    private GameObject portal = null;


    public void Init()
    {
        
    }


    public void OpenPortal()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int sceneIdx = GetActiveScene(currentScene);

        if (sceneIdx < 0 || sceneIdx >= (int)LoadingSceneManager.EScene.Count) return;

        portalEvents.SetDestination(sceneIdx);
        portal.gameObject.SetActive(true);
    }

    private int GetActiveScene(string _currentScene)
    {
        if (_currentScene == LoadingSceneManager.EScene.Play_Scene.ToString())
            return (int)LoadingSceneManager.EScene.Base_Scene;
        else if (_currentScene == LoadingSceneManager.EScene.Base_Scene.ToString())
            return (int)LoadingSceneManager.EScene.Play_Scene;
        else
            return -1;
    }
}
