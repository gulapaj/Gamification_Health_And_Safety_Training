using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrainingScreenController : Menu
{

    public GameObject text;

    public override void OnHideMenu(string options)
    {

        flowchartController.Instance.amountOfTrainingRead++;
        flowchartController.Instance.menuIsUp = false;
        flowchartController.Instance.playerCaught = false;
        flowchartController.Instance.StopPlayerMovement(false);
        Camera.main.GetComponent<CameraController>().ToStandardPosition();
        base.OnHideMenu(options);

        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("EncounterNPC"))
        {
            npc.GetComponent<NPCMovement>().moveToInitialPosition = true;
        }
    }

    public override void OnShowMenu(string options)
    {
        text.GetComponent<Text>().text = StorageManager.Instance.GetNPC(StorageManager.Instance.currentNPC).Training;
        base.OnShowMenu(options);
    }

        #region Testing

    public void OnExitSelected()
    {
        MenuManager.Instance.HideMenu(menuClassifier);

        

        //MenuManager.Instance.ShowMenu(moduleScreenClassifier);
    }

    public void OnTestFileIO()
    {
        FileIO.Instance.ReadFile("Assets/JSON/jsontemplate.json");
    }
    #endregion
}
