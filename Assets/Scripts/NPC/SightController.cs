using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightController : MonoBehaviour
{

    public Flowchart flowchart;

    public GameObject npc;
    public bool isNPCDone = false;
    // public bool playerCaught;

    public GameObject player;
   // private PlayerMovement playerMovement;
    private Animator playerAnimator;
    private Animator npcAnimator;

    private NPCMovement npcMovement;
    private CameraController camera;

    public GameObject lookBeacon;

    public bool doRaycast = true;

    bool asked = true;

    void Start()
    {
        npcMovement = npc.GetComponent<NPCMovement>();
        npcAnimator = npc.GetComponentInChildren<Animator>();
        camera = Camera.main.GetComponent<CameraController>();

        player = GameObject.FindGameObjectWithTag("Player");

        lookBeacon = GameObject.FindGameObjectWithTag("LookBeacon");
    }

    // Update is called once per frame
    void Update()
    {

        if (flowchartController.Instance.playerCaught)
        {
            //SetPlayerMovement(false);
            Debug.LogWarning("Player Caught of: " + npcMovement.npcId);
            if (camera.inPosition)
            {
                Debug.LogWarning("Camera in pos of: " + npcMovement.npcId);
                player.transform.LookAt(new Vector3(lookBeacon.transform.position.x, player.transform.position.y, lookBeacon.transform.position.z));
                npc.transform.LookAt(new Vector3(lookBeacon.transform.position.x, npc.transform.position.y, lookBeacon.transform.position.z));
            }
            else
            {
                Debug.LogWarning("Cam not in pos of: " + npcMovement.npcId);
                player.transform.LookAt(new Vector3(npc.transform.position.x, player.transform.position.y, npc.transform.position.z));
                npc.transform.LookAt(new Vector3(player.transform.position.x, npc.transform.position.y, player.transform.position.z));
            }
        }
        else
        {
            //SetPlayerMovement(true);

            if (doRaycast)
            {
                DoRaycast();
            }
        }
    }

    public void SetPlayerMovement(bool canMove)
    {
        Debug.LogWarning("setting player movement of: " + npcMovement.npcId);
        //playerMovement = player.GetComponent<PlayerMovement>();
        playerAnimator = player.GetComponentInChildren<Animator>();
        //playerMovement.enabled = canMove;
        //Debug.Log("canMoveS " + canMove + " : " + playerMovement.enabled);
        flowchartController.Instance.StopPlayerMovement(!canMove);
        playerAnimator.SetFloat("moveSpeed", 0);
        playerAnimator.SetBool("stopped", !canMove);
    }

    public void DoRaycast()
    {

        RaycastHit hit;
        Vector3 rayDirection;

        rayDirection = player.transform.position - gameObject.transform.position;

        Ray ray = new Ray(gameObject.transform.position + Vector3.up/4, rayDirection);
        if (Physics.Raycast(ray, out hit, 10f))
        {

            Debug.DrawRay(ray.origin, rayDirection, Color.green);
            if ((hit.collider.gameObject.tag == "Player" || hit.collider.gameObject.tag == "PlayerBody") && !isNPCDone)
            {
                Debug.LogWarning("hit player of: " + npcMovement.npcId);

                UpdateStorage();

                //set the values to fungues flowchart
                if (flowchartController.Instance.allDoneTraining)
                {
                    Debug.LogWarning("all done training of: " + npcMovement.npcId);
                    if (asked)
                    {
                        Debug.LogWarning("building flowchart of: " + npcMovement.npcId);
                        asked = false;
                        flowchart.GetComponent<FungusTesting>().BuildQuestions();
                        isNPCDone = true;
                    }
                }

                //stops the player's movement
                //SetPlayerMovement(false);
                flowchartController.Instance.playerCaught = true;

                SetNPCComponents();
            }
            else
            {

                if (!npcMovement.moveToInitialPosition)
                {
                    LookAtPlayer();
                }
            }
        }
        else
        {
            Debug.LogWarning("rc hit nothing of: " + npcMovement.npcId);
            if (!npcMovement.moveToInitialPosition)
            {
                LookAtPlayer();
            }
        }
    }

    public void UpdateStorage()
    {
        StorageManager.Instance.SetCurrentNPC(npcMovement.npcId);
        StorageManager.Instance.SetCurrentEncounter(npcMovement.encounterId);
        StorageManager.Instance.SetCurrentQuestion(npcMovement.questionId);

        StorageManager.Instance.currentChoices = StorageManager.Instance.GetQuestion(StorageManager.Instance.currentQuestion).Choices;
    }

    public void LookAtPlayer()
    {
        npc.transform.LookAt(new Vector3(player.transform.position.x, npc.transform.position.y, player.transform.position.z));
    }

    public void SetNPCComponents()
    {
        Debug.LogWarning("Sigthcontroller anim set speed");
        npcAnimator.SetFloat("moveSpeed", npcMovement.speed);

        npc.GetComponent<NPCMovement>().moveToInitialPosition = false;

        //enable the NPC Movement so npc goes towards player's position
        npc.GetComponent<NPCMovement>().moveTrigger = true;

        //activate npc's trigger
        //npc.gameObject.GetComponent<CapsuleCollider>().isTrigger = true;
        //npc.gameObject.GetComponent<SphereCollider>().isTrigger = true;
        npc.gameObject.GetComponent<SphereCollider>().enabled = true;
    }
}
