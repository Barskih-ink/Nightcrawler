using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerWallGrab wallGrab;

    private void Start()
    {
        animator = GetComponent<Animator>();
        wallGrab = GetComponent<PlayerWallGrab>();
    }

    private void Update()
    {
        // Не атаковать, если держимся за стену
        if (Input.GetMouseButtonDown(0) && (wallGrab == null || !wallGrab.IsGrabbingWall))
        {
            animator.SetTrigger("Attack");
        }
    }
}
