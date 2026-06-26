using System;

[Serializable]
public class WorldRequirement
{
    public string requiredFlag;
    public string requiredCompletedQuest;
    public string requiredActiveQuest;

    public bool IsMet()
    {
        if (!string.IsNullOrEmpty(requiredFlag) && !WorldStateService.HasFlag(requiredFlag))
            return false;

        if (!string.IsNullOrEmpty(requiredCompletedQuest) && QuestService.GetQuestStatus(requiredCompletedQuest) != QuestStatus.Completed)
            return false;        

        if (!string.IsNullOrEmpty(requiredActiveQuest) && QuestService.GetQuestStatus(requiredActiveQuest) != QuestStatus.Active)
            return false;    
        
        return true;
    }
}