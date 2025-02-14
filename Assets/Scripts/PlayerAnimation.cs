using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateAnimation(float moveInput, Vector2 velocity, bool isGrounded, bool isHoldingSpace)
    {
        bool isRunning = Mathf.Abs(velocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        bool isHolding = isHoldingSpace && isGrounded;

        if (isRunning)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isHolding", false);
        }
        else
        {
            animator.SetBool("isRunning", false);

            if (isHolding)
            {
                animator.SetBool("isHolding", true);
                animator.SetBool("isJumping", false);
            }
            else if (isJumping)
            {
                animator.SetBool("isJumping", true);
                animator.SetBool("isHolding", false);
            }
            else
            {
                animator.SetBool("isHolding", false);
                animator.SetBool("isJumping", false);
            }
        }
    }
}
