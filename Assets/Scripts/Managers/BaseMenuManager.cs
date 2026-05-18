using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
/*=============================================================================
Script Name:    Base Menu Manager
Last Edited:    2026-05-18
Contributors:   Khidany Ruiz
Description:    Handles Basic Controller Navigations between scenes. 
Notes:          Further speficications in certains scenes can be in their own script.
=============================================================================*/
public abstract class BaseMenuManager : MonoBehaviour
{
    [Header("Base UI Setup")]
    [SerializeField] protected PlayerInput playerInput;
    [SerializeField] protected GameObject initialSelection;

    protected Stack<GameObject> menuHistory = new Stack<GameObject>();
    private GameObject lastSelectedOnController;

    protected virtual void Start()
    {
        if (initialSelection != null)
        {
            SetSelection(initialSelection);
        }
    }

    // KR Designed for Recovering after Alt tab
    protected virtual void Update()
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            GameObject current = EventSystem.current.currentSelectedGameObject;

            if (current != null)
            {
                lastSelectedOnController = current;
            }
            else if (lastSelectedOnController != null && lastSelectedOnController.activeInHierarchy)
            {
                EventSystem.current.SetSelectedGameObject(lastSelectedOnController);
            }
        }
    }

    // KR - Opens a new panel and remembers last pressed button
    public virtual void OpenSubMenu(GameObject firstButtonInNewMenu)
    {
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            menuHistory.Push(EventSystem.current.currentSelectedGameObject);
        }

        SetSelection(firstButtonInNewMenu);
    }

    // Returns to the previous button in the stack
    public virtual void CloseSubMenu()
    {
        if (menuHistory.Count > 0)
        {
            SetSelection(menuHistory.Pop());
        }
    }

    protected void SetSelection(GameObject target)
    {
        lastSelectedOnController = target;
        EventSystem.current.SetSelectedGameObject(target);
    }

    public virtual void OnControlsChanged()
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            SetSelection(lastSelectedOnController ?? initialSelection);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}