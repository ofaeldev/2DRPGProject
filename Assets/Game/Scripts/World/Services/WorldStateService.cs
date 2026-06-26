using System.Collections.Generic;
using UnityEngine;

public static class WorldStateService
{
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
}