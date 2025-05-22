using UnityEngine;

public class PlayerLadderClimb : MonoBehaviour
{
    public float climbSpeed = 4f;
    private bool isClimbing;
    private float inputVertical;

    private Rigidbody2D rb;
    private float originalGravity;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalGravity = rb.gravityScale;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        inputVertical = Input.GetAxisRaw("Vertical");

        if (isClimbing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, inputVertical * climbSpeed);

            if (animator != null)
                animator.SetBool("IsClimbing", true); // ¬ Àﬁ◊¿≈Ã ‡ÌËÏ‡ˆË˛
        }
        else
        {
            if (animator != null)
                animator.SetBool("IsClimbing", false); // ¬€ Àﬁ◊¿≈Ã
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = true;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isClimbing = false;
            rb.gravityScale = originalGravity;
        }
    }
}
