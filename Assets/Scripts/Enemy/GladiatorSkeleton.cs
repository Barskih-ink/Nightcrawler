using UnityEngine;

public class GladiatorSkeleton : Enemy
{
    protected override void Start()
    {
        base.Start();
        shieldBlockPercentage = 0.5f;  // ��������� 75% �����
        shieldDuration = 1f;
        shieldCooldown = 4f;
    }

    protected override void ActivateShield()
    {
        base.ActivateShield();
        Debug.Log("GladiatorSkeleton ��������� ���");
        // ����� �������� �������� ��� �������
    }

    protected override void Attack()
    {
        Debug.Log("GladiatorSkeleton ������� � �����!");
        base.Attack();
    }
}
