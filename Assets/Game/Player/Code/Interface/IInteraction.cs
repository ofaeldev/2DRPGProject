using UnityEngine;

public interface IInteraction
{
    void Execute();
    string GetPopUpText();
    Transform InteractionPoint();
}
