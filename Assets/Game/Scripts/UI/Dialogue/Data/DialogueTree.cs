using System;

public class DialogueTree
{
    private int conversationId;
    private bool startCondition;
    private bool repeatCondition;
    private Action fallbackDialogue;
}