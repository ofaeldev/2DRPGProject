using System;

[Serializable]
public class QuestDatabase
{    
    private QuestDefinition[] questDefinitions;
    public QuestDatabase(QuestDefinition[] definition)
    {
        questDefinitions = definition;
    }

    public string GetQuestTitle(string questId)
    {
        if(string.IsNullOrEmpty(questId))
            return null;        
        
        if(questDefinitions == null)
            return null;

        for(int i = 0; i < questDefinitions.Length; i++)
        {
            if(questDefinitions[i] == null)
                continue; 

            if(questDefinitions[i].questId == questId)
                return questDefinitions[i].title;
        }

        return questId;
    }

    public QuestDefinition GetQuestDefinition(string questId)
    {
        if(string.IsNullOrEmpty(questId))
            return null;        
        
        if(questDefinitions == null)
            return null;

        for(int i = 0; i < questDefinitions.Length; i++)
        {
            if(questDefinitions[i] == null)
                continue;            

            if(questDefinitions[i].questId == questId)
            {
                return questDefinitions[i];                
            }              
        }

        return null;
    }
}