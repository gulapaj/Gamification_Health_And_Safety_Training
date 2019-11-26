using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : Singleton<NPCMovement>
{

    public float speed;
    public bool moveTrigger;
    public bool moveToInitialPosition;
    private GameObject player;

   // private GameObject npc;

    public int npcId = 0;
    public int encounterId = 0;
    public int questionId = 0;

    private Vector3 npcPos;

    NavMeshAgent navMeshAgent;
    private Animator npcAnimator;

    // Start is called before the first frame update
    void Start()
    {
        npcPos = transform.position;

        npcPos.x = this.transform.position.x;
        npcPos.y = this.transform.position.y;
        npcPos.z = this.transform.position.z;

        //PlayerPrefs.SetFloat("PlayerX", this.transform.position.x);
        //PlayerPrefs.SetFloat("PlayerY", this.transform.position.y);
        //PlayerPrefs.SetFloat("PlayerZ", this.transform.position.z);

        Debug.Log("this is the x pos: " + npcPos.x);
        Debug.Log("this is the y pos: " + npcPos.y);
        Debug.Log("this is the z pos: " + npcPos.z);

        navMeshAgent = GetComponent<NavMeshAgent>();
        npcAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");
      //  npc = this.gameObject; //gets the npc gameobject

       // Transform npcPos = npc.transform; //get the npc's current location

        //gets the position of the player as the destination of npc
        Transform playerTest = player.transform;

        if (moveTrigger)
        {
            //transform.position = Vector3.MoveTowards(transform.position, playerTest.position, speed * Time.deltaTime);
            Stop(false);
            navMeshAgent.SetDestination(player.transform.position);
            Debug.LogWarning("npc move anim set speed3");
            npcAnimator.SetFloat("moveSpeed", speed);
            npcAnimator.SetBool("stopped", false);
            if ((transform.position - npcPos).magnitude <= navMeshAgent.stoppingDistance)
            {
                moveToInitialPosition = false;
                Debug.LogWarning("npc move anim walk stop4");
                npcAnimator.SetFloat("moveSpeed", 0);
                npcAnimator.SetBool("stopped", true);
            }

            // Debug.Log(transform.position = Vector3.MoveTowards(transform.position, playerTest.position, speed * Time.deltaTime));
        }
        if(moveToInitialPosition)
        {
            //transform.position = Vector3.MoveTowards(transform.position, npcPos, speed * Time.deltaTime);
            npcPos.y = transform.position.y;
            transform.LookAt(npcPos);
            Stop(false);
            navMeshAgent.SetDestination(npcPos);
            GetComponent<Rigidbody>().isKinematic = false;
            Debug.LogWarning("npc move anim set speed");
            npcAnimator.SetFloat("moveSpeed", speed);
            npcAnimator.SetBool("stopped", false);
            if ((transform.position-npcPos).magnitude <= navMeshAgent.stoppingDistance)
            {
                moveToInitialPosition = false;
                Debug.LogWarning("npc move anim walk stop2");
                npcAnimator.SetFloat("moveSpeed", 0);
                npcAnimator.SetBool("stopped", true);
            }
        }
        if (navMeshAgent.isStopped)
        {
            Debug.LogWarning("npc move anim walk stop");
            npcAnimator.SetFloat("moveSpeed", 0);
            npcAnimator.SetBool("stopped", true);
        }
    }

    public void Stop(bool setStop)
    {
        navMeshAgent.isStopped = setStop;
    }
} 
