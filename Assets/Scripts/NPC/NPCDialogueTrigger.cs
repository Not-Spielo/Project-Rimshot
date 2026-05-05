/*=============================================================================
Script Name:    NPCDialogueTrigger.cs
Last Edited:    2026-05-05
Contributors:   Grant Harvey
Description:    Store Dialogue and Dialogue Reward Data for each NPC and trigger dialogue when player gets in range
=============================================================================*/
using System;
using System.Collections.Generic;
using UnityEditorInternal.VersionControl;
using UnityEngine;

/* GH - Set Dialogue Name and Character Portrait */
[System.Serializable] public class DialogueCharacter
{
    public string name;
    public Sprite characterPortrait;
}

/* GH - Set Line for Dialogue */
[System.Serializable] public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}

/* GH - Store Dialogue and dialogue reward data */
[System.Serializable] public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
    public List<Item> rewardItems = new List<Item>();
}

public class NPCDialogueTrigger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Dialogue dialogue;

    /* GH - start dialogue */
    public void TriggerDialogue()
    {
        DialogueManager.Instance.StartDialogue(dialogue);
    }

    /* GH - Trigger dialogue when user enters area */
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            TriggerDialogue();
        }
    }
}
