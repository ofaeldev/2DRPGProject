using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int MoveX = Animator.StringToHash("MoveX");
    private static readonly int MoveY = Animator.StringToHash("MoveY");
    private static readonly int LastMoveX = Animator.StringToHash("LastMoveX");
    private static readonly int LastMoveY = Animator.StringToHash("LastMoveY");
    private static readonly int Speed = Animator.StringToHash("Speed");

    public void SetMovement(Vector2 moveInput)
    {
        Vector2 normalizedMove = moveInput.normalized;
        animator.SetFloat(MoveX, normalizedMove.x);
        animator.SetFloat(MoveY, normalizedMove.y);
        animator.SetFloat(Speed, normalizedMove.magnitude);
    }

    public void SetLastMove(Vector2 lastMove)
    {
        Vector2 normalizedLastMove = lastMove.normalized;
        animator.SetFloat(LastMoveX, normalizedLastMove.x);
        animator.SetFloat(LastMoveY, normalizedLastMove.y);
    }
}