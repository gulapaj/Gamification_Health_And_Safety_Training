using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public MenuClassifier HomeScreen;

    public bool tutorial = false;

    public void ExitGame()
    {
        //resets the HUD gameobjects
        TimerManager.Instance.ResetTimer();
        HealthRecord.Instance.ResetHealth();
        TrophyHandler.Instance.ResetTrophy();

        SceneLoader.Instance.UnloadScene(SceneManager.GetActiveScene().path);
        // UNLOAD ART SCENE EVENTUALLY

        MenuManager.Instance.HideAllMenus();
        // REPLACE WITH GAMEOVER SCREEN
        MenuManager.Instance.ShowMenu(HomeScreen); 
    }

    //func for getting database username

    //func for time (overall time for each level)

    //func for score (each correct answer)

    //func for life/health
}
