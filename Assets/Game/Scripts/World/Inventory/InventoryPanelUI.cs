using UnityEngine;

public class InventoryPanelUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Transform itemContainer;
    [SerializeField] private InventoryDatabase inventoryDatabase;

    private readonly InventoryViewModel inventoryViewModel = new InventoryViewModel();

    private void Awake()
    {
        HideInventory();
    }

    private void Start()
    {
        RefreshInventory();
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

        if (itemPrefab == null)
        {
            Debug.LogWarning("Inventory panel is missing an item prefab.");
            return;
        }

        foreach (var entry in inventoryViewModel.BuildEntries(inventoryDatabase))
        {
            GameObject itemObject = Instantiate(itemPrefab, itemContainer);
            InventoryItemUI itemUI = itemObject.GetComponent<InventoryItemUI>();

            if (itemUI == null)
            {
                Debug.LogWarning($"Inventory prefab '{itemPrefab.name}' does not contain an InventoryItemUI component.");
                Destroy(itemObject);
                continue;
            }

            itemUI.SetItem(entry.DisplayName, entry.Amount);
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