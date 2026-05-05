using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUIElement : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI stackText;

    public void Set(Item item, int count)
    {
        icon.sprite = item.itemIcon;

        stackText.text = count > 1 ? "x" + count : "";
    }
}