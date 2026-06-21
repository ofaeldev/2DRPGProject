using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInputReader playerInputReader;
    [SerializeField] private float interactionDistance;
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactableLayer;
    private IInteraction currentInteraction;
    private bool isConnected;

    private void OnEnable()
    {
        Connect();
        GameplayStateService.StateChanged += OnStateChange;
    }

    private void OnDisable()
    {
        Disconnect();
        GameplayStateService.StateChanged -= OnStateChange;
    }

    private void OnStateChange()
    {
        if (!GameplayStateService.CanInteract)
        {
            currentInteraction = null;
            PopUpService.EndedPopUp();            
        }
    }

    public void Initialize(PlayerInputReader reader)
    {
        playerInputReader = reader;

        if(isActiveAndEnabled)
        {
            Connect();
        }
    }

    private void Update()
    {
        if(GameplayStateService.CanInteract)
            UpdateCurrentInteraction();
    }


    private void Connect()
    {
        if(isConnected)
        return;

        if(playerInputReader == null)
        return;
        
        playerInputReader.InteractionRequested += OnInteraction;
        
        isConnected = true;
    }

    private void Disconnect()
    {
        if(!isConnected)
        return;

        playerInputReader.InteractionRequested -= OnInteraction;

        isConnected = false;
    }
    private void UpdateCurrentInteraction()
    {
        IInteraction detectedInteraction = DetectTarget();
        
        if(currentInteraction == detectedInteraction)
            return;

        currentInteraction = detectedInteraction;        
        
        if(currentInteraction != null)
        {
            PopUpService.StartedPopUp(currentInteraction);
        }
        else
        {
            PopUpService.EndedPopUp();
        }
    }
    private IInteraction DetectTarget()
    {
        Vector2 interactionPoint = GetPoint();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            interactionPoint,
            interactionRadius,
            interactableLayer
            );

        IInteraction closestInteraction = null;
        float closestDistance = Mathf.Infinity;

        for(int i = 0; i < colliders.Length; i++)
        {
            Collider2D candidateCollider = colliders[i];

            if(!candidateCollider.TryGetComponent(out IInteraction candidateInteraction))
            {
                continue;
            }

            Vector2 candidateInteractionCollider = candidateCollider.ClosestPoint(interactionPoint);

            float candidateDistance = Vector2.Distance(
            interactionPoint,
            candidateInteractionCollider
            );

            if(candidateDistance < closestDistance)
            {
                closestDistance = candidateDistance;
                closestInteraction = candidateInteraction;
            }
        }

        return closestInteraction;
    }

    private void OnInteraction()
    {
        if (GameplayStateService.CanAdvanceDialogue)
        {
            DialogueService.AdvanceDialogue();
            return;
        }

        if(!GameplayStateService.CanInteract)
            return;
            
        currentInteraction?.Execute();
    }

    private Vector2 GetPoint()
    {
        Vector2 lookDirection;
        
        if(playerInputReader == null || playerInputReader.LastMoveDirection == Vector2.zero)
        {            
            lookDirection = Vector2.down;
        }
        else
        {
            lookDirection = playerInputReader.LastMoveDirection;            
        }

        Vector2 playerPosition = transform.position;

        Vector2 normalizedLookDirection = lookDirection.normalized;
        Vector2 interactionPoint = playerPosition + normalizedLookDirection * interactionDistance;
        return interactionPoint;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(GetPoint(), interactionRadius);
        Gizmos.DrawLine(transform.position, GetPoint());
    }
}
