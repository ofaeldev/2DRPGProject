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
    private bool canMove;

    /// <summary>Velocidade de movimento padrão em unidades por segundo.</summary>
    private const float DEFAULT_SPEED = 3f;

    /// <summary>Velocidade de movimento, configurável no Inspector. Intervalo: 0-10 u/s.</summary>
    [Range(0f, 10f)]
    [SerializeField] private float speed = DEFAULT_SPEED;

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

    /// <summary>Inicializa o componente de movimento com o leitor de input.</summary>
    public void Initialize(PlayerInputReader reader)
    {
        if (reader == null)
        {
            Debug.LogError("[PlayerMovement] PlayerInputReader cannot be null.", this);
            return;
        }

        playerInputReader = reader;
        Debug.Log($"[PlayerMovement] Initialized with speed={speed} u/s", this);
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
        if (rb2D == null || playerInputReader == null)
            return;

        Vector2 moveDirection = playerInputReader.MoveDirection.normalized;
        rb2D.linearVelocity = moveDirection * speed;
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
