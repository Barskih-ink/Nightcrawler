using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f; // Имя с большой буквы, чтобы другие скрипты видели это свойство
    private float moveInput;

    private Rigidbody2D rb;
    private PlayerWallGrab wallGrab;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallGrab = GetComponent<PlayerWallGrab>();
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
    }

    private void FixedUpdate()
    {
        // Блокируем движение, если игрок цепляется за стену
        if (wallGrab != null && wallGrab.IsGrabbingWall) // УБРАНЫ скобки ()
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        rb.linearVelocity = new Vector2(moveInput * MoveSpeed, rb.linearVelocity.y); // MoveSpeed с большой буквы
    }
}
