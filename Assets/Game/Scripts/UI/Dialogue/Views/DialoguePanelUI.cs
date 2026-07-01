using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanelUI : MonoBehaviour
{
    public TMP_Text dialogueLine;
    public TMP_Text npcName;
    public CanvasGroup canvasGroup;

    // Parent transform where option buttons will be instantiated
    public Transform optionsContainer;

    // Prefab for an option button. Prefab should contain a Button component
    // and a child TMP_Text to show the option text.
    public Button optionButtonPrefab;

    public void ShowDialogue(string npcName, string lines)
    {
        this.npcName.text = npcName;
        this.dialogueLine.text = lines;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

        HideOptions();
    }

    public void HideDialogue()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        dialogueLine.text = string.Empty;
        HideOptions();
    }

    // Show a dynamic list of options. The caller provides a callback to receive
    // the chosen option index. This scales to N options and keeps UI generic.
    public void ShowOptions(DialogueOption[] options, Action<int> onOptionSelected)
    {
        HideOptions();

        if (options == null || options.Length == 0 || optionButtonPrefab == null || optionsContainer == null)
        {
            Debug.LogError("Invalid or null = " + options + " or " + options.Length + " or " + optionButtonPrefab + " or " + optionsContainer);
            return;
        }

        for (int i = 0; i < options.Length; i++)
        {
            int index = i;
            Button btn = Instantiate(optionButtonPrefab, optionsContainer);
            TMP_Text txt = btn.GetComponentInChildren<TMP_Text>(true);
            if (txt != null)
                txt.text = options[i].optionText;

            btn.gameObject.SetActive(true);
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => onOptionSelected?.Invoke(index));
        }
    }

    public void HideOptions()
    {
        if (optionsContainer == null)
            return;

        for (int i = optionsContainer.childCount - 1; i >= 0; i--)
        {
            var child = optionsContainer.GetChild(i).gameObject;
            Destroy(child);
        }
    }
}