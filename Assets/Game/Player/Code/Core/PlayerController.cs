using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInputSource))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInteraction))]
public class PlayerController : MonoBehaviour
{
    private PlayerInputReader playerInputReader;
    private PlayerInputSource playerInputSource;
    private PlayerMovement playerMovement;
    private PlayerInteraction playerInteraction;
    [SerializeField] private PlayerAnimator playerAnimator;
    [SerializeField] private InputActionReference moveActionReference;
    [SerializeField] private InputActionReference interactionActionReference;

    private void Awake()
    {
        playerInputSource = GetComponent<PlayerInputSource>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();

        playerInputReader = new PlayerInputReader();

        playerInteraction.Initialize(playerInputReader);
        playerInputSource.Initialize(playerInputReader, moveActionReference, interactionActionReference);   
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