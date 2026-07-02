using UnityEngine;

public class GameSaveController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private void Awake()
    {
        if (playerTransform == null)
        {
            playerTransform = FindObjectOfType<PlayerController>()?.transform;
        }

        LoadGame();
    }

    private void OnEnable()
    {
        InventoryService.ItemAdded += HandleStateChanged;
        InventoryService.ItemRemoved += HandleStateChanged;
        QuestService.QuestStarted += HandleStateChanged;
        QuestService.QuestCompleted += HandleStateChanged;

        WorldStateService.FlagSet += HandleWorldStateChanged;
        WorldStateService.FlagCleared += HandleWorldStateChanged;
        WorldStateService.StageSet += HandleWorldStateChanged;
        WorldStateService.StageRemoved += HandleWorldStateChanged;
        WorldStateService.ConversationMarkedComplete += HandleWorldStateChanged;
    }

    private void OnDisable()
    {
        InventoryService.ItemAdded -= HandleStateChanged;
        InventoryService.ItemRemoved -= HandleStateChanged;
        QuestService.QuestStarted -= HandleStateChanged;
        QuestService.QuestCompleted -= HandleStateChanged;

        WorldStateService.FlagSet -= HandleWorldStateChanged;
        WorldStateService.FlagCleared -= HandleWorldStateChanged;
        WorldStateService.StageSet -= HandleWorldStateChanged;
        WorldStateService.StageRemoved -= HandleWorldStateChanged;
        WorldStateService.ConversationMarkedComplete -= HandleWorldStateChanged;
    }

    private void HandleWorldStateChanged(string flagId)
    {
        SaveGame();
    }

    private void HandleWorldStateChanged(string stageId, int value)
    {
        SaveGame();
    }

    private void HandleStateChanged(string id, int amount)
    {
        SaveGame();
    }

    private void HandleStateChanged(string questId)
    {
        SaveGame();
    }

    private void LoadGame()
    {
        GameSaveData save = SaveManager.Load();

        InventoryService.ClearAll();
        foreach (var item in save.inventory.items)
        {
            if (!string.IsNullOrEmpty(item.itemId) && item.amount > 0)
            {
                InventoryService.TryLoadItem(item.itemId, item.amount);
            }
        }

        QuestService.ClearAll();
        foreach (var quest in save.quests.quests)
        {
            if (string.IsNullOrEmpty(quest.questId))
                continue;

            if (quest.status == (int)QuestStatus.Active)
            {
                QuestService.SetQuest(quest.questId, QuestStatus.Active);
            }
            else if (quest.status == (int)QuestStatus.Completed)
            {
                QuestService.SetQuest(quest.questId, QuestStatus.Completed);
            }
        }

        if (playerTransform != null)
        {
            playerTransform.position = save.player.position;
        }

        WorldStateService.LoadAllData(save.worldState);

        // Notifica que os dados foram carregados (para UI se atualizar)
        InventoryService.NotifyInventoryLoaded();
        QuestService.NotifyQuestsLoaded();
    }

    private void SaveGame()
    {
        var data = new GameSaveData();

        foreach (var item in InventoryService.GetAllItems())
        {
            data.inventory.items.Add(new ItemSaveData
            {
                itemId = item.Key,
                amount = item.Value
            });
        }

        foreach (var quest in QuestService.GetAllQuestStates())
        {
            data.quests.quests.Add(new QuestStateSaveData
            {
                questId = quest.Key,
                status = (int)quest.Value
            });
        }

        if (playerTransform != null)
        {
            data.player.position = playerTransform.position;
        }

        data.worldState = WorldStateService.GetAllData();
        SaveManager.Save(data);
    }
}
