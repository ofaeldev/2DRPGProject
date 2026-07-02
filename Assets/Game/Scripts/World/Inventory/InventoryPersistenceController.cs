using UnityEngine;

public class InventoryPersistenceController : MonoBehaviour
{
    private void Awake()
    {
        InventoryService.LoadFromDisk();
    }

    private void OnEnable()
    {
        InventoryService.ItemAdded += HandleInventoryChanged;
        InventoryService.ItemRemoved += HandleInventoryChanged;
    }

    private void OnDisable()
    {
        InventoryService.ItemAdded -= HandleInventoryChanged;
        InventoryService.ItemRemoved -= HandleInventoryChanged;
    }

    private void HandleInventoryChanged(string itemId, int amount)
    {
        InventoryService.SaveToDisk();
    }
}