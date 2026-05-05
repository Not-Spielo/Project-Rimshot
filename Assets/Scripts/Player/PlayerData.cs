/*=============================================================================
Script Name:    PlayerData.cs
Last Edited:    2026-05-05
Contributors:   Grant Harvey
Description:    Store player items and their effects.
=============================================================================*/
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEditor.Progress;

[System.Serializable]
public class Items
{
    public Item item;
    public int count;
}

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    // Roguelike Items
    [HideInInspector] public List<Items> items = new List<Items>();

    // Roguelike Upgrade Modifiers
    [HideInInspector] public int strokesLostPerThrowAddition = 0;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        RecalculateStats();
    }

    /* GH - Sets all roguelike variables to default */
    /* NOTE!!! - Every New Roguelike Modifier should be added here with default values */
    public void SetModifiersToDefault()
    {
        strokesLostPerThrowAddition = 0;
    }

    /* GH - Add Items to PlayerData Object */
    public void AddItem(Item newItem)
    {
        Items existing = items.Find(i => i.item == newItem);

        if (existing != null)
        {
            existing.count++;
        }
        else
        {
            items.Add(new Items
            {
                item = newItem,
                count = 1
            });
        }

        newItem.Apply(this);

        GameplayManager.Instance.RefreshItemUI();
    }

    /* GH - Calculate Roguelike Upgrade Modifiers based on Items, each item may have multiple duplicates so take that into account */
    public void RecalculateStats()
    {
        SetModifiersToDefault();

        foreach (var item in items)
        {
            for (int i = 0; i < item.count; i++)
            {
                item.item.Apply(this);
            }
        }
    }

}