using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;

    private float moveInput;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private PlayerWallGrab wallGrab;
    private PlayerHealth health;
    private Animator animator;

    private bool isRolling = false;
    public float rollDuration = 0.5f;  // длительность переката

    private float rollTimer = 0f;

    public bool IsRolling => isRolling;  // только геттер для проверки состояния




    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallGrab = GetComponent<PlayerWallGrab>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = GetComponent<PlayerHealth>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (health != null && health.isDead) return;

        // Если перекатываемся - не обновляем ввод движения
        if (isRolling)
        {
            rollTimer -= Time.deltaTime;
            if (rollTimer <= 0f)
            {
                EndRoll();
            }
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");

        // Перекат по кнопке (например, LeftShift или другой)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            // Проверяем, что персонаж на земле и стоит или бежит
            bool isGrounded = GetComponent<PlayerJump>().IsGrounded();
            float speed = Mathf.Abs(rb.linearVelocity.x);

            if (isGrounded && (speed > 0.1f || speed < 0.1f))
            {
                StartRoll();
                return;
            }
        }

        // Отражение спрайта по направлению
        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
        }

        Debug.Log("MoveInput: " + moveInput + " | Input: " + Input.GetAxisRaw("Horizontal"));


    }

    private void FixedUpdate()
    {
        if (health != null && health.isDead) return;

        if (isRolling)
        {
            // Во время переката - движемся с фиксированной скоростью вперед
            float rollSpeed = moveInput != 0 ? Mathf.Sign(moveInput) * MoveSpeed * 1.5f : (spriteRenderer.flipX ? -1 : 1) * MoveSpeed * 1.5f;
            rb.linearVelocity = new Vector2(rollSpeed, rb.linearVelocity.y);
            return;
        }

        // Блокируем движение при хвате за стену
        if (wallGrab != null && wallGrab.IsGrabbingWall)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * MoveSpeed, rb.linearVelocity.y);
    }

    private void StartRoll()
    {
        isRolling = true;
        rollTimer = rollDuration;
        // Запустить анимацию переката
        animator.SetBool("isRolling", true);

        // Возможно, отключить управление движением на время переката (уже сделано через isRolling)
        // Также можно отключить коллайдер или сделать уворот на время переката (например, невосприимчивость)
    }

    private void EndRoll()
    {
        isRolling = false;
        animator.SetBool("isRolling", false);
    }
}
