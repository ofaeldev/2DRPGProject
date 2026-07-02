using System;
using UnityEngine;

public class PlayerInputReader
{
    public Vector2 MoveDirection { get; private set; }
    public Vector2 LastMoveDirection { get; private set; }
    public event Action InteractionRequested;
    public event Action InventoryRequested;

    public void SetMove(Vector2 direction)
    {
        MoveDirection = direction.normalized;
        
        if(direction.sqrMagnitude != 0)
            LastMoveDirection = direction.normalized;
    }

    public void RequestInteraction()
    {
        InteractionRequested?.Invoke();
    }

    public void RequestInventory()
    {
        InventoryRequested?.Invoke();
    }
}