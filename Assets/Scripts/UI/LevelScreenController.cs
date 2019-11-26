using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelScreenController : Menu
{
    public SceneReference LevelScene;
    public SceneReference TutorialScene;

    public MenuClassifier inGameHUDClassifier;

    public List<string> levelNames;

    public GameObject button;
    public GameObject scrollContent;
    public Vector3 buttonOffset = new Vector3(0,0,0);
    public Vector3 buttonOffsetIncrease = new Vector3(0,0,0);

    public override void OnShowMenu(string options)
    {
        levelNames.Clear();

        StorageManager.Instance.GetLevels();

        foreach (GameObject level in StorageManager.Instance.loadedLevels)
        {
            levelNames.Add(level.GetComponent<LevelMono>().Title.ToString());
        }

        foreach (string name in levelNames)
        {
            //Instantiate Button
            GameObject levButton = Instantiate(button);

            //Set Values
            levButton.GetComponentInChildren<Text>().text = name;
            levButton.transform.SetParent(scrollContent.transform);
            levButton.transform.position = levButton.transform.parent.transform.position + buttonOffset;

            buttonOffset += buttonOffsetIncrease;
            levButton.GetComponent<Button>().onClick.AddListener(OnTestLevelSelected);

            //Link Scene
        }

        base.OnShowMenu(options);
    }

    public override void OnHideMenu(string options)
    {
        ClearButtons();
        base.OnHideMenu(options);
    }

    void SceneLoadedCallback(List<string> scenesLoaded)
    {
        SceneLoader.Instance.sceneLoadedEvent.RemoveListener(SceneLoadedCallback);
        if (DebugManager.Instance.delayHUDDisplay == false)
            MenuManager.Instance.ShowMenu(inGameHUDClassifier);
    }

    public void OnTestLevelSelected()
    {
        SceneLoader.Instance.sceneLoadedEvent.AddListener(SceneLoadedCallback);
        List<string> scenes = new List<string>();

        string lName= levelNames.Find(x => x.Equals(EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Text>()[0].text));
        int lId = levelNames.IndexOf(lName);
        StorageManager.Instance.SetCurrentLevel(lId);

        if (StorageManager.Instance.GetModule(StorageManager.Instance.currentModule).ModuleTitle.Equals("Tutorial"))
        {
            GameManager.Instance.tutorial = true;
            scenes.Add(TutorialScene);
            //scenes.Add();     // ART SCENE WILL GO HERE EVENTUALLY
        }
        else
        {
            scenes.Add(LevelScene);
            //scenes.Add();     // ART SCENE WILL GO HERE EVENTUALLY
        }

        SceneLoader.Instance.LoadScenes(scenes);

        MenuManager.Instance.HideMenu(menuClassifier);
        if (DebugManager.Instance.delayHUDDisplay == true)
            MenuManager.Instance.ShowMenu(inGameHUDClassifier);
    }

    public void ClearButtons()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("DynamButton"))
        {
            Destroy(go);
        }
        buttonOffset = new Vector3(0, 0, 0);
    }
}

