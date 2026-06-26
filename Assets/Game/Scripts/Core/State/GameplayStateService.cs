using System;
using System.Collections.Generic;

public enum GameplayLockReason
{
    Dialogue,
    Inventory,
    Cutscene,
    Menu,
    Loading,
    Feedback
}

public static class GameplayStateService
{
    public static event Action StateChanged;

    private static readonly HashSet<GameplayLockReason> activeLockReasons = new HashSet<GameplayLockReason>();

    public static bool IsLocked => activeLockReasons.Count > 0;
    public static bool CanAdvanceDialogue => activeLockReasons.Contains(GameplayLockReason.Dialogue);
    public static bool CanInteract => !activeLockReasons.Contains(GameplayLockReason.Dialogue) &&
                                        !activeLockReasons.Contains(GameplayLockReason.Cutscene) &&
                                        !activeLockReasons.Contains(GameplayLockReason.Loading) &&
                                        !activeLockReasons.Contains(GameplayLockReason.Menu) &&
                                        !activeLockReasons.Contains(GameplayLockReason.Feedback);
    public static bool CanMove => !activeLockReasons.Contains(GameplayLockReason.Dialogue) &&
            !activeLockReasons.Contains(GameplayLockReason.Cutscene) &&
            !activeLockReasons.Contains(GameplayLockReason.Feedback) &&
            !activeLockReasons.Contains(GameplayLockReason.Loading) &&
            !activeLockReasons.Contains(GameplayLockReason.Inventory) &&
            !activeLockReasons.Contains(GameplayLockReason.Menu);

    public static void Lock(GameplayLockReason reason)
    {
        bool changed = activeLockReasons.Add(reason);

        if(changed)
            NotifyIfStateChanged();
    }

    public static void Unlock(GameplayLockReason reason)
    {
        bool changed = activeLockReasons.Remove(reason);

        if(changed)
            NotifyIfStateChanged();
    }

    private static void NotifyIfStateChanged()
    {
        StateChanged?.Invoke();
    }
}
