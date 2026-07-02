using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class InventoryPersistence
{
    private static string GetPath()
    {
        return Path.Combine(Application.persistentDataPath, "inventory.json");
    }

    public static void Save(Dictionary<string, int> inventory)
    {
        var data = new InventorySaveData();

        foreach (var pair in inventory)
        {
            data.items.Add(new ItemSaveData
            {
                itemId = pair.Key,
                amount = pair.Value
            });
        }

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(), json);
    }

    public static Dictionary<string, int> Load()
    {
        string path = GetPath();

        if (!File.Exists(path))
            return new Dictionary<string, int>();

        string json = File.ReadAllText(path);
        InventorySaveData data = JsonUtility.FromJson<InventorySaveData>(json);

        var inventory = new Dictionary<string, int>();

        foreach (var item in data.items)
        {
            if (!string.IsNullOrEmpty(item.itemId) && item.amount > 0)
            {
                inventory[item.itemId] = item.amount;
            }
        }

        return inventory;
    }
}