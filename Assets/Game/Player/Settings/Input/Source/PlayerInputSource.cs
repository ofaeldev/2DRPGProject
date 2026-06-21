using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSource : MonoBehaviour
{
    private PlayerInputReader playerInputReader;
    private InputAction moveAction;
    private InputAction interactionAction;
    private bool isConnected;

    public void Initialize(PlayerInputReader reader, InputActionReference moveActionReference, InputActionReference actionReference)
    {
        playerInputReader = reader;
        moveAction = moveActionReference.action;
        interactionAction = actionReference.action;

        if(isActiveAndEnabled)
        {
            Connect();
        }
    }

    private void OnEnable()
    {
        Connect();
    }
    private void OnDisable()
    {
        Disconnect();
    }
    private void Connect()
    {
        if(isConnected) return;

        if(playerInputReader == null || moveAction == null || interactionAction == null)
            return;

        moveAction.performed += OnMove;
        moveAction.canceled += OnMove;

        interactionAction.performed += OnInteraction;

        interactionAction.Enable();
        moveAction.Enable();

        isConnected = true;
    }

    private void Disconnect()
    {
        if (!isConnected)
            return;

        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMove;

        interactionAction.performed -= OnInteraction;

        interactionAction.Disable();
        moveAction.Disable();

        playerInputReader.SetMove(Vector2.zero);
        isConnected = false;
    }


    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        playerInputReader.SetMove(moveInput);
    }

    private void OnInteraction(InputAction.CallbackContext context)
    {
        playerInputReader.RequestInteraction();        
    }
}