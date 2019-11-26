using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameOverManager : Singleton<GameOverManager>
{
    public Text score;
    public Text health;
    public Text message;
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(HealthRecord.Instance.life.Count == 0)
        {
            message.text = "Please try again!";
        }
        else
        {
            message.text = "Well done! You finish the level!";
        }


        score.text = "Total Score: " + GameRecord.Instance.score;


        health.text = "Health Remaining: " + GameRecord.Instance.health;
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("Score", 20);
        form.AddField("UserId", 1);
        form.AddField("LevelId", 1);

        using (UnityWebRequest www = UnityWebRequest.Post("http://142.55.32.86:50171/CapstoneWeb/api/userlevels", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }
}
