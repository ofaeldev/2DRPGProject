using System.Collections.Generic;
using UnityEngine;

public class InventoryViewModel
{
    public List<InventoryEntryViewModel> BuildEntries(InventoryDatabase inventoryDatabase)
    {
        var entries = new List<InventoryEntryViewModel>();

        if (inventoryDatabase == null)
        {
            Debug.LogWarning("Inventory view model is missing an inventory database.");
            return entries;
        }

        foreach (var item in InventoryService.GetAllItems())
        {
            ItemDefinition definition = inventoryDatabase.GetItemById(item.Key);

            if (definition == null)
            {
                Debug.LogWarning($"Inventory item '{item.Key}' has no definition in the database.");
                continue;
            }

            entries.Add(new InventoryEntryViewModel(definition.displayName, item.Value));
        }

        return entries;
    }
}

public class InventoryEntryViewModel
{
    public InventoryEntryViewModel(string displayName, int amount)
    {
        DisplayName = displayName;
        Amount = amount;
    }

    public string DisplayName { get; }
    public int Amount { get; }
}
