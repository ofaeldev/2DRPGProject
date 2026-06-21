using System;

public static class DialogueService
{
    public static event Action<string,DialogueNode[]> DialogueStarted;
    public static event Action DialogueAdvanced;
    public static event Action DialogueEnded;

    public static void StartDialogue(string npcName, DialogueNode[] nodes)
    {
        DialogueStarted?.Invoke(npcName, nodes);
    }

    public static void AdvanceDialogue()
    {
        DialogueAdvanced?.Invoke();
    }

    public static void EndedDialogue()
    {
        DialogueEnded?.Invoke();
    }
    
}