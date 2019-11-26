using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : Singleton<MenuManager>
{
    public MenuClassifier loadingScreen;

    private Dictionary<string, Menu> MenuList = new Dictionary<string, Menu>();

    public T GetMenu<T>(MenuClassifier menuClassifier) where T : Menu
    {
        if (MenuList.ContainsKey(menuClassifier.menuName))
        {
            return (T)MenuList[menuClassifier.menuName];
        }

        return null;
    }

    public void AddMenu(Menu menu, MenuClassifier menuClassifier)
    {
        if (MenuList.ContainsKey(menuClassifier.menuName))
        {
            Debug.LogError("Menu already exists: "+ menuClassifier.ToString());
        }
        else
        {
            MenuList.Add(menuClassifier.menuName, menu);
        }
    }

    public void ShowMenu(MenuClassifier menuClassifier, string options = "")
    {
        Menu menu;

        if (MenuList.TryGetValue(menuClassifier.menuName,out menu))
        {
            menu.OnShowMenu(options);
        }
        else
        {
            Debug.LogError("Can't find menu item: " + menuClassifier.menuName);
        }
    }

    public void HideMenu(MenuClassifier menuClassifier, string options = "")
    {
        Menu menu;

        if (MenuList.TryGetValue(menuClassifier.menuName, out menu))
        {
            menu.OnHideMenu(options);
        }
        else
        {
            Debug.LogError("Can't find menu: " + menuClassifier.menuName);
        }
    }

    public void HideAllMenus()
    {
        foreach (var menuItem in MenuList)
        {
            menuItem.Value.OnHideMenu("");
        }
    }
}
