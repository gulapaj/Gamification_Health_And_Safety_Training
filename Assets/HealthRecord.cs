using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthRecord : Singleton<HealthRecord>
{
    public GameObject lifeTest;
    public Vector3 buttonOffset = new Vector3(0, 0, 0);
    public Vector3 buttonOffsetIncrease = new Vector3(0, 0, 0);
    public MenuClassifier gameOver;
    public MenuClassifier HUD;

    public List<GameObject> life;

    // Start is called before the first frame update
    void Start()
    {
        CreateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        if(life.Count == 0)
        {
            MenuManager.Instance.HideMenu(HUD);
            MenuManager.Instance.ShowMenu(gameOver);
        }
    }

    public void RemoverArray()
    {
            Destroy(this.life[0]);
            this.life.RemoveAt(0);
    }

    public void ResetHealth()
    {
        if(life.Count != 3) {
            //remove all instances of health
            for(int x = 0; x < life.Count; x++)
            {
                Destroy(this.life[x]);
            }

            //sets the life array to 0
            life.Clear();

            //sets the position back to the initial values
            buttonOffset.x = 14;
            buttonOffset.y = -40;

            //instantiate health
            CreateHealth();
        }

    }

    public void CreateHealth()
    {
        //stores the current scene
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log(currentScene);

        if (SceneManager.GetActiveScene().name == currentScene)
        {
            //always targets the UI scene 
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("UI"));

            int maxLife = 3;

            for (int i = 0; i < maxLife; i++)
            {
                Debug.Log(this.gameObject.transform);

                GameObject healthPlacer = Instantiate(lifeTest, this.gameObject.transform, false);
                healthPlacer.transform.localPosition += buttonOffset;

                life.Add(healthPlacer);

                //adds the extra x-position to the next instant of health
                buttonOffset += buttonOffsetIncrease;
            }

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(currentScene));
        }

    }


}
