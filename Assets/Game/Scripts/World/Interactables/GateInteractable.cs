using UnityEngine;

public class GateInteractable : MonoBehaviour, IInteraction
{
    [SerializeField] private GameObject interactionPointTransform;
    public string feedbackBlock;
    public string feedbackOpen;
    public WorldRequirement worldRequirement;
    public void Execute()
    {
        if (worldRequirement == null || worldRequirement.IsMet())
        {
            this.gameObject.SetActive(false);  
            FeedbackService.StartFeedback(feedbackOpen);  
            return;      
        }

        FeedbackService.StartFeedback(feedbackBlock);
    }

    public string GetPopUpText()
    {        
        if(worldRequirement == null || worldRequirement.IsMet())
            return feedbackOpen;
        
        return feedbackBlock;
    }

    public Transform InteractionPoint()
    {
        return this.interactionPointTransform != null ? this.interactionPointTransform.transform : this.transform;
    }
}