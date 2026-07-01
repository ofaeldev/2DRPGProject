using System.Collections;
using TMPro;
using UnityEngine;

public class QuestPanelUI : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text statusText;
    public CanvasGroup canvasGroup;
    private Coroutine hideRoutine;

    void Start()
    {
        HideQuestPanel();
    }
    public void ShowQuestStarted(string title, string status)
    {
        titleText.text = $"Start quest {title}";
        statusText.text = $"{status}";

        ShowTemporary();
    }
    public void ShowQuestCompleted(string title, string status)
    {
        titleText.text = $"Quest {title} is {status}";
        statusText.text = $"{status}";

        ShowTemporary();
    }

    private void ShowTemporary()
    {
        canvasGroup.alpha = 1;

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideAfterDelay());
    }

    private IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        HideQuestPanel();
        hideRoutine = null;
    }
    public void HideQuestPanel()
    {
        titleText.text = "";
        statusText.text = "";
        canvasGroup.alpha = 0;
    }    
    
}