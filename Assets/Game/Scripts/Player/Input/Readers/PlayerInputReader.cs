using System;
using UnityEngine;

public class PlayerInputReader
{
    public Vector2 MoveDirection { get; private set; }
    public Vector2 LastMoveDirection { get; private set; }
    public event Action InteractionRequested;

    public void SetMove(Vector2 direction)
    {
        MoveDirection = direction;
        
        if(direction.sqrMagnitude != 0)
            LastMoveDirection = direction;
    }

    public void RequestInteraction()
    {
        InteractionRequested?.Invoke();
    }
}