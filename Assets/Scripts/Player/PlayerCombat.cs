using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private PlayerWallGrab wallGrab;
    public Transform attackPoint;
    public float attackRange = 0.3f;
    public int attackDamage = 1;
    public LayerMask enemyLayers;
    public Animator animator;

    private SpriteRenderer spriteRenderer;
    private PlayerHealth health;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        wallGrab = GetComponent<PlayerWallGrab>();
        animator = GetComponent<Animator>();
        health = GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (health != null && health.isDead) return;

        if (Input.GetMouseButtonDown(0))
        {
            // ћожно атаковать только если не хватаетс€ за стену
            if (!wallGrab.IsGrabbingWall)
            {
                animator.SetTrigger("Attack");
                //Attack();
            }
        }

        FlipAttackPoint(); // јвтоматически зеркалит точку атаки
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>()?.TakeDamage(attackDamage);
        }
    }

    void FlipAttackPoint()
    {
        if (spriteRenderer != null && attackPoint != null)
        {
            Vector3 localPos = attackPoint.localPosition;
            localPos.x = Mathf.Abs(localPos.x) * (spriteRenderer.flipX ? -1 : 1);
            attackPoint.localPosition = localPos;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
