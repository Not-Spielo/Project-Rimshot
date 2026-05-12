/*=============================================================================
Script Name:    VariableModifierEffect.cs
Last Edited:    2026-05-05
Contributors:   Grant Harvey
Description:    Simple Effect script to add a strokes per loss modifer 
=============================================================================*/
using UnityEngine;


/* NOTE - Any time you add a "Roguelike Upgrade Modifier" variable to the PlayerData Script, you should add the variable here as well */
[CreateAssetMenu(menuName = "Effects/Simple Variable Modifier")]
public class VariableModifierEffect : Effect
{
    public int strokeLossPerThrowModifier;

    // Add Future Variables Here ^^^

    public override void Apply(PlayerData data)
    {
        data.strokesLostPerThrowAddition += strokeLossPerThrowModifier;

    // Add Future Variables Here ^^^
}
}