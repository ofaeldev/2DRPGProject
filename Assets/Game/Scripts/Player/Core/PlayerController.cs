using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputSource))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteraction))]
[RequireComponent(typeof(PlayerInventory))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputReader playerInputReader;
    private PlayerInputSource playerInputSource;
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private InputActionReference moveActionReference;
    [SerializeField] private InputActionReference interactionActionReference;
    [SerializeField] private InputActionReference inventoryActionReference;

    private void Awake()
    {
        playerInputSource = GetComponent<PlayerInputSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerInventory = GetComponent<PlayerInventory>();

        playerInputReader = new PlayerInputReader();

        playerInteraction.Initialize(playerInputReader);
        playerInventory.Initialize(playerInputReader);
        playerInputSource.Initialize(playerInputReader, moveActionReference, interactionActionReference, inventoryActionReference);   
        playerMovement.Initialize(playerInputReader);
    }

    private void Update()
    {
        
        if (!playerMovement.CanMove)
        {
            playerAnimator.SetMovement(Vector2.zero);
            return;
        }

        playerAnimator.SetLastMove(playerInputReader.LastMoveDirection);
        playerAnimator.SetMovement(playerInputReader.MoveDirection);
    }
}