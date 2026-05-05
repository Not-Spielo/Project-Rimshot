/*=============================================================================
Script Name:    Item.cs
Last Edited:    2026-05-05
Contributors:   Grant Harvey
Description:    Simple Item scriptable object
=============================================================================*/
using UnityEngine;
using System.Collections.Generic;

public abstract class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;

    public List<Effect> effects;

    public void Apply(PlayerData data)
    {
        foreach (var effect in effects)
        {
            effect.Apply(data);
        }
    }
}

