using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenController : Menu
{
    //public SceneReference TestLevelScene;

    public MenuClassifier moduleScreenClassifier;//inGameHUDClassifier;

    public override void OnHideMenu(string options)
    {
        base.OnHideMenu(options);
    }

    public override void OnShowMenu(string options)
    {
        base.OnShowMenu(options);
    }

   /* void SceneLoadedCallback(List<string> scenesLoaded)
    {
        SceneLoader.Instance.sceneLoadedEvent.RemoveListener(SceneLoadedCallback);
        if(DebugManager.Instance.delayHUDDisplay == false) MenuManager.Instance.ShowMenu(inGameHUDClassifier);
    }*/

        #region Testing
        /*
        public void OnTestLevelSelected()
    {
        //SceneManager.LoadScene(TestLevelScene,LoadSceneMode.Additive);                        // Replace with scene loader

        SceneLoader.Instance.sceneLoadedEvent.AddListener(SceneLoadedCallback);
        List<string> scenes = new List<string>();
        scenes.Add(TestLevelScene);
        //scenes.Add();     // ART SCENE WILL GO HERE EVENTUALLY
        SceneLoader.Instance.LoadScenes(scenes);

        MenuManager.Instance.HideMenu(menuClassifier);
        if (DebugManager.Instance.delayHUDDisplay == true)
            MenuManager.Instance.ShowMenu(inGameHUDClassifier);

            //SceneManager.SetActiveScene(SceneManager.GetSceneByPath(TestLevelScene.ScenePath));   // Replace with scene loader
    }*/

        public void OnToModuleSelected()
    {
        MenuManager.Instance.HideMenu(menuClassifier);
        MenuManager.Instance.ShowMenu(moduleScreenClassifier);
    }

    public void OnTestFileIO()
    {
        FileIO.Instance.ReadFile("Assets/JSON/jsontemplate.json");
    }
    #endregion
}
