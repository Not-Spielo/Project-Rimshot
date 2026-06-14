using UnityEngine;
using UnityEngine.SceneManagement;
/*=============================================================================
Script Name:    Main Menu Manager
Last Edited:    2026-05-18
Contributors:   Khidany Ruiz
Description:    Handles Certain Interactions in the Main Menu Scene 

=============================================================================*/
public class MainMenuManager : BaseMenuManager
{

    [SerializeField] private GameObject optionsPanel;


    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}