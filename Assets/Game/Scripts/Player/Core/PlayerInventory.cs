using UnityEngine;

/// <summary>
/// Gerencia a abertura/fechamento do inventário do jogador.
/// Conecta ao evento InventoryRequested do PlayerInputReader e delega para PlayerInventoryController.
/// </summary>
public class PlayerInventory : PlayerInputSubscriber
{
    [SerializeField] private PlayerInventoryController inventoryController;

    /// <summary>Conecta ao evento InventoryRequested do input reader.</summary>
    protected override void ConnectToInputEvents()
    {
        playerInputReader.InventoryRequested += OnInventory;
    }

    /// <summary>Desconecta do evento InventoryRequested do input reader.</summary>
    protected override void DisconnectFromInputEvents()
    {
        playerInputReader.InventoryRequested -= OnInventory;
    }

    /// <summary>Alterna o estado do inventário (aberto/fechado).</summary>
    private void OnInventory()
    {
        if (inventoryController == null)
        {
            Debug.LogError("[PlayerInventory] Missing PlayerInventoryController reference. Cannot toggle inventory.", this);
            return;
        }

        inventoryController.ToggleInventory();
        Debug.Log("[PlayerInventory] Inventory toggled", this);
    }
}