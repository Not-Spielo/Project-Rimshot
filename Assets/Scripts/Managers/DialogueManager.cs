/*=============================================================================
Script Name:    DialogueManager.cs
Last Edited:    2026-05-12
Contributors:   Grant Harvey
Description:    Manage variables and such for Dialogue with NPC's
=============================================================================*/
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Settings")]
    [SerializeField] private GameObject itemBox;
    [SerializeField] private Image characterIcon;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private TextMeshProUGUI dialogueArea;
    [SerializeField] private Animator animator;
    [SerializeField] private float dialogSpeed = 0.2f;

    // Settings to be set during dialog
    [HideInInspector] public Queue<DialogueLine> lines;
    [HideInInspector] public bool isDialogueActive = false;
    private Dialogue currentDialogue;

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        isDialogueActive = false;
        lines = new Queue<DialogueLine>();
    }

    /* GH - Set dialogue variables then start it */
    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        currentDialogue = dialogue; 
        animator.Play("show");
        lines.Clear();

        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
        DisplayNextDialogueLine();
    }

    /* GH - end dialogue if over, otherwise proceed to next line */
    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = lines.Dequeue();

        characterIcon.sprite = currentLine.character.characterPortrait;
        characterName.text = currentLine.character.name;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(currentLine));
    }

    /* GH - animation to type line */
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(dialogSpeed);
        }
    }

    /* GH - hide dialogue box, set variables, give rewards */
    void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("hide");

        GiveRewards();
    }

    /* GH - run function to show item choice popup */
    private void GiveRewards()
    {
        if (currentDialogue == null) return;

        ChooseItem.Instance.itemChoiceAmount = currentDialogue.amountToGive;
        ChooseItem.Instance.ChooseItems(currentDialogue.chooseBetweenHowManyItems, currentDialogue.canSkip, currentDialogue.rewardItems);

        currentDialogue = null;
    }
}
