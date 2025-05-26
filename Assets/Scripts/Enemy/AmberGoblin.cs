using UnityEngine;

public class AmberGoblin : Enemy
{
    [Header("���������")]
    [Range(0f, 1f)]
    public float dodgeChance = 0.3f; // 25% ���� ���������

    protected override void Start()
    {
        base.Start();
        shieldBlockPercentage = 0f; // �� ���������� ���, ������ ���������
    }

    public override void TakeDamage(int damage)
    {
        if (Random.value < dodgeChance)
        {
            Debug.Log($"{gameObject.name} ��������� �� �����!");
            animator.SetTrigger("Dodge");
            return; // ��������� �������� �����
        }

        base.TakeDamage(damage);
    }

    protected override void Attack()
    {
        Debug.Log("AmberGoblin ������ ������� �����!");
        base.Attack();
    }

    // ������� ��������� ���� � ������ �� ����� �����������
    protected override void TryActivateShield() { }
    protected override void ActivateShield() { }
}
