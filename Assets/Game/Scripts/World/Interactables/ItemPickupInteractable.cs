using UnityEngine;

public class ItemPickupInteractable : MonoBehaviour, IInteraction
{
    [SerializeField] private GameObject interactionPointTransform;
    [SerializeField] private string feedbackPopup;
    public ItemDefinition itemDefinition;
    public void Execute()
    {
        InventoryService.AddItem(itemDefinition.itemId, itemDefinition.amount);
        this.gameObject.SetActive(false);
    }

    public string GetPopUpText()
    {
        return feedbackPopup;
    }

    public Transform InteractionPoint()
    {
        return this.interactionPointTransform != null ? this.interactionPointTransform.transform : this.transform;
    }
}