using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flowchartController : Singleton<flowchartController>
{
    public int amountOfTrainingRead = 0;
    public bool allDoneTraining = false;
    public bool menuIsUp = false;
    public bool playerCaught = false;
    private float timer = 0;

    private void Update()
    {
        if (flowchartController.Instance.amountOfTrainingRead >= StorageManager.Instance.GetLevel(StorageManager.Instance.currentLevel).NPC.Count)
        {
            if (timer >= 5f)
            {
                allDoneTraining = true;
                DoneTraining();
                amountOfTrainingRead = 0;
            }
            else
            {
                timer += Time.deltaTime;
            }
        }

        if (flowchartController.Instance.playerCaught)
        {
            StopPlayerMovement(true);
        }
       /* else
        {
            StopPlayerMovement(false);
        }*/
    }

    public void StopPlayerMovement(bool stopMove)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().StopMovement(stopMove);
    }

    public void DoneTraining()
    {
        foreach (GameObject npc in GameObject.FindGameObjectsWithTag("EncounterNPC"))
        {
            ResetAfterTraining(npc);
        }
    }

    public void ResetAfterTraining(GameObject npc)
    {
        Debug.LogWarning("Reset after training");

        npc.GetComponent<EncounterNPCLogic>().enabled = true;
        npc.GetComponent<EncounterNPCLogic>().ResetCamera();
        npc.GetComponent<EncounterNPCLogic>().SetRaycast(true);
        npc.GetComponent<Rigidbody>().isKinematic = false;
        playerCaught = false;
        npc.GetComponentInChildren<SightController>().doRaycast = true;
        npc.GetComponentInChildren<SightController>().isNPCDone = false;
        //npc.gameObject.GetComponent<SphereCollider>().isTrigger = true;  //CapsuleCollider test
        npc.gameObject.GetComponent<SphereCollider>().enabled = true;  //CapsuleCollider test
        npc.GetComponent<NPCMovement>().moveToInitialPosition = true;
    }
}
