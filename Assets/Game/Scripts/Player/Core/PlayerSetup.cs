using UnityEngine;

/// <summary>
/// Orquestrador central de inicialização do sistema de player.
/// Busca todos os componentes do player, cria o PlayerInputReader compartilhado,
/// e inicializa cada subsistema com suas dependências corretas.
/// </summary>
[RequireComponent(typeof(PlayerInputSource))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(PlayerInventory))]
public class PlayerSetup : MonoBehaviour
{
    [Header("Configurações")]
    [Tooltip("Scriptable Object com as configurações do jogador")]
    [SerializeField] private PlayerSettings playerSettings;

    private PlayerController playerController;
    private PlayerInputReader inputReader;

    private void Awake()
    {
        Debug.Log("[PlayerSetup] Starting player initialization...", this);

        if (playerSettings == null)
        {
            Debug.LogError("[PlayerSetup] PlayerSettings não foi atribuído no Inspector! Inicialização cancelada.", this);
            return;
        }

        playerController = GetComponent<PlayerController>();
        if (playerController == null)
        {
            Debug.LogError("[PlayerSetup] PlayerController not found on this GameObject.", this);
            return;
        }

        PlayerInputSource inputSource = GetComponent<PlayerInputSource>();
        PlayerMovement movement = GetComponent<PlayerMovement>();
        PlayerInteraction interaction = GetComponent<PlayerInteraction>();
        PlayerInventory inventory = GetComponent<PlayerInventory>();
        PlayerAnimator playerAnimator = GetComponentInChildren<PlayerAnimator>();
        Animator animator = GetComponentInChildren<Animator>();

        if (playerAnimator == null)
        {
            Debug.LogError("[PlayerSetup] PlayerAnimator not found in children.", this);
            return;
        }

        if (animator == null)
        {
            Debug.LogError("[PlayerSetup] Animator component not found in children.", this);
            return;
        }

        // Cria o leitor de input compartilhado
        inputReader = new PlayerInputReader();
        Debug.Log("[PlayerSetup] Created shared PlayerInputReader instance", this);

        // Inicializa o InputSource (conecta ao Input System)
        inputSource.Initialize(
            inputReader,
            playerController.MoveActionReference,
            playerController.InteractionActionReference,
            playerController.InventoryActionReference);

        // Inicializa cada subsistema com as configurações
        movement.Initialize(inputReader, playerSettings);
        interaction.Initialize(inputReader, playerSettings);
        inventory.Initialize(inputReader);
        playerController.Initialize(inputReader);
        playerAnimator.Initialize(animator, movement, inputReader);

        Debug.Log("[PlayerSetup] Player initialization complete!", this);
    }
}