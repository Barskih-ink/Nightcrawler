using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float acceleration = 10f;
    public float deceleration = 12f;

    [Header("Jumping")]
    public float jumpForce = 16f;
    public int maxJumps = 2;
    public float coyoteTime = 0.15f;
    public float jumpBufferTime = 0.1f;

    private int jumpCount;
    private float coyoteTimer;
    private float jumpBufferTimer;

    [Header("Wall Jump")]
    public float wallJumpForce = 12f;
    public Vector2 wallJumpDirection = new Vector2(1f, 1f);
    public float wallJumpDuration = 0.2f;
    public Transform wallCheck;
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;

    private bool isTouchingWall;
    private bool isWallJumping;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private bool isDashing;
    private float dashTimer;
    private float dashCooldownTimer;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Vector2 input;
    private bool isGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
        CheckGrounded();
        CheckWall();
        HandleJumpBuffer();
        dashCooldownTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing && dashCooldownTimer <= 0f)
        {
            StartDash();
        }
    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            Move();
            HandleJump();
        }
        else
        {
            dashTimer -= Time.fixedDeltaTime;
            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }
    }

    void HandleInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferTimer = jumpBufferTime;
        }
    }

    void Move()
    {
        float targetSpeed = input.x * moveSpeed;
        float speedDiff = targetSpeed - rb.linearVelocity.x;
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        float movement = speedDiff * accelRate;

        rb.AddForce(Vector2.right * movement, ForceMode2D.Force);

        // Flip character sprite based on direction
        if (input.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(input.x), 1, 1);
    }

    void HandleJump()
    {
        if (jumpBufferTimer > 0f)
        {
            if (isGrounded || coyoteTimer > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpBufferTimer = 0f;
                jumpCount = maxJumps - 1;
            }
            else if (jumpCount > 0 && !isGrounded && !isWallJumping)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                jumpBufferTimer = 0f;
                jumpCount--;
            }
            else if (isTouchingWall)
            {
                isWallJumping = true;
                rb.linearVelocity = Vector2.zero;
                Vector2 dir = new Vector2(-transform.localScale.x * wallJumpDirection.x, wallJumpDirection.y).normalized;
                rb.AddForce(dir * wallJumpForce, ForceMode2D.Impulse);
                jumpBufferTimer = 0f;
                jumpCount = maxJumps - 1;
                Invoke(nameof(StopWallJump), wallJumpDuration);
            }
        }
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;

        rb.linearVelocity = new Vector2(transform.localScale.x * dashForce, 0f);
    }

    void CheckGrounded()
    {
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded && !wasGrounded)
        {
            jumpCount = maxJumps;
            isWallJumping = false;
        }

        if (isGrounded)
        {
            coyoteTimer = coyoteTime;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }

    void CheckWall()
    {
        isTouchingWall = Physics2D.Raycast(wallCheck.position, Vector2.right * transform.localScale.x, wallCheckDistance, wallLayer);
    }

    void HandleJumpBuffer()
    {
        if (jumpBufferTimer > 0f)
        {
            jumpBufferTimer -= Time.deltaTime;
        }
    }

    void StopWallJump()
    {
        isWallJumping = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + Vector3.right * transform.localScale.x * wallCheckDistance);
        }
    }
}
