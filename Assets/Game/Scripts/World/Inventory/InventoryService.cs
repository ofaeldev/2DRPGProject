using System;
using System.Collections.Generic;
using System.Diagnostics;

public static class InventoryService
{
    public static event Action<string, int> PickupItemOnInventory;
    public static event Action<string,int> RemoveItemOnInventory;
    public static Dictionary<string, int> inventoryItem = new Dictionary<string, int>();

    public static void AddItem(string itemId, int amount)
    {
        if(string.IsNullOrEmpty(itemId) || amount == 0)
            return;
        
        inventoryItem[itemId] = amount;        
        
        PickupItemOnInventory?.Invoke(itemId, amount);
    }

    public static void RemoveItem(string itemId, int amount)
    {
        if(string.IsNullOrEmpty(itemId) || amount == 0)
            return;
        
        if(inventoryItem.Count == 0)
            return;

        inventoryItem.Remove(itemId);

        RemoveItemOnInventory?.Invoke(itemId, amount);
    }

    public static bool HasItem(string itemId)
    {
        if(string.IsNullOrEmpty(itemId))
            return false;
        
        if(inventoryItem.Count == 0)
            return false;
        
        if(!inventoryItem.ContainsKey(itemId))
            return false;
        
        return true;
    }

    public static void GetItemAmount(string itemId)
    {
        if(string.IsNullOrEmpty(itemId))
            return;
        
        if(inventoryItem.Count == 0)
            return;

        if (inventoryItem.ContainsKey(itemId))
        {
            
        }
    }
}