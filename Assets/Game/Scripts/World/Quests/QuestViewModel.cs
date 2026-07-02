using UnityEngine;

public class QuestViewModel
{
    private readonly QuestDatabase questDatabase;

    public QuestViewModel(QuestDatabase questDatabase)
    {
        this.questDatabase = questDatabase;
    }

    public QuestDisplayModel BuildDisplayModel(string questId)
    {
        string title = questDatabase != null ? questDatabase.GetQuestTitle(questId) : questId;
        QuestStatus status = QuestService.GetQuestStatus(questId);
        return new QuestDisplayModel(title, status.ToString());
    }
}

public class QuestDisplayModel
{
    public QuestDisplayModel(string title, string status)
    {
        Title = title;
        Status = status;
    }

    public string Title { get; }
    public string Status { get; }
}
