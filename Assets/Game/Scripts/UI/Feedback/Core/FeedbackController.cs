using System.Collections;
using UnityEngine;

public class FeedbackController : MonoBehaviour
{
    [SerializeField] private float visibleDuration = 2f;
    private FeedbackPanelUI feedbackPanelUI;
    private Coroutine hideRoutine;

    private void Awake()
    {
        feedbackPanelUI = GetComponent<FeedbackPanelUI>();
        HideImmediate();
    }

    private void OnEnable()
    {
        FeedbackService.FeedbackStarted += OnFeedbackStarted;
        FeedbackService.FeedbackEnded += OnFeedbackEnded;
    }

    private void OnDisable()
    {
        FeedbackService.FeedbackStarted -= OnFeedbackStarted;
        FeedbackService.FeedbackEnded -= OnFeedbackEnded;

        if(hideRoutine != null)
        {
            StopCoroutine(hideRoutine);
            HideImmediate();
            GameplayStateService.Unlock(GameplayLockReason.Feedback);
        }
    }
    private void OnFeedbackEnded()
    {
        if(hideRoutine != null)
            StopCoroutine(hideRoutine);  

        HideImmediate(); 
        GameplayStateService.Unlock(GameplayLockReason.Feedback);
        
    }

    private void OnFeedbackStarted(string message)
    {
        if(hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideRoutine(message));
        GameplayStateService.Lock(GameplayLockReason.Feedback);
    }

    private IEnumerator HideRoutine(string message)
    {
        feedbackPanelUI.ShowFeedback(message);

        yield return new WaitForSeconds(visibleDuration);

        FeedbackService.EndedFeedback();
    }

    private void HideImmediate()
    {        
        feedbackPanelUI.HideFeedback();
        hideRoutine = null;        
    }
}
