using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUp : MonoBehaviour
{
    public SceneReference UISceneReference;
    public SceneReference StorageSceneReference;
    public MenuClassifier TitleScreen;

    // Start is called before the first frame update
    void Start()
    {
        Scene UIScene = SceneManager.GetSceneByPath(UISceneReference);

        if (!UIScene.isLoaded)
        {
            StartCoroutine(BootSequence());
        }
        else if(UIScene.buildIndex == -1)
        {
            Debug.LogError("Scene not found: " + UIScene);
        }
        else
        {
            StartCoroutine(IgnoreBootSequence());
        }
    }
    
    void SceneLoadedCallback(List<string> scenesLoaded)
    {
        SceneLoader.Instance.sceneLoadedEvent.RemoveListener(SceneLoadedCallback);
        MenuManager.Instance.HideMenu(TitleScreen);
    }

    IEnumerator BootSequence()
    {
        //SceneManager.LoadScene(UISceneReference, LoadSceneMode.Additive);                      // Replace with scene loader

        yield return new WaitForSeconds(2);

        //SceneManager.SetActiveScene(SceneManager.GetSceneByPath(UISceneReference.ScenePath));  // Replace with scene loader
        //MenuManager.Instance.HideMenu(TitleScreen);                                            // Replace with scene loader
        
        SceneLoader.Instance.sceneLoadedEvent.AddListener(SceneLoadedCallback);
        SceneLoader.Instance.LoadScene(StorageSceneReference, false);
        SceneLoader.Instance.LoadScene(UISceneReference, false);
    }

    IEnumerator IgnoreBootSequence()
    {
        Debug.LogWarning("Boot sequence ignored.");
        yield return new WaitForSeconds(2);
        SceneLoadedCallback(null);
    }
}
