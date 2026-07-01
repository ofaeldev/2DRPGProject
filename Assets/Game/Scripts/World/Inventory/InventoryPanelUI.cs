using UnityEngine;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private InventoryDatabase inventoryDatabase;

    private void Awake()
    {
        HideInventory();
    }

    private void OnEnable()
    {
        InventoryService.ItemAdded += OnItemChanged;
        InventoryService.ItemRemoved += OnItemChanged;
    }

    private void OnDisable()
    {
        InventoryService.ItemAdded -= OnItemChanged;
        InventoryService.ItemRemoved -= OnItemChanged;
    }

    private void OnItemChanged(string itemId, int amount)
    {
        RefreshInventory();
    }

    private void RefreshInventory()
    {
        if (itemContainer == null)
        {
            Debug.LogWarning("Inventory panel is missing an item container.");
            return;
        }

        for (int i = itemContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(itemContainer.GetChild(i).gameObject);
        }

        if (inventoryDatabase == null)
        {
            Debug.LogWarning("Inventory panel is missing an inventory database.");
            return;
        }

        if (itemPrefab == null)
        {
            Debug.LogWarning("Inventory panel is missing an item prefab.");
            return;
        }

        foreach (var item in InventoryService.GetAllItems())
        {
            string itemId = item.Key;
            int amount = item.Value;

            ItemDefinition definition = inventoryDatabase.GetItemById(itemId);

            if (definition == null)
            {
                Debug.LogWarning($"Inventory item '{itemId}' has no definition in the database.");
                continue;
            }

            GameObject itemObject = Instantiate(itemPrefab, itemContainer);
            InventoryItemUI itemUI = itemObject.GetComponent<InventoryItemUI>();

            if (itemUI == null)
            {
                Debug.LogWarning($"Inventory prefab '{itemPrefab.name}' does not contain an InventoryItemUI component.");
                Destroy(itemObject);
                continue;
            }

            itemUI.SetItem(definition.displayName, amount);
        }
    }

    public void ShowInventory()
    {
        RefreshInventory();

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void HideInventory()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }
}