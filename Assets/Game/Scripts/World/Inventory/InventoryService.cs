using System;
using System.Collections.Generic;

public static class InventoryService
{
    public static event Action<string, int> ItemAdded;
    public static event Action<string, int> ItemRemoved;
    private static readonly Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
    private static bool hasLoadedFromDisk;

    private static void EnsureLoaded()
    {
        if (hasLoadedFromDisk)
            return;

        LoadFromDisk();
    }

    public static void AddItem(string itemId, int amount)
    {
        EnsureLoaded();
        TryAddItem(itemId, amount);
    }

    public static bool TryAddItem(string itemId, int amount)
    {
        if (string.IsNullOrEmpty(itemId) || amount <= 0)
            return false;

        if (inventoryItems.ContainsKey(itemId))
        {
            inventoryItems[itemId] += amount;
        }
        else
        {
            inventoryItems[itemId] = amount;
        }

        ItemAdded?.Invoke(itemId, amount);
        return true;
    }

    public static void RemoveItem(string itemId, int amount)
    {
        TryRemoveItem(itemId, amount);
    }

    public static bool TryRemoveItem(string itemId, int amount)
    {
        EnsureLoaded();

        if (string.IsNullOrEmpty(itemId) || amount <= 0)
            return false;

        if (!inventoryItems.TryGetValue(itemId, out int currentAmount))
            return false;

        if (currentAmount < amount)
            return false;

        int newAmount = currentAmount - amount;

        if (newAmount <= 0)
        {
            inventoryItems.Remove(itemId);
        }
        else
        {
            inventoryItems[itemId] = newAmount;
        }

        ItemRemoved?.Invoke(itemId, amount);
        return true;
    }

    public static bool HasItem(string itemId)
    {
        EnsureLoaded();
        return !string.IsNullOrEmpty(itemId) && inventoryItems.ContainsKey(itemId);
    }

    public static int GetItemAmount(string itemId)
    {
        EnsureLoaded();

        if (string.IsNullOrEmpty(itemId))
            return 0;

        if (inventoryItems.TryGetValue(itemId, out int amount))
            return amount;

        return 0;
    }

    public static IReadOnlyDictionary<string, int> GetAllItems()
    {
        EnsureLoaded();
        return new Dictionary<string, int>(inventoryItems);
    }

    public static void ClearAll()
    {
        inventoryItems.Clear();
    }

    public static void SaveToDisk()
    {
        EnsureLoaded();
        InventoryPersistence.Save(inventoryItems);
    }

    public static void LoadFromDisk()
    {
        var loadedInventory = InventoryPersistence.Load();
        inventoryItems.Clear();

        foreach (var pair in loadedInventory)
        {
            inventoryItems[pair.Key] = pair.Value;
        }

        hasLoadedFromDisk = true;
    }
}