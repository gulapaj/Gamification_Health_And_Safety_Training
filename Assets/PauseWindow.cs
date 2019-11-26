using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseWindow : Menu
{
    public override void OnHideMenu(string options)
    {
        base.OnHideMenu(options);
    }

    public override void OnShowMenu(string options)
    {
        base.OnShowMenu(options);
    }

    public void ExitGame()
    {
        //resets the HUD gameobjects
        TimerManager.Instance.ResetTimer();
        HealthRecord.Instance.ResetHealth();
        TrophyHandler.Instance.ResetTrophy();

        GameManager.Instance.ExitGame();
    }

    public void ContinueGame()
    {
        //enabled player's movement
        PlayerMovement.Instance.enabled = true;

        //start the timer again
        TimerManager.Instance.triggerPause(false);

        //remove the pause screen
        MenuManager.Instance.HideMenu(menuClassifier);
    }
}
