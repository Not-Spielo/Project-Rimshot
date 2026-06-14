using UnityEngine;
using UnityEngine.SceneManagement;
/*=============================================================================
Script Name:    Mid Run Manager
Last Edited:    2026-05-18
Contributors:   Khidany Ruiz
Description:    Handles Certain Interactions in the Play Scene 

=============================================================================*/
public class MidRunManager : BaseMenuManager
{

    [SerializeField] private GameObject optionsPanel;

    public void ReturnToMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}