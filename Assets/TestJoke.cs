using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using System;

public class TestJoke : MonoBehaviour
{
    private bool isColliding;
    public bool isPlayerCollided;
    public bool isTheSameJoke;


    public int usedJoke;


    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        //isColliding = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        //if (isColliding) return;
        //isColliding = true;
        flowchartController.Instance.StopPlayerMovement(true);
        if (!isTheSameJoke)
        {
            usedJoke = JokeDialogManager.Instance.GetJokeID();

            JokeDialogManager.Instance.flowchart.ExecuteBlock(JokeDialogManager.Instance.flowchart.FindBlock("Joke" + usedJoke),0,onComplete);
            isTheSameJoke = true;
            Debug.Log("new joke");
        }
        else
        {
            JokeDialogManager.Instance.flowchart.ExecuteBlock(JokeDialogManager.Instance.flowchart.FindBlock("Joke" + usedJoke), 0, onComplete);
            //JokeDialogManager.Instance.flowchart.ExecuteBlock("Joke" + usedJoke);
            //      JokeDialogManager.Instance.flowchart.ExecuteBlock("Joke" + joke);
            Debug.Log("used joke");
        }
    }

    private void onComplete()
    {
        flowchartController.Instance.StopPlayerMovement(false);
    }
}
