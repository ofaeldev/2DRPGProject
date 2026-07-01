using UnityEngine;

public class ItemPickupInteractable : MonoBehaviour, IInteraction
{
    [SerializeField] private GameObject interactionPointTransform;
    [SerializeField] private string feedbackPopup;
    [SerializeField] private string feedbackPickup;
    [SerializeField] private string itemId;

    [Min(1)]
    [SerializeField] private int amount;

    public void Execute()
    {
        if (string.IsNullOrEmpty(itemId) || amount <= 0)
            return;

        if (!InventoryService.TryAddItem(itemId, amount))
            return;

        FeedbackService.StartFeedback(feedbackPickup);
        gameObject.SetActive(false);
    }

    public string GetPopUpText()
    {
        return feedbackPopup;
    }

    public Transform InteractionPoint()
    {
        return interactionPointTransform != null ? interactionPointTransform.transform : transform;
    }
}