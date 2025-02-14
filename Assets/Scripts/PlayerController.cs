using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Animator animator;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float baseJumpForce = 10f;
    private float currentJumpForce;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private float storedMoveInput;

    [Header("Jump Setting")]
    public float maxHoldTime = 3f;
    public float forceIncreasePerSecond = 2f;
    private float holdTime;
    private bool isHoldingSpace;
    public Slider jumpForceBar;
    public bool canJump = true; // Điều khiển việc nhảy

    [Header("Buff Settings")]
    public float fireCampJumpBoost = 2f;
    public float buffDuration = 5f;
    private bool isBuffed;
    private Coroutine buffCoroutine;

    [Header("Effect")]
    private AudioManager audioManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentJumpForce = baseJumpForce;
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    void Start()
    {
        if (jumpForceBar != null)
        {
            jumpForceBar.value = 0;
        }
    }

    void Update()
    {
        PlayerMovement();
        if (canJump) // Chỉ nhảy khi được phép
        {
            PlayerJump();
        }
        UpdateAnimation();
    }

    // di chuyển 
    public void PlayerMovement()
    {
        moveInput = Input.GetAxis("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isHoldingSpace && isGrounded)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // nhảy 
    public void PlayerJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!canJump) return; // Không cho phép nhảy nếu bị vô hiệu hóa

        // Nếu đang giữ nhảy và vẫn trên mặt đất
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            isHoldingSpace = true;
            holdTime += Time.deltaTime;
            holdTime = Mathf.Min(holdTime, maxHoldTime);

            if (moveInput != 0)
            {
                storedMoveInput = moveInput;
            }

            if (jumpForceBar != null)
            {
                jumpForceBar.value = holdTime / maxHoldTime;
            }
        }

        // Nếu đang giữ nhảy mà bị rơi xuống
        if (isHoldingSpace && !isGrounded)
        {
            isHoldingSpace = false;
            holdTime = 0f;

            if (jumpForceBar != null)
            {
                jumpForceBar.value = 0;
            }
        }

        // Nhả phím nhảy khi còn đứng trên mặt đất
        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && isHoldingSpace)
        {
            float jumpForce = currentJumpForce + (holdTime * forceIncreasePerSecond);
            rb.linearVelocity = new Vector2(storedMoveInput * moveSpeed * 2.0f, jumpForce);
            audioManager.PlayJumpSound();
            holdTime = 0f;
            isHoldingSpace = false;

            if (jumpForceBar != null)
            {
                jumpForceBar.value = 0;
            }
        }
    }

    // animation 
    public void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f;
        bool isJumping = !isGrounded;
        bool isHolding = Input.GetKey(KeyCode.Space) && isGrounded;

        if (isRunning)
        {
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumping", isJumping);
            animator.SetBool("isHolding", false);
        }
        else
        {
            animator.SetBool("isRunning", false);

            if (isHoldingSpace)
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

    // nhận buff khi chạm vào lửa trại 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FireCamp"))
        {
            if (buffCoroutine != null)
            {
                StopCoroutine(buffCoroutine);
            }
            buffCoroutine = StartCoroutine(ApplyJumpBuff());
        }
    }

    // quản lý thời gian buff 
    private IEnumerator ApplyJumpBuff()
    {
        isBuffed = true;
        currentJumpForce = baseJumpForce + fireCampJumpBoost;
        yield return new WaitForSeconds(buffDuration);
        currentJumpForce = baseJumpForce;
        isBuffed = false;
    }
}

