/*=============================================================================
Script Name:    GameplayManager.cs
Last Edited:    2026-03-24
Contributors:   Grant Harvey
Description:    Manage variables and such for gameplay
=============================================================================*/
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("Disc Settings")]
    [HideInInspector] public bool diskInFlight;

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
