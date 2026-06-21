using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerInputReader playerInputReader;
    public float speed { get; private set; } = 3f;
    private Rigidbody2D rb2D;
    private bool canMove;
    public bool CanMove => canMove;

    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }
    public void Initialize(PlayerInputReader playerInputReader)
    {
        this.playerInputReader = playerInputReader;
    }
    void OnEnable()
    {
        canMove = GameplayStateService.CanMove;
        
        GameplayStateService.StateChanged += OnStateChanged;
    }

    void OnDisable()
    {
        GameplayStateService.StateChanged -= OnStateChanged;
    }

    private void OnStateChanged()
    {
        canMove = GameplayStateService.CanMove;
    }

    public void MovePlayer()
    {
        Vector2 moveDirection = playerInputReader.MoveDirection;
        moveDirection.Normalize();

        rb2D.linearVelocity = moveDirection * speed;
    }

    private void FixedUpdate()
    {
        if (playerInputReader == null) return;
        if (!canMove)
        {
          rb2D.linearVelocity = Vector2.zero;
          return;   
        }              

        MovePlayer();
    }
    
}
