using System;

public static class DialogueService
{
    public static event Action<string, DialogueSession> DialogueStartedSession;
    public static event Action DialogueAdvanced;
    public static event Action DialogueEnded;

    // New overload to start dialogue using the DialogueSession model
    public static void StartDialogue(string npcName, DialogueSession session)
    {
        if (session == null)
            return;

        // Notify listeners that want the full session
        DialogueStartedSession?.Invoke(npcName, session);
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