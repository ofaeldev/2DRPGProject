using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryPanelUI inventoryPanelUI;
    private PlayerInputReader playerInputReader;
    private bool isConnected;
    private bool isInventoryOpen;

    public void Initialize(PlayerInputReader playerInputReader)
    {
        this.playerInputReader = playerInputReader;

        if (isActiveAndEnabled)
        {
            Connect();
        }
    }

    private void OnEnable()
    {
        Connect();
    }

    private void OnDisable()
    {
        Disconnect();
    }

    private void Connect()
    {
        if (isConnected)
            return;

        if (playerInputReader == null)
            return;

        playerInputReader.InventoryRequested += OnInventory;
        isConnected = true;
    }

    private void Disconnect()
    {
        if (!isConnected)
            return;

        playerInputReader.InventoryRequested -= OnInventory;
        isConnected = false;
    }

    private void OnInventory()
    {
        isInventoryOpen = !isInventoryOpen;

        if (inventoryPanelUI == null)
        {
            Debug.LogWarning("PlayerInventory is missing an InventoryPanelUI reference.");
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