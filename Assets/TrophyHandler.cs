using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TrophyHandler : Singleton<TrophyHandler>
{
    public GameObject trophy;
    public Vector3 buttonOffset = new Vector3(0, 0, 0);
    public Vector3 buttonOffsetIncrease = new Vector3(0, 0, 0);
    public List<GameObject> trophies;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //checks if progressbar gameObject has no children then create trophy/ies
        if(this.transform.childCount == 0)
        {
            CreateTrophy();
        }
    }

    public void CreateTrophy()
    {
        //stores the current scene
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log(currentScene);

       // if (SceneManager.GetActiveScene().name == currentScene)
      //  {
            //always targets the UI scene 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("UI"));

            //gets the current level and the total number of NPC
            GameObject level = StorageManager.Instance.currentLevel;
            int npcSize = level.GetComponent<LevelMono>().NPC.Count;

            Debug.Log(npcSize);

            Debug.Log("Creating Trophy");

            //instantiate the gameObject trophy based on the number of NPCs
            for (int i = 0; i < npcSize; i++)
            {
                Debug.Log(this.gameObject.transform);

                GameObject trophyPlacer = Instantiate(trophy, this.gameObject.transform, false);
                trophyPlacer.transform.localPosition += buttonOffset;

                //changes this to just add the number instead of the gameobject itself
                trophies.Add(trophyPlacer);

                //adds the extra x-position to the next instant of trophy
                buttonOffset += buttonOffsetIncrease;
            }

            Debug.Log("THis is the number of trophies::" + trophies.Count);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
      //  }
    }

    public void ResetTrophy()
    {
        //remove all instances of trophies
        for (int x = 0; x < trophies.Count; x++)
        {
            Destroy(this.trophies[x]);
        }

        //sets the trophy array to 0
        trophies.Clear();

        //sets PosX back to default
        buttonOffset.x = 93;
    }
}
