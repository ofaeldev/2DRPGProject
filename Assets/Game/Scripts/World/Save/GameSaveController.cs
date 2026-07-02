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
    }

    private void OnDisable()
    {
        InventoryService.ItemAdded -= HandleStateChanged;
        InventoryService.ItemRemoved -= HandleStateChanged;
        QuestService.QuestStarted -= HandleStateChanged;
        QuestService.QuestCompleted -= HandleStateChanged;
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
                InventoryService.TryAddItem(item.itemId, item.amount);
            }
        }

        QuestService.ClearAll();
        foreach (var quest in save.quests.quests)
        {
            if (string.IsNullOrEmpty(quest.questId))
                continue;

            if (quest.status == (int)QuestStatus.Active)
            {
                QuestService.StartQuest(quest.questId);
            }
            else if (quest.status == (int)QuestStatus.Completed)
            {
                QuestService.StartQuest(quest.questId);
                QuestService.CompleteQuest(quest.questId);
            }
        }

        if (playerTransform != null)
        {
            playerTransform.position = save.player.position;
        }
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

        SaveManager.Save(data);
    }
}
