using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : Singleton<TimerManager>
{
    private float time;
    public Text timeLabel;
    private bool pauseTimer = false;

    void Start()
    {

    }

    void Update()
    {
        if (!pauseTimer) {
            time += Time.deltaTime;

            var minutes = Mathf.Floor(time / 60);
            var seconds = time % 60;

            //update the label value
            timeLabel.text = string.Format("{0:00} : {1:00}", minutes, seconds);
        }     
    }

    public void triggerPause(bool pause)
    {
        pauseTimer = pause;
    }

    public void ResetTimer()
    {
        pauseTimer = false;
        time = 0;
    }
    
}
