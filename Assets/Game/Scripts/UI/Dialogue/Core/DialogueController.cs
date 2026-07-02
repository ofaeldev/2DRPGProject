using UnityEngine;

public class DialogueController : MonoBehaviour
{
    private DialoguePanelUI dialoguePanelUI;
    private DialogueFlowController dialogueFlowController;
    private string currentNpcName;
    private DialogueNode[] currentNodes;
    private DialogueNode currentNode;
    private bool isDialogueActive;
    private string currentConversationId;

    #region Unity Methods
    private void Awake()
    {
        dialoguePanelUI = GetComponent<DialoguePanelUI>();
        dialogueFlowController = new DialogueFlowController();
        dialoguePanelUI.HideDialogue();
    }

    private void OnEnable()
    {
        DialogueService.DialogueStartedSession += OnDialogueStartedSession;
        DialogueService.DialogueAdvanced += OnDialogueAdvanced;
        DialogueService.DialogueEnded += OnDialogueEnded;
    }

    private void OnDisable()
    {
        DialogueService.DialogueStartedSession -= OnDialogueStartedSession;
        DialogueService.DialogueAdvanced -= OnDialogueAdvanced;
        DialogueService.DialogueEnded -= OnDialogueEnded;
    }
    #endregion
    // New start (npcName + DialogueSession)
    private void OnDialogueStartedSession(string npcName, DialogueSession session)
    {
        if (session == null)
            return;

        StartDialogueInternal(npcName, ChooseNodes(session), session.conversationId, session.startNodeId);
    }

    private void StartDialogueInternal(string npcName, DialogueNode[] dialogueLines, string conversationId, string startNodeId)
    {
        if (isDialogueActive)
            return;

        if (dialogueLines == null || dialogueLines.Length == 0)
            return;

        currentNodes = dialogueLines;
        dialogueFlowController.BuildNodeMap(currentNodes, npcName);

        currentNpcName = npcName;
        currentConversationId = conversationId;

        // choose start node
        if (!string.IsNullOrWhiteSpace(startNodeId) && dialogueFlowController.FindNodeById(startNodeId) is DialogueNode startNode)
        {
            currentNode = startNode;
        }
        else
        {
            currentNode = currentNodes[0];
        }

        GameplayStateService.Lock(GameplayLockReason.Dialogue);

        WorldStateService.SetFlag(currentNode.flagConversation);
        
        dialoguePanelUI.ShowDialogue(currentNpcName, currentNode.text);
        isDialogueActive = true;
    }

    private void OnDialogueAdvanced()
    {
        if (currentNode == null)
            return;

        bool hasOption = currentNode.dialogueOption != null && currentNode.dialogueOption.Length > 0;

        if (hasOption)
        {
            dialoguePanelUI.ShowOptions(currentNode.dialogueOption, ChooseOption);
            return;
        }

        if (string.IsNullOrWhiteSpace(currentNode.nextId))
        {
            DialogueService.EndedDialogue();
            return;
        }

        DialogueNode nextNode = dialogueFlowController.FindNodeById(currentNode.nextId);

        if (nextNode != null)
        {
            currentNode = nextNode;
            dialoguePanelUI.ShowDialogue(currentNpcName, currentNode.text);
        }
        else
        {
            Debug.LogWarning($"Invalid next id: {currentNode.nextId} in npc {currentNpcName}");
            DialogueService.EndedDialogue();
        }
    }

    private void OnDialogueEnded()
    {
        GameplayStateService.Unlock(GameplayLockReason.Dialogue);

        dialoguePanelUI.HideDialogue();
        isDialogueActive = false;
        dialogueFlowController.Clear();

        if (!string.IsNullOrWhiteSpace(currentConversationId))
        {
            WorldStateService.MarkConversationComplete(currentConversationId);
            currentConversationId = null;
        }
    }

    #region Helpers
    public void ChooseOption(int optionIndex)
    {
        if (currentNode == null)
            return;

        if (currentNode.dialogueOption == null || currentNode.dialogueOption.Length == 0)
            return;

        if (optionIndex < 0 || optionIndex >= currentNode.dialogueOption.Length)
            return;

        DialogueOption choose = currentNode.dialogueOption[optionIndex];

        if(choose == null)
            return;

        if(!string.IsNullOrWhiteSpace(choose.flagOptions))
            WorldStateService.SetFlag(choose.flagOptions);
        
        if(!string.IsNullOrEmpty(choose.questToStart))
            QuestService.StartQuest(choose.questToStart);

        if(!string.IsNullOrEmpty(choose.questToComplete))
            QuestService.CompleteQuest(choose.questToComplete);

        if (string.IsNullOrWhiteSpace(choose.nextId))
        {
            DialogueService.EndedDialogue();
            return;
        }

        DialogueNode nextNode = dialogueFlowController.FindNodeById(choose.nextId);

        if (nextNode != null)
        {
            currentNode = nextNode;
            dialoguePanelUI.ShowDialogue(currentNpcName, currentNode.text);
        }
        else
        {
            Debug.LogWarning($"Invalid next id: {choose.nextId} in npc {currentNpcName}");
            DialogueService.EndedDialogue();
        }
    }

    private DialogueNode[] ChooseNodes(DialogueSession session)
    {
        return dialogueFlowController.BuildNodeSet(session);
    }
#endregion
}
