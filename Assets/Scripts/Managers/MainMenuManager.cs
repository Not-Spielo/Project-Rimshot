using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
/*=============================================================================
Script Name:    <MainMenuManager>
Last Edited:    <2026-05-13>
Contributors:   <Khidany>
Description:    <Manages Scene Transitions, and controller navigation in the Main Menu Scene. >
Notes:          <Player input invokes Unity events, go to Main Menu Manager, and with the dropdown is all the events we could program for QOL, under controls for UI is the close Submenu. Main Scene I wonder
if it can tell whats UI and what is Player or we will have a script determine that.>
=============================================================================*/
public class MainMenuManager : MonoBehaviour
{

    [Header("Setup")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject initialSelection;

    // This stack remembers the buttons that opened previous menus
    private Stack<GameObject> menuHistory = new Stack<GameObject>();
    private GameObject lastSelectedOnController;

    void Start()
    {
        lastSelectedOnController = initialSelection;
        EventSystem.current.SetSelectedGameObject(initialSelection);
    }

    void Update()
    {
        // 1. Monitor selection for Controller Recovery
        if (playerInput.currentControlScheme == "Gamepad")
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                // Constantly remember what the controller is looking at
                lastSelectedOnController = EventSystem.current.currentSelectedGameObject;
            }
            else if (lastSelectedOnController != null && lastSelectedOnController.activeInHierarchy)
            {
                // RECOVERY: If selection is lost (mouse click/Alt-tab), 
                // snap back to the EXACT last button when the stick is moved
                EventSystem.current.SetSelectedGameObject(lastSelectedOnController);
            }
        }
    }


    // Call this when you CLICK a button to open a new menu 
    public void OpenSubMenu(GameObject firstButtonInNewMenu)
    {
        // Remember the button that opened this menu so we can return to it later
        menuHistory.Push(EventSystem.current.currentSelectedGameObject);

        // Move focus to the new menu
        EventSystem.current.SetSelectedGameObject(firstButtonInNewMenu);
    }

    // Call this when you press Cancel
    public void CloseSubMenu()
    {
        if (menuHistory.Count > 0)
        {
            // Pop the previous button off the stack
            GameObject previousButton = menuHistory.Pop();

            // Set selection back to the button that opened the menu
            EventSystem.current.SetSelectedGameObject(previousButton);
        }
    }

    // --- DEVICE SWAP LOGIC ---

    public void OnControlsChanged()
    {
        if (playerInput.currentControlScheme == "Gamepad")
        {
            // When switching BACK to controller, restore the specific last button
            EventSystem.current.SetSelectedGameObject(lastSelectedOnController);
        }
        else
        {
            // Optional: clear selection for mouse for a cleaner look
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void PlayGame()
    {
        // Opens Scene 1 in the Scene List which rn is SampleScene Adjust if needed
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
