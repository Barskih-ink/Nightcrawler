using UnityEngine;

public class SpecterSkeleton : Enemy
{
    protected override void Start()
    {
        base.Start();
        shieldBlockPercentage = 1f;   // ��������� 50% �����
        shieldDuration = 1.5f;
        shieldCooldown = 3f;
    }

    protected override void ActivateShield()
    {
        base.ActivateShield();
        Debug.Log("SpecterSkeleton ������ ���������� ������� ���!");
        // ����� �������� ������� ���������� ������
    }

    protected override void Attack()
    {
        Debug.Log("SpecterSkeleton ������� ������!");
        base.Attack();
    }
}
