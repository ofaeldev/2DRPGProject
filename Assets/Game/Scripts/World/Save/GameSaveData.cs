using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameSaveData
{
    public InventorySaveData inventory = new InventorySaveData();
    public QuestSaveData quests = new QuestSaveData();
    public PlayerSaveData player = new PlayerSaveData();
}

[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> items = new List<ItemSaveData>();
}

[Serializable]
public class ItemSaveData
{
    public string itemId;
    public int amount;
}

[Serializable]
public class QuestSaveData
{
    public List<QuestStateSaveData> quests = new List<QuestStateSaveData>();
}

[Serializable]
public class QuestStateSaveData
{
    public string questId;
    public int status;
}

[Serializable]
public class PlayerSaveData
{
    public Vector3 position;
}
