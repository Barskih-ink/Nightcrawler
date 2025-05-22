using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce = 8f;

    private Rigidbody2D rb;
    private PlayerWallGrab wallGrab;
    private PlayerMovement movement;

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        wallGrab = GetComponent<PlayerWallGrab>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            }
            else if (wallGrab != null && wallGrab.IsGrabbingWall) // УБРАНЫ СКОБКИ
            {
                // Wall Jump
                float wallJumpDirection = movement.MoveSpeed > 0 ? -1 : 1;
                rb.linearVelocity = new Vector2(wallJumpDirection * movement.MoveSpeed * 1.2f, jumpForce);

                wallGrab.StopWallGrab(); // ❗НЕ wallGrab.IsGrabbingWall = false;
            }
        }
    }
}
