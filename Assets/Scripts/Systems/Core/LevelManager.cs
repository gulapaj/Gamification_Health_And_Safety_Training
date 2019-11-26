using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    public MenuClassifier gameOver;
    public MenuClassifier HUD;
    public bool levelDone;
    public bool x;

    public Vector3 layoutRotation;

    public GameObject playerReference;

    // Start is called before the first frame update
    void Start()
    {
        playerReference = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {


        //checks if HUD is active
        if (MenuManager.Instance.GetMenu<Menu>(HUD).isActiveAndEnabled)
        {
            this.gameObject.GetComponent<GameRecord>().enabled = true;
        }

        if (levelDone && !x)
        {
            x = true;
            MenuManager.Instance.HideMenu(HUD);
            MenuManager.Instance.ShowMenu(gameOver);
            Debug.Log("this is menu");
        }
    }

    //instantiation of flowcharts for dialogs
   // public void 

}
