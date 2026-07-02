using System.Collections.Generic;
using UnityEngine;
using System;

public static class WorldStateService
{
    public static event Action<string> FlagSet;
    public static event Action<string> FlagCleared;
    public static event Action<string, int> StageSet;
    public static event Action<string> StageRemoved;
    public static event Action<string> ConversationMarkedComplete;
    public static event Action WorldStateLoaded;

    private static Dictionary<string, bool> worldFlags = new Dictionary<string, bool>(); 
    private static Dictionary<string, int> stages = new Dictionary<string, int>();
    private static HashSet<string> conversationComplete = new HashSet<string>();

    #region Flags
    public static bool HasFlag(string flagId)
    {        
        if(string.IsNullOrEmpty(flagId))
            return false;

        if(worldFlags.ContainsKey(flagId))
            return true;

        return false;
    }

    public static void SetFlag(string flagId)
    {
        if(string.IsNullOrEmpty(flagId))
            return;

        if(worldFlags == null)
            return;

        worldFlags[flagId] = true;

        FlagSet?.Invoke(flagId);       

    }

    public static void ClearFlag(string flagId)
    {
        if(string.IsNullOrEmpty(flagId))
            return;

        if(worldFlags == null)
            return;

        if(!worldFlags.ContainsKey(flagId))
            return;

        worldFlags.Remove(flagId);
        FlagCleared?.Invoke(flagId);
        
    }
    #endregion

    #region Stage

    public static int GetStage(string stageId)
    {
        if(string.IsNullOrEmpty(stageId))
            return -1;

        if(stages == null)
            return -1;

        if(stages.TryGetValue(stageId, out int stageValue))
            return stageValue;

        return -1;
    }

    public static void SetStage(string stageId, int stageValue)
    {
        if(stages == null)
            return;
                
        if(string.IsNullOrEmpty(stageId))
            return;
        
        if(stageValue < 0)
            return;
        
        stages[stageId] = stageValue;
        StageSet?.Invoke(stageId, stageValue);
        
    }

    public static bool HasStage(string stageId)
    {
        if(stages == null)
            return false;

        if(string.IsNullOrEmpty(stageId))
            return false;

        if(stages.ContainsKey(stageId))
            return true;
        
        return false;
    }

    public static void RemoveStage(string stageId)
    {
        if(stages == null)
            return;

        if(string.IsNullOrEmpty(stageId))
            return;

        if (!stages.ContainsKey(stageId))
            return;

        stages.Remove(stageId);           
        StageRemoved?.Invoke(stageId);
        
    }

    #endregion

    #region Conversation

    public static bool HasConversationCompleted(string conversationId)
    {
        
        if(string.IsNullOrEmpty(conversationId))
            return false;
        
        if(conversationComplete.Contains(conversationId))
            return true;

        return false;
        
    }

    public static void MarkConversationComplete(string conversationId)
    {
        
        if(string.IsNullOrEmpty(conversationId))
            return;
        
        if(conversationComplete.Contains(conversationId))
            return;

        conversationComplete.Add(conversationId);
        ConversationMarkedComplete?.Invoke(conversationId);
        
    }

    #endregion

    #region Helpers
    public static void ClearAllFlags()
    {
        if(worldFlags.Count > 0)
            worldFlags.Clear();
        
        Debug.LogWarning($"Clear {worldFlags.Count} flag(s)");
    }

    public static void ClearAllStages()
    {
        if(stages.Count > 0)
            stages.Clear();
        
        Debug.LogWarning($"Clear {stages.Count} stage(s)");
    }

    public static void ClearAllConversationHistory()
    {
        if(conversationComplete.Count > 0)
            conversationComplete.Clear();

        Debug.LogWarning($"Clear {conversationComplete.Count} conversation(s)");
    }

    public static void ClearAll()
    {
            worldFlags.Clear();
            stages.Clear();
            conversationComplete.Clear();

            Debug.LogWarning($"Clear all historic flags {worldFlags.Count}, stages {stages.Count}, conversation {conversationComplete.Count}");        
    }
    #endregion

    #region Save/Load Helpers
    public static WorldStateSaveData GetAllData()
    {
        WorldStateSaveData data = new WorldStateSaveData();

        // Salva flags
        foreach (var flagId in worldFlags.Keys)
        {
            data.flags.Add(new FlagSaveData { flagId = flagId });
        }

        // Salva stages
        foreach (var stage in stages)
        {
            data.stages.Add(new StageSaveData { stageId = stage.Key, value = stage.Value });
        }

        // Salva conversas completadas
        foreach (var conversationId in conversationComplete)
        {
            data.conversationsCompleted.Add(conversationId);
        }

        return data;
    }

    public static void LoadAllData(WorldStateSaveData data)
    {
        if (data == null)
            return;

        // Limpa dados existentes
        ClearAll();

        // Carrega flags
        foreach (var flagData in data.flags)
        {
            if (!string.IsNullOrEmpty(flagData.flagId))
            {
                worldFlags[flagData.flagId] = true;
            }
        }

        // Carrega stages
        foreach (var stageData in data.stages)
        {
            if (!string.IsNullOrEmpty(stageData.stageId))
            {
                stages[stageData.stageId] = stageData.value;
            }
        }

        // Carrega conversas completadas
        foreach (var conversationId in data.conversationsCompleted)
        {
            if (!string.IsNullOrEmpty(conversationId))
            {
                conversationComplete.Add(conversationId);
            }
        }
        
        WorldStateLoaded?.Invoke();
    }
    #endregion
    
}