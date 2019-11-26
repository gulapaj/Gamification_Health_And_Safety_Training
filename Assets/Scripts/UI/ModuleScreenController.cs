
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ModuleScreenController : Menu
{
    public MenuClassifier LevelScreenClassifier;

    public List<string> moduleNames;
    public List<string> fileNames;
    public string currentPath;

    public GameObject button;
    public GameObject scrollContent;
    public Vector3 buttonOffset = new Vector3(0, 0, 0);
    public Vector3 buttonOffsetIncrease = new Vector3(0, 0, 0);

    public override void OnShowMenu(string options)
    {

        base.OnShowMenu(options);
        GameManager.Instance.tutorial = false;
        moduleNames.Clear();
        fileNames.Clear();
        StorageManager.Instance.ClearStorage();

        moduleNames = new List<string>();  //Directory.GetFiles("Assets/JSON/", "*.json"));


        // StartCoroutine(GetRequest("http://yanjin.dev.fast.sheridanc.on.ca/capstonegame/Tutorial.json"));
        //StartCoroutine(GetRequest("http://yanjin.dev.fast.sheridanc.on.ca/etrain/JSON/WHMIS%20Basics.json"));

        StartCoroutine(GetRequestFiles("http://yanjin.dev.fast.sheridanc.on.ca/etrain/JSON"));
    }

    public override void OnHideMenu(string options)
    {
        ClearButtons();
        base.OnHideMenu(options);
    }

    public void SelectModule()
    {
        ReadJSON();
        GoToLevelSelect();
    }

    public void ReadJSON()
    {
        currentPath = EventSystem.current.currentSelectedGameObject.GetComponentsInChildren<Text>()[1].text;
        FileIO.Instance.ReadFile(currentPath);
    }

    public void GoToLevelSelect()
    {
        MenuManager.Instance.HideMenu(menuClassifier);
        MenuManager.Instance.ShowMenu(LevelScreenClassifier);
    }

    public void ClearButtons()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("DynamButton"))
        {
            Destroy(go);
        }
        buttonOffset = new Vector3(0, 0, 0);
    }

    IEnumerator GetRequestFiles(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        www.downloadHandler = new DownloadHandlerBuffer();

        // Request and wait for the desired page.
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //moduleNames.AddRange(Directory.GetFiles("Assets/JSON/","Tutorial.json"));
            Regex regex = new Regex("<a href=\".*\">(?<name>.*\\.json)<\\/a>");
            MatchCollection matches = regex.Matches(www.downloadHandler.text);

            //fileNames.Add(www.downloadHandler.text);
            Debug.LogWarning(matches.Count);
            foreach (Match match in matches)
            {
                string cleanFileName = match.Groups["name"].Value;
                Debug.LogWarning(cleanFileName);
                cleanFileName = cleanFileName.Replace(" ", "%20");
                Debug.LogWarning(cleanFileName);

                fileNames.Add(cleanFileName);
            }

            foreach (string file in fileNames)
            {
                Debug.LogWarning("http://yanjin.dev.fast.sheridanc.on.ca/etrain/JSON/" + file);
                StartCoroutine(GetRequest("http://yanjin.dev.fast.sheridanc.on.ca/etrain/JSON/" + file));
            }
        }
    }

    IEnumerator GetRequest(string uri)
    {
        UnityWebRequest www = UnityWebRequest.Get(uri);
        www.downloadHandler = new DownloadHandlerBuffer();

        // Request and wait for the desired page.
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            //moduleNames.AddRange(Directory.GetFiles("Assets/JSON/","Tutorial.json"));
            Regex regex = new Regex("[\\n\\r].*ModuleTitle\":\\s*\"([^\\n\\r]*)\"");
            MatchCollection matches = regex.Matches(www.downloadHandler.text);

            moduleNames.Add(www.downloadHandler.text);

            foreach (Match match in matches)
            {
                //Instantiate Button
                GameObject modButton = Instantiate(button);

                //Correct text
                //string name = Regex.Match(path, "[\\n\\r].*ModuleTitle\":\\s*\"([^\\n\\r]*)\"").Captures[0].Value;
                Debug.LogWarning(name);

                //Set Values
                modButton.GetComponentsInChildren<Text>()[0].text = match.Groups[1].Value;
                modButton.GetComponentsInChildren<Text>()[1].text = www.downloadHandler.text;
                modButton.transform.SetParent(scrollContent.transform);
                modButton.transform.position = modButton.transform.parent.transform.position + buttonOffset;

                buttonOffset += buttonOffsetIncrease;
                modButton.GetComponent<Button>().onClick.AddListener(SelectModule);
            }
        }
    }
}
