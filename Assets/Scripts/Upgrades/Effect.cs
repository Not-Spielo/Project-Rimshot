/*=============================================================================
Script Name:    Effect.cs
Last Edited:    2026-05-05
Contributors:   Grant Harvey
Description:    Simple Effect script to add a variety of effects we can edit
=============================================================================*/
using UnityEngine;

public abstract class Effect : ScriptableObject
{
    public abstract void Apply(PlayerData data);
}
