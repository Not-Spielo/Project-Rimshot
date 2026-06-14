/*=============================================================================
Script Name:    GameplayManager.cs
Last Edited:    2026-06-14
Contributors:   Grant Harvey, Khidany Ruiz
Description:    Manage variables and such for gameplay
=============================================================================*/
using Unity.VisualScripting.Antlr3.Runtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("Disc Settings")]
    [HideInInspector] public bool diskInFlight;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenuUI;
    [HideInInspector] public bool isPaused;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI strokesTillDeathText;
    [SerializeField] private int strokesTillDeath = 5;
    [SerializeField] private GameObject itemContainer;
    [SerializeField] private GameObject itemPrefab;
    private List<GameObject> spawnedItems = new List<GameObject>();

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
        /* Don't Allow user to pause if choosing item */
        if (true == ChooseItem.Instance.isItemSelectionActive)
        {
            return;
        }

        isPaused = !isPaused;
        pauseMenuUI.SetActive(isPaused);

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

    /* GH - Update strokes with how much they lost */
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

    /* GH - Update strokes with how much they lost */
    public void RefreshItemUI()
    {
        // clear old Items in UI
        foreach (var obj in spawnedItems)
            Destroy(obj);

        spawnedItems.Clear();

        // rebuild item UI from PlayerData
        foreach (var item in PlayerData.Instance.items)
        {
            GameObject ui = Instantiate(itemPrefab, itemContainer.transform);
            spawnedItems.Add(ui);

            ItemUIElement element = ui.GetComponent<ItemUIElement>();
            element.Set(item.item, item.count);
        }
    }
}
