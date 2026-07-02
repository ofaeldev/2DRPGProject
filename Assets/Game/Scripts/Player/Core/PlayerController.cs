using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Orquestrador central do sistema de controle do jogador.
/// Mantém referências para as ações de input e coordena a inicialização dos subsistemas.
/// Delega funcionalidades específicas para componentes especializados.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private PlayerAnimator playerAnimator;
    
    /// <summary>Referência da ação de movimento (esquerda/cima/direita/baixo).</summary>
    [SerializeField] private InputActionReference moveActionReference;
    
    /// <summary>Referência da ação de interação (pressionar para interagir com objetos).</summary>
    [SerializeField] private InputActionReference interactionActionReference;
    
    /// <summary>Referência da ação de inventário (abrir/fechar inventário).</summary>
    [SerializeField] private InputActionReference inventoryActionReference;

    private PlayerInputReader playerInputReader;

    /// <summary>Inicializa o controlador com o leitor de input compartilhado.</summary>
    public void Initialize(PlayerInputReader reader)
    {
        if (reader == null)
        {
            Debug.LogError("[PlayerController] PlayerInputReader cannot be null.", this);
            return;
        }

        playerInputReader = reader;
        Debug.Log("[PlayerController] Initialized with PlayerInputReader", this);
    }    

    /// <summary>Ação de movimento (leitura).</summary>
    public InputActionReference MoveActionReference => moveActionReference;
    
    /// <summary>Ação de interação (leitura).</summary>
    public InputActionReference InteractionActionReference => interactionActionReference;
    
    /// <summary>Ação de inventário (leitura).</summary>
    public InputActionReference InventoryActionReference => inventoryActionReference;
}