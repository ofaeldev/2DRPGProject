using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    private string currentNpcName;
    private DialogueNode[] currentNodes;
    private DialogueNode currentNode;
    private bool isDialogueActive;
    private DialoguePanelUI dialoguePanelUI;
    private string currentConversationId;

    private Dictionary<string, DialogueNode> validateId = new Dictionary<string, DialogueNode>();

    #region Unity Methods
    private void Awake()
    {
        dialoguePanelUI = GetComponent<DialoguePanelUI>();
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

        validateId.Clear();

        currentNodes = dialogueLines;

        for (int i = 0; i < currentNodes.Length; i++)
        {
            string id = currentNodes[i].id;
            if (string.IsNullOrWhiteSpace(id))
            {
                Debug.LogWarning($"Empty node id at index {i} for npc {npcName}");
                continue;
            }

            if (validateId.ContainsKey(id))
            {
                Debug.LogWarning($"Duplicate node id {id} in dialogue for npc {npcName}");
            }
            else
            {
                validateId[id] = currentNodes[i];
            }
        }

        currentNpcName = npcName;
        currentConversationId = conversationId;

        // choose start node
        if (!string.IsNullOrWhiteSpace(startNodeId) && validateId.TryGetValue(startNodeId, out DialogueNode startNode))
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

        DialogueNode nextNode = FindNodeById(currentNode.nextId);

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
        validateId.Clear();

        if (!string.IsNullOrWhiteSpace(currentConversationId))
        {
            WorldStateService.MarkConversationComplete(currentConversationId);
            currentConversationId = null;
        }
    }

    #region Helpers
    private DialogueNode FindNodeById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        if (validateId.TryGetValue(id, out DialogueNode node))
            return node;

        return null;
    }

    public void ChooseOption(int optionIndex)
    {
        if (currentNode == null)
            return;

        if (currentNode.dialogueOption == null || currentNode.dialogueOption.Length == 0)
            return;

        if (optionIndex < 0 || optionIndex >= currentNode.dialogueOption.Length)
            return;

        DialogueOption choose = currentNode.dialogueOption[optionIndex];

        WorldStateService.SetFlag(choose.flagOptions.ToString());

        if (string.IsNullOrWhiteSpace(choose.nextId))
        {
            DialogueService.EndedDialogue();
            return;
        }

        DialogueNode nextNode = FindNodeById(choose.nextId);

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

    private bool HasNodes(DialogueNode[] nodes)
    {
        return nodes != null && nodes.Length > 0;
    }

    private DialogueNode[] ChooseNodes(DialogueSession session)
    {
        if(session == null)
            return null;
        
        DialogueNode[] nodesToUse = session.firstTimeNodes;

        if(session.worldStateBranches == null)
            return nodesToUse;

        for (int i = 0; i < session.worldStateBranches.Length; i++)
        {
            
            DialogueBranch branch = session.worldStateBranches[i];

            if(branch == null)
                continue;
            
            if(string.IsNullOrEmpty(branch.requiredFlag))
                continue;
            
            if(!HasNodes(branch.nodes))
                continue;
            
            if(!WorldStateService.HasFlag(branch.requiredFlag))
                continue;
                
            nodesToUse = branch.nodes;
            break;
        }

        return nodesToUse;
    }
#endregion
}
