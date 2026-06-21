using System;

public static class FeedbackService
{  
    public static event Action<string> FeedbackStarted;
    public static event Action FeedbackEnded;
    
    public static void EndedFeedback()
    {
        FeedbackEnded?.Invoke();
    }
    public static void StartFeedback(string message)
    {
        FeedbackStarted?.Invoke(message);
    }
}