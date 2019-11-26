using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWindow : Menu
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
        MenuManager.Instance.HideMenu(menuClassifier);

        GameManager.Instance.ExitGame();
    }

}
