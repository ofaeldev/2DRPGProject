using System.Collections.Generic;
using UnityEngine;

public class DialogueFlowController
{
    private readonly Dictionary<string, DialogueNode> nodeMap = new Dictionary<string, DialogueNode>();

    public DialogueNode[] BuildNodeSet(DialogueSession session)
    {
        if (session == null)
            return null;

        DialogueNode[] nodesToUse = session.firstTimeNodes;

        if (session.worldStateBranches == null)
            return nodesToUse;

        for (int i = 0; i < session.worldStateBranches.Length; i++)
        {
            DialogueBranch branch = session.worldStateBranches[i];

            if (branch == null)
                continue;

            if (string.IsNullOrEmpty(branch.requiredFlag))
                continue;

            if (!HasNodes(branch.nodes))
                continue;

            if (!WorldStateService.HasFlag(branch.requiredFlag))
                continue;

            nodesToUse = branch.nodes;
            break;
        }

        return nodesToUse;
    }

    public void BuildNodeMap(DialogueNode[] nodes, string npcName)
    {
        nodeMap.Clear();

        if (nodes == null || nodes.Length == 0)
            return;

        for (int i = 0; i < nodes.Length; i++)
        {
            DialogueNode node = nodes[i];
            if (node == null)
                continue;

            string id = node.id;
            if (string.IsNullOrWhiteSpace(id))
            {
                Debug.LogWarning($"Empty node id at index {i} for npc {npcName}");
                continue;
            }

            if (nodeMap.ContainsKey(id))
            {
                Debug.LogWarning($"Duplicate node id {id} in dialogue for npc {npcName}");
            }
            else
            {
                nodeMap[id] = node;
            }
        }
    }

    public DialogueNode FindNodeById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        if (nodeMap.TryGetValue(id, out DialogueNode node))
            return node;

        return null;
    }

    public void Clear()
    {
        nodeMap.Clear();
    }

    private bool HasNodes(DialogueNode[] nodes)
    {
        return nodes != null && nodes.Length > 0;
    }
}
