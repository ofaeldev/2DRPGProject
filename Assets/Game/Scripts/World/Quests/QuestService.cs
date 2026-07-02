using System;
using System.Collections.Generic;

public static class QuestService
{
    public static event Action<string> QuestStarted;
    public static event Action<string> QuestCompleted;
    public static event Action QuestsLoaded; // Evento quando quests são carregadas

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

    public static Dictionary<string, QuestStatus> GetAllQuestStates()
    {
        return new Dictionary<string, QuestStatus>(quests);
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

    public static void SetQuest(string questId, QuestStatus status)
    {
        if (!IsValidQuestId(questId))
            return;

        quests[questId] = status;
    }

    /// <summary>
    /// Notifica que as quests foram completamente carregadas.
    /// </summary>
    public static void NotifyQuestsLoaded()
    {
        QuestsLoaded?.Invoke();
    }
}
