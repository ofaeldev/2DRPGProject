using UnityEngine;

public class PlayerInteractionResolver
{
    private readonly Transform owner;
    private readonly LayerMask interactableLayer;
    private readonly float interactionDistance;
    private readonly float interactionRadius;
    private readonly PlayerInputReader playerInputReader;

    public PlayerInteractionResolver(Transform owner, LayerMask interactableLayer, float interactionDistance, float interactionRadius, PlayerInputReader playerInputReader)
    {
        this.owner = owner;
        this.interactableLayer = interactableLayer;
        this.interactionDistance = interactionDistance;
        this.interactionRadius = interactionRadius;
        this.playerInputReader = playerInputReader;
    }

    public IInteraction DetectTarget()
    {
        Vector2 interactionPoint = GetPoint();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(interactionPoint, interactionRadius, interactableLayer);

        IInteraction closestInteraction = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D candidateCollider = colliders[i];

            if (!candidateCollider.TryGetComponent(out IInteraction candidateInteraction))
                continue;

            Vector2 candidateInteractionCollider = candidateCollider.ClosestPoint(interactionPoint);
            float candidateDistance = Vector2.Distance(interactionPoint, candidateInteractionCollider);

            if (candidateDistance < closestDistance)
            {
                closestDistance = candidateDistance;
                closestInteraction = candidateInteraction;
            }
        }

        return closestInteraction;
    }

    public Vector2 GetPoint()
    {
        Vector2 lookDirection;

        if (playerInputReader == null || playerInputReader.LastMoveDirection == Vector2.zero)
        {
            lookDirection = Vector2.down;
        }
        else
        {
            lookDirection = playerInputReader.LastMoveDirection;
        }

        Vector2 playerPosition = owner.position;
        Vector2 normalizedLookDirection = lookDirection.normalized;
        return playerPosition + normalizedLookDirection * interactionDistance;
    }
}
