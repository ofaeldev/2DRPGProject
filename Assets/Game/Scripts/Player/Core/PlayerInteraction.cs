using UnityEngine;

/// <summary>
/// Gerencia detecção e execução de interações com objetos do mundo.
/// Detecta alvo de interação próximo ao jogador e executa ações ao pressionar a tecla.
/// </summary>
public class PlayerInteraction : PlayerInputSubscriber
{
    [SerializeField] private float interactionDistance;
    [SerializeField] private float interactionRadius;
    [SerializeField] private LayerMask interactableLayer;
    private IInteraction currentInteraction;
    private PlayerInteractionResolver interactionResolver;

    public override void Initialize(PlayerInputReader reader)
    {
        base.Initialize(reader);
        interactionResolver = new PlayerInteractionResolver(transform, interactableLayer, interactionDistance, interactionRadius, reader);
        Debug.Log($"[PlayerInteraction] Initialized with distance={interactionDistance}, radius={interactionRadius}", this);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameplayStateService.StateChanged += OnStateChange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameplayStateService.StateChanged -= OnStateChange;
    }

    /// <summary>Reage a mudanças no estado do gameplay (locked/unlocked).</summary>
    private void OnStateChange()
    {
        if (!GameplayStateService.CanInteract)
        {
            currentInteraction = null;
            PopUpService.EndedPopUp();
        }
    }

    private void Update()
    {
        if (GameplayStateService.CanInteract)
            UpdateCurrentInteraction();
    }

    /// <summary>Conecta ao evento InteractionRequested do input reader.</summary>
    protected override void ConnectToInputEvents()
    {
        playerInputReader.InteractionRequested += OnInteraction;
    }

    /// <summary>Desconecta do evento InteractionRequested do input reader.</summary>
    protected override void DisconnectFromInputEvents()
    {
        playerInputReader.InteractionRequested -= OnInteraction;
    }
    /// <summary>Atualiza o alvo de interação atual baseado na detecção.</summary>
    private void UpdateCurrentInteraction()
    {
        IInteraction detectedInteraction = interactionResolver != null ? interactionResolver.DetectTarget() : null;
        
        if (currentInteraction == detectedInteraction)
            return;

        currentInteraction = detectedInteraction;        
        
        if (currentInteraction != null)
        {
            PopUpService.StartedPopUp(currentInteraction);
            Debug.Log($"[PlayerInteraction] Detected interactable: {currentInteraction}", this);
        }
        else
        {
            PopUpService.EndedPopUp();
        }
    }

    /// <summary>Executa a interação atual ou avança diálogo se disponível.</summary>
    private void OnInteraction()
    {
        if (GameplayStateService.CanAdvanceDialogue)
        {
            DialogueService.AdvanceDialogue();
            return;
        }

        if (!GameplayStateService.CanInteract)
            return;
        
        if (currentInteraction != null)
        {
            Debug.Log($"[PlayerInteraction] Executing: {currentInteraction}", this);
            currentInteraction.Execute();
        }
    }

    /// <summary>Desenha Gizmos para visualizar a área de detecção de interação.</summary>
    private void OnDrawGizmos()
    {
        if (interactionResolver == null)
            return;

        Gizmos.color = Color.yellow;

        Vector2 interactionPoint = interactionResolver.GetPoint();
        Gizmos.DrawWireSphere(interactionPoint, interactionRadius);
        Gizmos.DrawLine(transform.position, interactionPoint);
    }
}
