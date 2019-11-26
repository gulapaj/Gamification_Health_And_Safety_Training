using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : Menu
{
    public MenuClassifier screen;

    public override void OnHideMenu(string options)
    {
        base.OnHideMenu(options);
    }

    public override void OnShowMenu(string options)
    {
        base.OnShowMenu(options);
    }

    public void PauseGame()
    {
        Debug.Log("Here is a popup");

        //pause player's movement
        PlayerMovement.Instance.enabled = false;
        
        //pause the timer
        TimerManager.Instance.triggerPause(true);

        //add the pop-up window
        MenuManager.Instance.ShowMenu(screen);

        //Map.Instance.ShowMapMarkers();
    }
}
