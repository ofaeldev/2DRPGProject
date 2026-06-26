using UnityEngine;

public class FeedbackInteractable : MonoBehaviour, IInteraction
{
    public GameObject interactionPointTransform;
    public string popUpFeedbackMsg;
    public string interactionFeedbackMsg;
    public void Execute()
    {
        FeedbackService.StartFeedback(interactionFeedbackMsg);
    }
    public string GetPopUpText()
    {
        return popUpFeedbackMsg;
    }

    public Transform InteractionPoint()
    {
        return this.interactionPointTransform != null ? this.interactionPointTransform.transform : this.transform;
    }
}