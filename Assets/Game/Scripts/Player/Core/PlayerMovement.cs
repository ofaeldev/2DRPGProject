using UnityEngine;

/// <summary>
/// Gerencia o movimento físico do jogador via Rigidbody2D.
/// Aplica velocidade baseada no input do jogador, respeitando locks do GameplayStateService.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputReader playerInputReader;
    private Rigidbody2D rb2D;
    private PlayerSettings playerSettings;
    private bool canMove;

    /// <summary>Indica se o jogador pode se mover atualmente.</summary>
    public bool CanMove => canMove;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        if (rb2D == null)
        {
            Debug.LogError("[PlayerMovement] Rigidbody2D not found. Movement will not work.", this);
        }
    }

    /// <summary>Inicializa o componente de movimento com o leitor de input e configurações.</summary>
    public void Initialize(PlayerInputReader reader, PlayerSettings settings)
    {
        if (reader == null)
        {
            Debug.LogError("[PlayerMovement] PlayerInputReader cannot be null.", this);
            return;
        }

        if (settings == null)
        {
            Debug.LogError("[PlayerMovement] PlayerSettings cannot be null.", this);
            return;
        }

        playerInputReader = reader;
        playerSettings = settings;
        Debug.Log($"[PlayerMovement] Initialized with speed={playerSettings.moveSpeed} u/s", this);
    }

    private void OnEnable()
    {
        canMove = GameplayStateService.CanMove;
        GameplayStateService.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        GameplayStateService.StateChanged -= OnStateChanged;
    }

    /// <summary>Atualiza o estado local quando GameplayStateService muda.</summary>
    private void OnStateChanged()
    {
        bool wasCanMove = canMove;
        canMove = GameplayStateService.CanMove;

        if (wasCanMove != canMove)
        {
            if (!canMove)
            {
                Debug.Log("[PlayerMovement] Movement locked by GameplayStateService", this);
            }
        }
    }

    /// <summary>Aplica a velocidade ao Rigidbody2D baseado no input normalizado.</summary>
    private void MovePlayer()
    {
        if (rb2D == null || playerInputReader == null || playerSettings == null)
            return;

        Vector2 moveDirection = playerInputReader.MoveDirection.normalized;
        rb2D.linearVelocity = moveDirection * playerSettings.moveSpeed;
    }

    private void FixedUpdate()
    {
        if (playerInputReader == null)
        {
            return;
        }

        if (!canMove)
        {
            if (rb2D != null)
            {
                rb2D.linearVelocity = Vector2.zero;
            }
            return;
        }

        MovePlayer();
    }
}
