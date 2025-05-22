using UnityEngine;

public class PlayerWallGrab : MonoBehaviour
{
    [Header("Wall Grab Settings")]
    public Transform[] wallCheckPoints;
    public float wallCheckDistance = 0.5f;
    public LayerMask wallLayer;
    public float wallJumpForce = 5f;

    private Rigidbody2D rb;
    private Animator animator;

    public bool isGrabbingWall = false;
    public bool IsGrabbingWall => isGrabbingWall;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Зацеп за край (ЛКМ при касании стены)
        if (Input.GetMouseButtonDown(0) && IsTouchingWall())
        {
            isGrabbingWall = true;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0f;

            if (animator != null)
                animator.SetBool("IsHanging", true);
        }

        // Отпрыгивание от стены (если зацепился)
        if (isGrabbingWall && Input.GetKeyDown(KeyCode.Space))
        {
            rb.gravityScale = 1f;
            isGrabbingWall = false;

            // Прыжок в сторону от стены
            Vector2 jumpDir = Vector2.right;
            if (IsTouchingWallLeft()) jumpDir = Vector2.right;
            else if (IsTouchingWallRight()) jumpDir = Vector2.left;

            rb.linearVelocity = new Vector2(jumpDir.x * wallJumpForce, wallJumpForce);

            if (animator != null)
                animator.SetBool("IsHanging", false);
        }

        // Если отпустил стену или стал на землю
        if (isGrabbingWall && (!IsTouchingWall() || IsGrounded()))
        {
            StopWallGrab();
        }
    }

    private bool IsTouchingWall()
    {
        foreach (var point in wallCheckPoints)
        {
            if (IsTouchingWallLeft() || IsTouchingWallRight())
                return true;
        }
        return false;
    }

    private bool IsTouchingWallLeft()
    {
        foreach (var point in wallCheckPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(point.position, Vector2.left, wallCheckDistance, wallLayer);
            Debug.DrawRay(point.position, Vector2.left * wallCheckDistance, Color.blue);
            if (hit.collider != null)
                return true;
        }
        return false;
    }

    private bool IsTouchingWallRight()
    {
        foreach (var point in wallCheckPoints)
        {
            RaycastHit2D hit = Physics2D.Raycast(point.position, Vector2.right, wallCheckDistance, wallLayer);
            Debug.DrawRay(point.position, Vector2.right * wallCheckDistance, Color.red);
            if (hit.collider != null)
                return true;
        }
        return false;
    }

    private bool IsGrounded()
    {
        RaycastHit2D groundHit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, wallLayer);
        return groundHit.collider != null;
    }

    public void StopWallGrab()
    {
        isGrabbingWall = false;
        rb.gravityScale = 1f;

        if (animator != null)
            animator.SetBool("IsHanging", false);
    }
}
