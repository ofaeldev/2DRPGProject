using System;

public enum QuestStatus
{
    NotStarted,
    Active,
    Completed
}

[Serializable]
public class QuestDefinition
{
    public string questId;
    public string title;
    public string description;
}