using UnityEngine;

public class PlayerInventoryController : MonoBehaviour
{
    [SerializeField] private InventoryPanelUI inventoryPanelUI;

    private bool isInventoryOpen;

    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventoryPanelUI == null)
        {
            Debug.LogWarning("PlayerInventoryController is missing an InventoryPanelUI reference.");
            return;
        }

        if (isInventoryOpen)
        {
            inventoryPanelUI.ShowInventory();
            GameplayStateService.Lock(GameplayLockReason.Inventory);
            return;
        }

        inventoryPanelUI.HideInventory();
        GameplayStateService.Unlock(GameplayLockReason.Inventory);
    }
}
