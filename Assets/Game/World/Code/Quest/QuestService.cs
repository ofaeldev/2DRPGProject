using System;
using System.Collections.Generic;

public static class QuestService
{
    public static event Action<string> QuestStarted;
    public static event Action<string> QuestCompleted;

    private static Dictionary<string, QuestStatus> quests = new Dictionary<string, QuestStatus>();
    public static void StartQuest(string questId)
    {
        if (!IsValidQuestId(questId))
            return;

        QuestStatus currentStatus = GetQuestStatus(questId);

        if(currentStatus == QuestStatus.Completed)
            return;

        if(currentStatus == QuestStatus.Active)
            return;
        
        quests[questId] = QuestStatus.Active;

        QuestStarted?.Invoke(questId);
    }

    public static void CompleteQuest(string questId)
    {
        if (!IsValidQuestId(questId))
            return;

        QuestStatus currentStatus = GetQuestStatus(questId);

        if (currentStatus != QuestStatus.Active)
            return;

        quests[questId] = QuestStatus.Completed;
        QuestCompleted?.Invoke(questId);
    }

    public static void ClearAll()
    {
        quests.Clear();
    }

    public static bool IsQuestActive(string questId)
    {
        return GetQuestStatus(questId) == QuestStatus.Active;
    }

    public static bool IsQuestComplete(string questId)
    {
        return GetQuestStatus(questId) == QuestStatus.Completed;
    }
    private static bool IsValidQuestId(string questId)
    {
        return !string.IsNullOrWhiteSpace(questId);
    }

    public static QuestStatus GetQuestStatus(string questId)
    {
        if (!IsValidQuestId(questId))
            return QuestStatus.NotStarted;

        if (quests.TryGetValue(questId, out QuestStatus status))
            return status;

        return QuestStatus.NotStarted;
    }
}
