/*=============================================================================
Script Name:    GameplayManager.cs
Last Edited:    2026-05-18
Contributors:   Grant Harvey
Description:    Manage variables and such for gameplay
=============================================================================*/
using Unity.VisualScripting.Antlr3.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("Disc Settings")]
    [HideInInspector] public bool diskInFlight;

    [Header("Pause Menu")]
    public GameObject menuUI;
    [HideInInspector] public bool isPaused;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI strokesTillDeathText;
    [SerializeField] private int strokesTillDeath = 5;

    [Header("Game Needed")]
    [SerializeField] private BoxCollider DiscBasket;
    [SerializeField] private GameObject Player;

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

    // KR Toggle Pausing during game
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
        strokesTillDeathText.text = strokesTillDeath + " Strokes Till Death";
    }

    private void Update()
    {
        // Stroke Out
        if ( (strokesTillDeath <= 0) && (diskInFlight == false) )
        {
            GameLost();
        }

        // Win
        if ((DiscBasket.bounds.Contains(((GameObject)Player).transform.position)) && (diskInFlight == false))
        {
            GameWin();
        }
    }

    public void UpdateStrokes(int strokeLoss)
    {
        strokesTillDeath -= strokeLoss;
        strokesTillDeathText.text = strokesTillDeath + " Strokes Till Death";
    }

    private void GameLost()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameWin()
    {
        strokesTillDeathText.text = "You Win!";
    }
}
