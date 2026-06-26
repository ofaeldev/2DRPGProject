using System;

[Serializable]
public class DialogueNode
{
    public string id;
    public string nextId;
    public string text;
    public string flagConversation;
    public DialogueOption[] dialogueOption;
}