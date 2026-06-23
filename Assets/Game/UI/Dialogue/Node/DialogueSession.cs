using System;

[Serializable]
public class DialogueSession
{
    // Unique identifier for this conversation (e.g. "npc_alfred_intro")
    public string conversationId;

    // Optional explicit start node id; when empty, controller will use the first node in `nodes`.
    public string startNodeId;

    // Main dialogue nodes for the first time path
    public DialogueNode[] nodes;

    // Optional alternative nodes for repeat interactions
    public DialogueNode[] repeatNodes;

    // If true and repeatNodes is provided, DialogueController may show repeatNodes when conversation already completed
    public bool useRepeatWhenCompleted = true;
}
