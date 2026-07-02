using System;
using System.Collections.Generic;

public static class InventoryService
{
    public static event Action<string, int> ItemAdded;
    public static event Action<string, int> ItemRemoved;
    public static event Action InventoryLoaded; // Evento quando inventário é carregado
    private static readonly Dictionary<string, int> inventoryItems = new Dictionary<string, int>();
    
    public static void AddItem(string itemId, int amount)
    {
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
        return !string.IsNullOrEmpty(itemId) && inventoryItems.ContainsKey(itemId);
    }

    public static int GetItemAmount(string itemId)
    {
        if (string.IsNullOrEmpty(itemId))
            return 0;

        if (inventoryItems.TryGetValue(itemId, out int amount))
            return amount;

        return 0;
    }

    public static IReadOnlyDictionary<string, int> GetAllItems()
    {
        return new Dictionary<string, int>(inventoryItems);
    }

    public static void ClearAll()
    {
        inventoryItems.Clear();
    }

    /// <summary>
    /// Adiciona um item diretamente ao inventário, sem disparar eventos.
    /// Usado durante o carregamento do save.
    /// </summary>
    public static bool TryLoadItem(string itemId, int amount)
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

        return true;
    }

    /// <summary>
    /// Notifica que o inventário foi completamente carregado.
    /// </summary>
    public static void NotifyInventoryLoaded()
    {
        InventoryLoaded?.Invoke();
    }
}