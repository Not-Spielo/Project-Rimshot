/*=============================================================================
Script Name:    ChooseItem.cs
Last Edited:    2026-05-12
Contributors:   Grant Harvey
Description:    Update UI for Choosing Item and add item to PlayerData
=============================================================================*/
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

[System.Serializable] public class ItemChoiceUI
{
    public GameObject item;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public Image icon;
    public Button selectButton;
}

public class ChooseItem : MonoBehaviour
{
    public static ChooseItem Instance;
    [HideInInspector] public bool isItemSelectionActive = false;
    [HideInInspector] public int itemChoiceAmount = 1;
    private List<Item> currentItems;
    private bool currentCanSkip;
    private int currentItemsToChooseFrom;

    [Header("References")]
    [SerializeField] private Button skipButton;
    [SerializeField] private List<ItemChoiceUI> choiceUI = new List<ItemChoiceUI>();
    [SerializeField] private TextMeshProUGUI SelectItemText;
    [SerializeField] private Animator animator;

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        isItemSelectionActive = false;
        SelectItemText.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        for (int i = 0; i < choiceUI.Count; i++)
            choiceUI[i].item.gameObject.SetActive(false);
    }

    /* GH - Update UI for Selecting Item */
    public void ChooseItems(int itemsToChooseFrom, bool canSkip, List<Item> items)
    {
        animator.Play("show");
        isItemSelectionActive = true;
        currentItems = items;
        currentItemsToChooseFrom = itemsToChooseFrom;
        currentCanSkip = canSkip;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Set Item Box Settings
        for (int i = 0; i < itemsToChooseFrom; i++)
        {
            choiceUI[i].item.gameObject.SetActive(true);

            Item item = items[i];

            // Set item Stats
            choiceUI[i].icon.sprite = item.itemIcon;
            choiceUI[i].itemName.text = item.itemName;
            choiceUI[i].description.text = item.itemDescription;

            // Set Item onClick 
            choiceUI[i].selectButton.onClick.RemoveAllListeners();
            choiceUI[i].selectButton.onClick.AddListener(() => { ItemSelectedButton(item); });
        }

        skipButton.gameObject.SetActive(canSkip);
        SetSelectItemText();
    }

    /* GH - give item to player */
    public void ItemSelectedButton(Item item)
    {
        PlayerData.Instance.AddItem(item);

        StartCoroutine(CheckRunChoiceAgain());
    }

    /* GH - Run Item Choice Again or Close Item UI (Can be called for Skip */
    public IEnumerator CheckRunChoiceAgain()
    {
        itemChoiceAmount--;

        if (itemChoiceAmount > 0)
        {
            ChooseItems(currentItemsToChooseFrom, currentCanSkip, currentItems);
            yield break;
        }

        // End Item Selection
        animator.Play("hide");
        yield return new WaitForSeconds(2);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isItemSelectionActive = false;
        SelectItemText.gameObject.SetActive(false);
        skipButton.gameObject.SetActive(false);
        for (int i = 0; i < choiceUI.Count; i++)
            choiceUI[i].item.gameObject.SetActive(false);
    }

    public void SkipButton()
    {
        StartCoroutine(CheckRunChoiceAgain());
    }

    /* GH - Set UI Text to "Select Your Xth Item" */
    private void SetSelectItemText()
    {
        SelectItemText.gameObject.SetActive(true);
        if (PlayerData.Instance.totalNumberOfItems == 0 || PlayerData.Instance.totalNumberOfItems == 20 || PlayerData.Instance.totalNumberOfItems == 30)
            SelectItemText.text = $"Select Your {PlayerData.Instance.totalNumberOfItems + 1}st Item";
        else if (PlayerData.Instance.totalNumberOfItems == 1 || PlayerData.Instance.totalNumberOfItems == 21 || PlayerData.Instance.totalNumberOfItems == 31)
            SelectItemText.text = $"Select Your {PlayerData.Instance.totalNumberOfItems + 1}nd Item";
        else if (PlayerData.Instance.totalNumberOfItems == 2 || PlayerData.Instance.totalNumberOfItems == 22 || PlayerData.Instance.totalNumberOfItems == 32)
            SelectItemText.text = $"Select Your {PlayerData.Instance.totalNumberOfItems + 1}rd Item";
        else
            SelectItemText.text = $"Select Your {PlayerData.Instance.totalNumberOfItems + 1}th Item";
    }
}
