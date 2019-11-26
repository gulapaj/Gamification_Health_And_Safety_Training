using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class Menu : MonoBehaviour
{
    public MenuClassifier menuClassifier;

    public enum StartMenuState { Ignore, Active, Disabled,}
    public StartMenuState startMenuState = StartMenuState.Active;

    [Serializable]
    public class RefreshMenuEvent : UnityEvent { }
    public RefreshMenuEvent onRefreshMenu = new RefreshMenuEvent();

    private Animator animator;

    public bool resetPosition = true;

    public bool isOpen
    {
        get
        {
            if(animator != null)
            {
                return animator.GetBool("isOpen");
            }
            return false;
        }

        set
        {
            if(value == true)
            {
                gameObject.SetActive(true);
                onRefreshMenu.Invoke();
            }

            if(animator != null)
            {
                animator.SetBool("isOpen", value);
            }
        }
    }

    public void Refresh()
    {
        onRefreshMenu.Invoke();
    }

    public virtual void OnShowMenu(string options) { isOpen = true; }
    public virtual void OnHideMenu(string options)
    {
        isOpen = false;
        if (DebugManager.Instance.useMenuAnimations == false) { gameObject.SetActive(false); }
    }

    public virtual void AnimationCompleted()
    {
        if(isOpen == false) { gameObject.SetActive(false); }
    }

    public virtual void Awake()
    {
        animator = GetComponent<Animator>();

        var rect = GetComponent<RectTransform>();

        if(resetPosition == true) { rect.localPosition = Vector3.zero; }
    }

    public virtual void Start()
    {
        MenuManager.Instance.AddMenu(this, menuClassifier);

        switch (startMenuState)
        {
            case StartMenuState.Active:
                gameObject.SetActive(true);
                isOpen = true;
                break;
            case StartMenuState.Disabled:
                gameObject.SetActive(false);
                isOpen = false;
                break;
        }
    }
}
