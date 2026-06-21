using UnityEngine;

public class NpcInteractable : MonoBehaviour, IInteraction
{
    [SerializeField] string npcName;
    public DialogueNode[] dialogueNode;
    public string popUpFeedbackMsg;
    public GameObject interactionPointTransform;
    public void Execute()
    {
        DialogueService.StartDialogue(npcName, dialogueNode);
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