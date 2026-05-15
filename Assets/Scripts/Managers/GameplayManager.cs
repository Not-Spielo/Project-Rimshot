/*=============================================================================
Script Name:    GameplayManager.cs
Last Edited:    2026-03-24
Contributors:   Grant Harvey
Description:    Manage variables and such for gameplay
=============================================================================*/
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("Disc Settings")]
    [HideInInspector] public bool diskInFlight;

    // KR - Pause Menu
    [Header("Pause Menu")]
    public GameObject menuUI;
    [HideInInspector] public bool isPaused;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        diskInFlight = false;
        isPaused = false;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        if (menuUI == null)
        {
            return;
        }
        else
        {
            menuUI.SetActive(isPaused);
        }
        // KR - avoid camera locking after disabling player input
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None; // Let the mouse move freely
            Cursor.visible = true;                 // Show the cursor
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // Snap mouse back to center
            Cursor.visible = false;                  // Hide the cursor
        }
    }
}
