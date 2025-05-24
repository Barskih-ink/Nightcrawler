using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;

    private float moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerWallGrab wallGrab;
    private PlayerHealth health;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallGrab = GetComponent<PlayerWallGrab>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        if (health != null && health.isDead) return;

        moveInput = Input.GetAxisRaw("Horizontal");

        // Отражение спрайта по направлению
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (health != null && health.isDead) return;

        // Блокируем движение при хвате за стену
        if (wallGrab != null && wallGrab.IsGrabbingWall)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * MoveSpeed, rb.linearVelocity.y);
    }
}
