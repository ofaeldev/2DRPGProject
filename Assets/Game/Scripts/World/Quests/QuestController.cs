using UnityEngine;

public class QuestController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private string debugQuestId = "help_south_gate";
    [SerializeField] private bool logInitialStatus = true;
    [SerializeField] private QuestDefinition[] questDefinition;
    private QuestPanelUI questPanelUI;    
    private QuestDatabase questDatabase;

    private void Awake()
    {
        questPanelUI = GetComponent<QuestPanelUI>();

        questDatabase = new QuestDatabase(questDefinition);
    }
    private void OnEnable()
    {
        QuestService.QuestStarted += OnQuestStarted;
        QuestService.QuestCompleted += OnQuestCompleted;
    }

    private void Start()
    {
        if (logInitialStatus)
            LogQuestStatus(debugQuestId);
    }

    private void OnDisable()
    {
        QuestService.QuestStarted -= OnQuestStarted;
        QuestService.QuestCompleted -= OnQuestCompleted;
    }

    private void OnQuestStarted(string questId)
    {
        var currentTitle = questDatabase.GetQuestTitle(questId);
        var currentStatus = QuestService.GetQuestStatus(questId);

        Debug.Log($"Quest started: {currentTitle} | Status: {currentStatus}");
        questPanelUI.ShowQuestStarted(currentTitle, currentStatus.ToString());
    }

    private void OnQuestCompleted(string questId)
    {
        var currentTitle = questDatabase.GetQuestTitle(questId);
        var currentStatus = QuestService.GetQuestStatus(questId);

        Debug.Log($"Quest completed: {currentTitle} | Status: {currentStatus}");
        questPanelUI.ShowQuestCompleted(currentTitle, currentStatus.ToString());
    }

    [ContextMenu("Debug/Start Quest")]
    private void DebugStartQuest()
    {
        QuestService.StartQuest(debugQuestId);
        LogQuestStatus(debugQuestId);
    }

    [ContextMenu("Debug/Complete Quest")]
    private void DebugCompleteQuest()
    {
        QuestService.CompleteQuest(debugQuestId);
        LogQuestStatus(debugQuestId);
    }

    [ContextMenu("Debug/Log Quest Status")]
    private void DebugLogQuestStatus()
    {
        LogQuestStatus(debugQuestId);
    }

    [ContextMenu("Debug/Clear All Quests")]
    private void DebugClearAllQuests()
    {
        QuestService.ClearAll();
        Debug.Log("All quests cleared.");
        LogQuestStatus(debugQuestId);
    }

    private void LogQuestStatus(string questId)
    {
        Debug.Log($"Quest status: {questId} -> {QuestService.GetQuestStatus(questId)}");
    }
}
