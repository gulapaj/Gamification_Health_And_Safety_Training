using System.Collections;
using System.Collections.Generic;
using Fungus;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EncounterNPCLogic : MonoBehaviour
{
    public GameObject npc;
    public GameObject Question;
    public Flowchart flowchart;
    public bool isLevelDone;

    private GameObject player;
    private PlayerMovement playerMovement;
    private Animator npcAnimator;
    private Animator playerAnimator;

    private CameraController camera;

    public bool trained = false;
    public bool readTraining = false;

    private bool reset = false;
    private bool questioned = false;
    private bool needTraining = true;

    NPCMovement npcMovement;

    //public SayDialog largeDialog;
    public MenuClassifier trainingScreenClassifier;

    // Start is called before the first frame update
    void Start()
    {
        npcAnimator = npc.GetComponentInChildren<Animator>();
        camera = Camera.main.GetComponent<CameraController>();

        npcMovement = npc.GetComponent<NPCMovement>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isLevelDone)
        {
            Debug.LogWarning("lvl done of: " + npcMovement.npcId);
            LevelManager.Instance.levelDone = isLevelDone;
        }

        if (flowchart.GetBooleanVariable("Dead"))
        {
            Debug.LogWarning("DEAD of: " + npcMovement.npcId);

            flowchart.SetBooleanVariable("Dead", false);

            //unable the exclamation mark above the NPC
            npc.transform.GetChild(3).gameObject.SetActive(false);

            //remove one life if wrong
            HealthRecord.Instance.RemoverArray();
        }

        if (flowchart.GetBooleanVariable("Point"))
        {
            Debug.LogWarning("POINT of: " + npcMovement.npcId);

            flowchart.SetBooleanVariable("Point", false);

            //unable the exclamation mark above the NPC
            npc.transform.GetChild(3).gameObject.SetActive(false);

            //add one point in score manager
            ScoreManager.Instance.AddPoint();
        }

        if (flowchart.GetBooleanVariable("counter"))
        {
            Debug.LogWarning("ENC DONE of: " + npcMovement.npcId);

            flowchart.SetBooleanVariable("counter", false);

            //SetPlayerMovement(true);
            flowchartController.Instance.playerCaught = false;
            flowchartController.Instance.StopPlayerMovement(false);

            //gets the total number of the encounter
            int trophies = TrophyHandler.Instance.trophies.Count;

            npcAnimator.SetBool("stopped", false);

            //camera.ToStandardPosition();
            ResetCamera();

            //calls the ProgressBarChecker to change one encounter to done
            ProgressBarChecker(trophies);

            npc.GetComponent<NPCMovement>().moveTrigger = false;
            //moves the npc back to initial position to avoid blocking the player
            npc.GetComponent<NPCMovement>().moveToInitialPosition = true;
            npc.GetComponent<NPCMovement>().Stop(false);
        }
        /*
        if (readTraining)
        {
            if (!flowchartController.Instance.menuIsUp)
            {
                if (!flowchartController.Instance.allDoneTraining)
                {
                    //flowchartController.Instance.DoneTraining();
                    if (reset)
                    {
                        SetNPCComponentsViaUpdate();

                        //SetPlayerMovement(true);
                        
                        //camera.ToStandardPosition();
                    }
                }
            }
        }*/
    }

    void OnTriggerEnter(Collider collider)
    {
        //needs to check if the box collider for the player can be deleted!!!!
        if (collider.GetType() == typeof(SphereCollider) && collider.gameObject.tag == "Player")
        {
            player = collider.gameObject;

            if (trained)
            {
                if (!questioned)
                {
                    questioned = true;
                    SetNPCComponetsToStopViaTrigger();
                    SetNPCComponentsViaUpdate();
                    SetCamera();
                    flowchartController.Instance.playerCaught = true;
                    StartQuestions();
                    SetRaycast(false);
                }

                SetNPCComponetsToStopViaTrigger();
            }
            else
            {
                trained = true;
                SetNPCComponetsToStopViaTrigger();
                SetNPCComponentsViaUpdate();

                SetCamera();
                flowchartController.Instance.playerCaught = true;
                ShowTraining();
                SetRaycast(false);
            }
        }
    }

    //this will get the first trophy and change the color to black indicating that
    //the first encounter is done. You can the image to check to indicate that it is done
    public void ProgressBarChecker(int trophies)
    {
        //checks the trophy if it is done or not then move to the other
        for (int x = 0; x < trophies;)
        {
            int maxTrophies = trophies - 1;
            if (!isLevelDone)
            {
                GameObject selectedTrophy = TrophyHandler.Instance.trophies[x];
                if (selectedTrophy.gameObject.GetComponent<RawImage>().color != Color.white && maxTrophies == x) //this is last trophy and black
                {
                    isLevelDone = true;
                    Debug.Log("CONGRATULATIONS");
                    isLevelDone = true;
                    selectedTrophy.gameObject.GetComponent<RawImage>().color = Color.white;

                    x = trophies + 1; //ends the loop

                    //makes the npc goes back to initial position
                    npc.GetComponent<NPCMovement>().moveToInitialPosition = true;
                }
                else if (selectedTrophy.gameObject.GetComponent<RawImage>().color == Color.white && maxTrophies != x)
                {
                    //makes the npc goes back to initial position
                    npc.GetComponent<NPCMovement>().moveToInitialPosition = true;

                    x++; //goes to the next element in the array of trophies
                }
                else
                {
                    //makes the npc goes back to initial position
                    npc.GetComponent<NPCMovement>().moveToInitialPosition = true;

                    selectedTrophy.gameObject.GetComponent<RawImage>().color = Color.white;
                    x = trophies + 1; //this will stop the loop
                }
            }
        }

       // this.enabled = false;
    }

    public void SetPlayerMovement(bool canMove)
    {
        Debug.LogWarning("Set player movemment. " + canMove);
        flowchartController.Instance.StopPlayerMovement(!canMove);

        playerAnimator = player.GetComponentInChildren<Animator>();
        playerAnimator.SetBool("stopped", !canMove);
    }

    public void SetCamera()
    {
        Debug.LogWarning("Set cam");

        camera.SetCameraPositions(player.transform, npc.transform);
        camera.ToEncounterPosition();
    }

    public void ResetCamera()
    {
        Debug.LogWarning("Reset cam");

        //camera.SetCameraPositions(player.transform, npc.transform);
        camera.ToStandardPosition();
    }

    public void SetNPCComponetsToStopViaTrigger()
    {
        Debug.LogWarning("Set NPC Components Via Trigger");

        //this will remove the on trigger for npc after the question is done
        npc.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;

        //this serves as a wall
        npc.gameObject.GetComponent<CapsuleCollider>().enabled = true;

        Debug.LogWarning("enc anim walk stop");
        npcAnimator.SetFloat("moveSpeed", 0);
        npcAnimator.SetBool("stopped", true);
        //moves the npc back to initial position to avoid blocking the player
        npc.GetComponent<NPCMovement>().Stop(true);

        npc.GetComponent<NPCMovement>().moveTrigger = false;
        npc.GetComponent<Rigidbody>().isKinematic = true;


        npc.gameObject.GetComponent<SphereCollider>().enabled = false;
    }

    public void ShowTraining()
    {
        Debug.LogWarning("Show training");

        needTraining = false;
        MenuManager.Instance.ShowMenu(trainingScreenClassifier);
        flowchartController.Instance.menuIsUp = true;
        readTraining = true;

        //cut scene can be added here

        //moves the npc back to initial position to avoid blocking the player
        //npc.GetComponent<NPCMovement>().moveToInitialPosition = true;
    }

    public void StartQuestions()
    {
        Debug.LogWarning("Start Questions");

        flowchartController.Instance.amountOfTrainingRead = 0;
        //flowchartController.Instance.allDoneTraining = false;
        reset = true;
        npc.gameObject.GetComponent<SphereCollider>().enabled = false;
        //execute the questions
        flowchart.ExecuteBlock("Question1");
    }

    public void SetNPCComponentsViaUpdate()
    {
        Debug.LogWarning("Set NPC Componets Via Update");

        npcAnimator.SetBool("stopped", false);

        reset = false;
        //npc.GetComponentInChildren<SightController>().doRaycast = false;

        flowchartController.Instance.playerCaught = false;
        flowchartController.Instance.StopPlayerMovement(false);
    }

    public void SetRaycast(bool doRaycast)
    {

        npc.GetComponentInChildren<SightController>().doRaycast = doRaycast;
    }

    
}
