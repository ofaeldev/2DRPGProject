
using UnityEngine;
[CreateAssetMenu(fileName = "NewInventoryItem", menuName = "Game/Inventory")]
public class InventoryDatabase : ScriptableObject
{
    [SerializeField] private ItemDefinition[] items;

    public ItemDefinition GetItemById(string itemId)
    {
        if(string.IsNullOrEmpty(itemId))
            return null;

        if(items == null)
            return null;
        
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i] == null)
                continue;
            
            if(items[i].itemId == itemId)
                return items[i];
        }

        return null;
    }
}