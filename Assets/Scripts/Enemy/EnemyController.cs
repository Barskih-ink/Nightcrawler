using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Настройка")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask playerLayer;
    public float attackCooldown = 1f;
    private float lastAttackTime;

    [Header("Позиционка")]
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    private Vector3 targetPoint;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        targetPoint = pointB.position;
    }

    void Update()
    {
        Patrol();
        TryAttack();
    }

    void Patrol()
    {
        Vector2 direction = (targetPoint - transform.position).normalized;
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

      
        if (direction.x > 0)
            spriteRenderer.flipX = false;
        else if (direction.x < 0)
            spriteRenderer.flipX = true;

        
        if (Vector2.Distance(transform.position, targetPoint) < 0.2f)
        {
            targetPoint = targetPoint == pointA.position ? pointB.position : pointA.position;
        }
    }

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        Collider2D player = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
        if (player != null)
        {
            Debug.Log("Нанес урон!");
            // player.GetComponent<PlayerController>().TakeDamage(damage); 
            lastAttackTime = Time.time;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        Gizmos.color = Color.blue;
        if (pointA != null)
            Gizmos.DrawSphere(pointA.position, 0.1f);
        if (pointB != null)
            Gizmos.DrawSphere(pointB.position, 0.1f);
    }
}
