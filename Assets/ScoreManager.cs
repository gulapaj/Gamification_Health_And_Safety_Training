using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

public class ScoreManager : Singleton<ScoreManager>
{
    public Flowchart flowchart;
    public int score;

    // Start is called before the first frame update
    void Start()
    {
        //gets the instance of flowchart after it got spawned
        flowchart = FungusTesting.Instance.flowchart;
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddPoint()
    {
        score++;
    }
}
