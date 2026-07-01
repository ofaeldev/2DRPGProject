using TMPro;
using UnityEngine;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField] private TMP_Text displayName;
    [SerializeField] private TMP_Text amountText;

    public void SetItem(string itemName, int amount)
    {
        if (displayName != null)
        {
            displayName.text = itemName;
        }
        else
        {
            Debug.LogWarning("Inventory item UI is missing a displayName reference.");
        }

        if (amountText != null)
        {
            amountText.text = $"x{amount}";
        }
        else
        {
            Debug.LogWarning("Inventory item UI is missing an amountText reference.");
        }
    }
}