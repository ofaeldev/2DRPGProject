using System.Collections.Generic;
using UnityEngine;

public class NpcInteractable : MonoBehaviour, IInteraction
{
    [Header("Info")]
    [SerializeField] private string npcName;
    [SerializeField] private string popUpFeedbackMsg;
    [SerializeField] private DialogueSession session;
    private GameObject interactionPointTransform;
    public void Execute()
    {
        // Use NPC name and the DialogueSession bundle
        DialogueService.StartDialogue(npcName, session);
    }

    public string GetPopUpText()
    {
        return popUpFeedbackMsg;
    }

    public Transform InteractionPoint()
    {
        return this.interactionPointTransform != null ? this.interactionPointTransform.transform : this.transform;
    }

    #if UNITY_EDITOR
    private void OnValidate()
    {
        var nodes = session != null ? session.firstTimeNodes : null;
        if (nodes == null || nodes.Length == 0)
            return;

        string label = string.IsNullOrEmpty(npcName) ? gameObject.name : npcName;

        // 1) coletar ids e detectar duplicatas
        var ids = new HashSet<string>();
        for (int i = 0; i < nodes.Length; i++)
        {
            var node = nodes[i];
            if (node == null)
                continue;

            if (string.IsNullOrWhiteSpace(node.id))
                Debug.LogWarning($"Empty node id in NPC '{label}' (index {i})", this);

            if (!ids.Add(node.id))
                Debug.LogWarning($"Duplicate node id {node.id} in NPC '{label}' (index {i})", this);
        }

        // 2) verificar nextId e options.nextId apontarem para ids existentes ou empty/null
        for (int i = 0; i < nodes.Length; i++)
        {
            var node = nodes[i];
            if (node == null)
                continue;

            if (!string.IsNullOrWhiteSpace(node.nextId) && !ids.Contains(node.nextId))
                Debug.LogWarning($"Invalid nextId {node.nextId} in node {node.id} of NPC '{label}' (index {i})", this);

            if (node.dialogueOption != null)
            {
                for (int j = 0; j < node.dialogueOption.Length; j++)
                {
                    var opt = node.dialogueOption[j];
                    if (opt == null)
                        continue;

                    if (!string.IsNullOrWhiteSpace(opt.nextId) && !ids.Contains(opt.nextId))
                        Debug.LogWarning($"Invalid option.nextId {opt.nextId} in node {node.id} option {j} of NPC '{label}'", this);
                }
            }
        }
    }
    #endif
}