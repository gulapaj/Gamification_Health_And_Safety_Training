using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    [Serializable]
    public class SceneLoadedEvent : UnityEvent<List<string>> { }

    [HideInInspector] public SceneLoadedEvent sceneLoadedEvent = new SceneLoadedEvent();
    private List<string> scenesLoaded = new List<string>();

    public MenuClassifier loadingScreen;

    public float delayTimer = 1;

    public void LoadScene(string scene, bool displayLoadingScreen = true, string unloadScene = "")
    {
        StartCoroutine(_LoadScene(scene, displayLoadingScreen, unloadScene, true));
    }

    public void LoadScenes(List<string> scenes, bool displayLoadingScreen = true, string unloadScene = "")
    {
        StartCoroutine(_LoadScenes(scenes, displayLoadingScreen, unloadScene));
    }

    public void UnloadScene(string scene)
    {
        StartCoroutine(_UnloadScene(scene));
    }

    #region Coroutines

    IEnumerator _LoadScene(string scene, bool displayLoadingScreen, string unloadScene, bool raiseEvent)
    {
        SceneLoadedEvent sceneEvent = new SceneLoadedEvent();

        if(SceneManager.GetSceneByPath(scene).isLoaded && raiseEvent)
        {
            Debug.LogWarning("Scene is already loaded : " + scene);

            scenesLoaded.Clear();
            scenesLoaded.Add(scene);
            sceneLoadedEvent.Invoke(scenesLoaded);

            yield return null;
        }

        Application.backgroundLoadingPriority = ThreadPriority.Low;

        if (displayLoadingScreen == true)
        {
            MenuManager.Instance.ShowMenu(loadingScreen);
        }

        yield return new WaitForSeconds(delayTimer);

        AsyncOperation async = null;

        if(unloadScene != "")
        {
            async = SceneManager.UnloadSceneAsync(unloadScene);
            while (async.isDone == false) { yield return null; }
            async = Resources.UnloadUnusedAssets();
            while(async.isDone == false) { yield return null; }
        }

        yield return new WaitForSeconds(delayTimer);

        async = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        while(async.isDone == false) { yield return null; }

        yield return new WaitForSeconds(delayTimer);

        Scene managerScene = SceneManager.GetSceneByPath(scene);
        if(managerScene != null && managerScene.buildIndex != -1)
        {
            SceneManager.SetActiveScene(managerScene);
        }
        else
        {
            Debug.LogError("Can not make scene active");
        }

        if (raiseEvent)
        {
            scenesLoaded.Clear();
            scenesLoaded.Add(scene);
            sceneLoadedEvent.Invoke(scenesLoaded);
        }

        if (displayLoadingScreen)
        {
            MenuManager.Instance.HideMenu(loadingScreen);
        }

        Application.backgroundLoadingPriority = ThreadPriority.Normal;
    }

    IEnumerator _LoadScenes(List<string> scenes, bool displayLoadingScreen, string unloadScene)
    {
        scenesLoaded.Clear();
        foreach(string scene in scenes)
        {
            yield return StartCoroutine(_LoadScene(scene, displayLoadingScreen, unloadScene, false));
            scenesLoaded.Add(scene);
        }
        sceneLoadedEvent.Invoke(scenesLoaded);
    }

    IEnumerator _UnloadScene(string unloadScene)
    {
        AsyncOperation async = null;
        async = SceneManager.UnloadSceneAsync(unloadScene);
        while (async.isDone == false) { yield return null; }
        async = Resources.UnloadUnusedAssets();
        while(async.isDone == false) { yield return null; }
    }

    #endregion
}
