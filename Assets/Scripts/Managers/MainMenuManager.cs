using UnityEngine;
using UnityEngine.SceneManagement;
/*=============================================================================
Script Name:    <MainMenuManager>
Last Edited:    <2026-04-05>
Contributors:   <Khidany>
Description:    <Manages Scene Transitions in the Main Menu Scene. Also thinking of making the options stuff here but idk >
Notes:          <Any notes neccesary, you can remove this if not needed>
=============================================================================*/
public class MainMenuManager : MonoBehaviour
{
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
