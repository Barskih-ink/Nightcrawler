using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth;
    private Animator animator;
    public bool isDead { get; private set; } = false;


    public Image healthBarFill; // <-- ���� � ���������� ���������� Image (� fill amount)

    private void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return; // ���� ��� ����, �� ����������

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }


    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        // ���������� ����� ����� 1.5 ������� (������ ������� ��� ��������)
        Invoke(nameof(ReloadLevel), 2.5f);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
