using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float airMoveSpeedFactor = 0.5f; // Tốc độ di chuyển trên không
    public float baseJumpForce = 10f;
    private float currentJumpForce;
    public bool canMove = true;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float moveInput;
    private float storedMoveInput;

    [Header("Jump Settings")]
    public float maxHoldTime = 3f;
    public float forceIncreasePerSecond = 2f;
    private float holdTime;
    private bool isHoldingSpace;
    public Slider jumpForceBar;
    public bool canJump = true;
    public bool canPlayJumpSound = true;

    [Header("Horizontal Jump Charge")]
    public float maxHoldMoveTime = 2f; // Giới hạn thời gian tích lực di chuyển
    public float moveForceIncreasePerSecond = 2f; // Tăng lực di chuyển theo thời gian giữ phím
    private float holdMoveTime = 0f; // Thời gian giữ phím di chuyển

    [Header("Buff Settings")]
    public float fireCampJumpBoost = 2f;
    public float buffDuration = 5f;
    private bool isBuffed;
    private Coroutine buffCoroutine;

    [Header("Effect")]
    private AudioManager audioManager;
    private PlayerAnimation playerAnimation;

    public bool isPaused = false; // Biến kiểm soát trạng thái tạm dừng

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentJumpForce = baseJumpForce;
        audioManager = FindAnyObjectByType<AudioManager>();
        playerAnimation = GetComponent<PlayerAnimation>();
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
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (!isHoldingSpace)
        {
            PlayerMovementLogic();
        }

        if (canJump)
        {
            PlayerJump();
        }

        playerAnimation.UpdateAnimation(moveInput, rb.linearVelocity, isGrounded, isHoldingSpace);
        
    }

    public void PlayerMovementLogic()
    {
        if (!canMove) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        if (isHoldingSpace)
        {
            if (moveInput != 0) // Nếu đang giữ phím di chuyển trong lúc giữ nhảy
            {
                holdMoveTime += Time.deltaTime;
                holdMoveTime = Mathf.Min(holdMoveTime, maxHoldMoveTime); // Giới hạn tối đa thời gian tích lực
                storedMoveInput = moveInput; // Ghi nhớ hướng nhảy
            }
            return;
        }

        float currentMoveSpeed = isGrounded ? moveSpeed : moveSpeed * airMoveSpeedFactor;
        rb.linearVelocity = new Vector2(moveInput * currentMoveSpeed, rb.linearVelocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    public void PlayerJump()
    {
        if (isPaused || !canJump)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            isHoldingSpace = true;
            holdTime += Time.deltaTime;
            holdTime = Mathf.Min(holdTime, maxHoldTime);

            // Nếu đang di chuyển thì dừng ngay lập tức
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

            // Ghi nhớ hướng di chuyển trước khi nhảy
            if (moveInput != 0)
            {
                storedMoveInput = moveInput;
            }

            if (jumpForceBar != null)
            {
                jumpForceBar.value = holdTime / maxHoldTime;
            }
        }

        if (isHoldingSpace && !isGrounded)
        {
            isHoldingSpace = false;
            holdTime = 0f;

            if (jumpForceBar != null)
            {
                jumpForceBar.value = 0;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space) && isGrounded && isHoldingSpace)
        {
            float jumpForce = currentJumpForce + (holdTime * forceIncreasePerSecond);

            // Nhảy theo hướng đã lưu trước đó
            rb.linearVelocity = new Vector2(storedMoveInput * moveSpeed * 2.0f, jumpForce);

            if (!isPaused)
            {
                audioManager.PlayJumpSound();
            }

            holdTime = 0f;
            isHoldingSpace = false;

            if (jumpForceBar != null)
            {
                jumpForceBar.value = 0;
            }
        }

    }

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

    private IEnumerator ApplyJumpBuff()
    {
        isBuffed = true;
        currentJumpForce = baseJumpForce + fireCampJumpBoost;
        yield return new WaitForSeconds(buffDuration);
        currentJumpForce = baseJumpForce;
        isBuffed = false;
    }
}

