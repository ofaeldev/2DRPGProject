using UnityEngine;

/// <summary>
/// Gerencia a atualização de parâmetros do Animator baseado no estado de movimento do jogador.
/// Normaliza vetores de movimento e atualiza floats do Animator para animações corretas.
/// </summary>
public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;
    private PlayerInputReader playerInputReader;

    /// <summary>Hash do parâmetro animator MoveX (movimento horizontal).</summary>
    private static readonly int MoveX = Animator.StringToHash("MoveX");
    
    /// <summary>Hash do parâmetro animator MoveY (movimento vertical).</summary>
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    
    /// <summary>Hash do parâmetro animator LastMoveX (última direção horizontal).</summary>
    private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
    
    /// <summary>Hash do parâmetro animator LastMoveY (última direção vertical).</summary>
    private static readonly int LastMoveY = Animator.StringToHash("LastMoveY");
    
    /// <summary>Hash do parâmetro animator Speed (magnitude do movimento).</summary>
    private static readonly int Speed = Animator.StringToHash("Speed");

    /// <summary>Inicializa o componente com referências necessárias.</summary>
    /// <param name="newAnimator">Componente Animator do jogador.</param>
    /// <param name="movement">Componente PlayerMovement para verificar estado canMove.</param>
    /// <param name="inputReader">Leitor de input para acessar direções de movimento.</param>
    public void Initialize(Animator newAnimator, PlayerMovement movement, PlayerInputReader inputReader)
    {
        if (newAnimator == null)
        {
            Debug.LogError("[PlayerAnimator] Animator is null during initialization.", this);
            return;
        }

        animator = newAnimator;
        playerMovement = movement;
        playerInputReader = inputReader;

        Debug.Log("[PlayerAnimator] Initialized with Animator and dependencies", this);
    }

    /// <summary>Atualiza os parâmetros de movimento atual no Animator.</summary>
    /// <param name="moveInput">Vetor de movimento a normalizar e aplicar.</param>
    public void SetMovement(Vector2 moveInput)
    {
        if (animator == null)
            return;

        Vector2 normalizedMove = moveInput.normalized;
        animator.SetFloat(MoveX, normalizedMove.x);
        animator.SetFloat(MoveY, normalizedMove.y);
        animator.SetFloat(Speed, normalizedMove.magnitude);
    }

    /// <summary>Atualiza a última direção de movimento válida no Animator (para idle correto).</summary>
    /// <param name="lastMove">Última direção de movimento válida.</param>
    public void SetLastMove(Vector2 lastMove)
    {
        if (animator == null)
            return;

        Vector2 normalizedLastMove = lastMove.normalized;
        animator.SetFloat(LastMoveX, normalizedLastMove.x);
        animator.SetFloat(LastMoveY, normalizedLastMove.y);
    }

    private void Update()
    {
        if (playerMovement == null || animator == null || playerInputReader == null)
        {
            return;
        }

        // Se o jogador não pode se mover, força animação de parado
        if (!playerMovement.CanMove)
        {
            SetMovement(Vector2.zero);
            return;
        }

        // Atualiza os parâmetros de animação
        SetLastMove(playerInputReader.LastMoveDirection);
        SetMovement(playerInputReader.MoveDirection);
    }
}