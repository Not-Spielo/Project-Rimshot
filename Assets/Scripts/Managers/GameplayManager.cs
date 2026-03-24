/*=============================================================================
Script Name:    GameplayManager.cs
Last Edited:    2026-03-24
Contributors:   Grant Harvey
Description:    Manage variables and such for gameplay
=============================================================================*/
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager Instance;

    [Header("Disc Settings")]
    [HideInInspector] public bool diskInFlight;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        diskInFlight = false;
    }
}
