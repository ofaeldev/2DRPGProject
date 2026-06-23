using System;

[Serializable]
public class DialogueNode
{
    public string id;
    public string nextId;
    public string text;
    public DialogueOption[] dialogueOption;
}