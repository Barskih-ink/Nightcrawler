using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"������ {other.name} ����� � ���� ������");

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            Debug.Log("����� ������, ������� ����������� ����");
            playerHealth.TakeDamage(playerHealth.currentHealth);
        }
    }
}
