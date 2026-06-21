using System;

public static class PopUpService
{
    public static event Action<IInteraction> PopUpStartedEvent;
    public static event Action PopUpEnded;

    public static void StartedPopUp(IInteraction PopUpMessage)
    {
        PopUpStartedEvent?.Invoke(PopUpMessage);
    }

    public static void EndedPopUp()
    {
        PopUpEnded?.Invoke();
    }
}