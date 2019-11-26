using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRecord : Singleton<GameRecord>
{
    public string score;
    public string time;
    public string health;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int finalScore = ScoreManager.Instance.score;
        score = finalScore.ToString();

        string timeLabel = TimerManager.Instance.timeLabel.text;
        time = timeLabel;

        health = HealthRecord.Instance.life.Count.ToString();
    }
}
