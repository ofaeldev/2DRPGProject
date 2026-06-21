using System;

[Serializable]
public class DialogueNode
{
    public int id;
    public int nextId;
    public string text;
    public DialogueOption[] dialogueOption;
}