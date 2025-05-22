using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    private float moveInput;

    private Rigidbody2D rb;
    private PlayerWallGrab wallGrab;
    private Animator animator;
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallGrab = GetComponent<PlayerWallGrab>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // или GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // === ОБНОВЛЕНИЕ ПАРАМЕТРОВ АНИМАЦИИ ===
        if (animator != null && playerController != null)
        {
            animator.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x));
            animator.SetBool("IsGrounded", playerController.IsGrounded());
        }

        // === ФЛИП ПЕРСОНАЖА ===
        if (spriteRenderer != null)
        {
            if (moveInput > 0)
                spriteRenderer.flipX = false;
            else if (moveInput < 0)
                spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (wallGrab != null && wallGrab.IsGrabbingWall)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * MoveSpeed, rb.linearVelocity.y);
    }
}
