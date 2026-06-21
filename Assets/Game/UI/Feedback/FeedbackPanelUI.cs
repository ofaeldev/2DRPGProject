using TMPro;
using UnityEngine;

public class FeedbackPanelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text feedbackMessage;        
    [SerializeField] private CanvasGroup canvasGroup;

    public void ShowFeedback(string message)
    {
        feedbackMessage.text = message;
        canvasGroup.alpha = 1;
    }

    public void HideFeedback()
    {
        feedbackMessage.text = string.Empty;
        canvasGroup.alpha = 0;
    }
    
}