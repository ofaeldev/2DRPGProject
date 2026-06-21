using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    private string currentNpcName;
    private DialogueNode[] currentNodes;
    private DialogueNode currentNode;
    private bool isDialogueActive;
    private DialoguePanelUI dialoguePanelUI;
    
    private Dictionary<int, DialogueNode> validateId = new Dictionary<int, DialogueNode>();

    private void Awake()
    {
        dialoguePanelUI = GetComponent<DialoguePanelUI>();

        dialoguePanelUI.HideDialogue();
    }

    private void OnEnable()
    {
        DialogueService.DialogueStarted += OnDialogueStarted;
        DialogueService.DialogueAdvanced += OnDialogueAdvanced;
        DialogueService.DialogueEnded += OnDialogueEnded;
    }

    private void OnDisable()
    {
        DialogueService.DialogueStarted -= OnDialogueStarted;
        DialogueService.DialogueAdvanced -= OnDialogueAdvanced;
        DialogueService.DialogueEnded -= OnDialogueEnded;
    }

    private void OnDialogueAdvanced()
    {    
        if(currentNode == null)
            return;   

        bool hasOption = currentNode.dialogueOption != null &&
                            currentNode.dialogueOption.Length > 0;
                            

        if (hasOption)
        {
            // Pass a callback so UI buttons can notify which option was chosen
            dialoguePanelUI.ShowOptions(currentNode.dialogueOption, ChooseOption);
            return;
        }

        DialogueNode nextNode = FindNodeById(currentNode.nextId);

        if(nextNode != null)
        {
            currentNode = nextNode;
            dialoguePanelUI.ShowDialogue(currentNpcName, currentNode.text);
        }
        else
        {
            Debug.LogWarning(currentNode.nextId + "invalid id");
            DialogueService.EndedDialogue();
        }
        
    }

    private void OnDialogueEnded()
    {
        GameplayStateService.Unlock(GameplayLockReason.Dialogue);

        dialoguePanelUI.HideDialogue();
        isDialogueActive = false;
        validateId.Clear();
    }

    private void OnDialogueStarted(string npcName, DialogueNode[] dialogueLines)
    {
        if(isDialogueActive)
            return;

        if(dialogueLines == null)
            return;
        
        if(npcName == null)
            return;

         if(dialogueLines.Length == 0)
            return;
        
        validateId.Clear();

        currentNodes = dialogueLines;

        for (int i=0; i < currentNodes.Length; i++)
        {
            int id = currentNodes[i].id;
            
            if(validateId.ContainsKey(id))
            {
                Debug.LogWarning($"Duplicate node id {id} in dialogue for npc {currentNpcName}");
            }
            else
            {
                validateId[id] = currentNodes[i];
            }
        }

        currentNpcName = npcName;
        currentNode = currentNodes[0];
        GameplayStateService.Lock(GameplayLockReason.Dialogue);

        dialoguePanelUI.ShowDialogue(currentNpcName, currentNode.text);
        isDialogueActive = true;
    }

    private DialogueNode FindNodeById(int id)
    {
        if(validateId.TryGetValue(id, out DialogueNode node))
            return node;

        return null;
    }

    public void ChooseOption(int optionIndex)
    {
        if(currentNode == null)
            return;  

        if(currentNode.dialogueOption == null)
            return;
        
        if(currentNode.dialogueOption.Length == 0)
            return;      
        
        if(optionIndex >= currentNode.dialogueOption.Length)
            return;

        DialogueOption choose = currentNode.dialogueOption[optionIndex];
        DialogueNode nextNode = FindNodeById(choose.nextId);

        if(nextNode != null)
        {
            currentNode = nextNode;
            dialoguePanelUI.ShowDialogue(currentNpcName, currentNode.text);
        }
        else
        {
            Debug.LogWarning($"Invalid next id: {currentNode.dialogueOption[optionIndex].nextId} in npc {currentNpcName}" );
            DialogueService.EndedDialogue();
        }

    }
}
