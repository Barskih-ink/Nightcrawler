using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Здоровье")]
    public int maxHealth = 5;
    public int currentHealth;
    public Image healthBarFill;

    [Header("Хилки")]
    public int maxHeals = 1;
    public int currentHeals;
    public GameObject healIconPrefab;      // Префаб иконки хилки
    public Transform healContainer;        // Horizontal Layout Group
    private Image[] healIcons;             // Массив иконок для обновления цвета

    [Header("Настройки")]
    public int healAmount = 2;             // Сколько HP восстанавливает хилка
    public KeyCode healKey = KeyCode.E;    // Клавиша использования хилки

    private Animator animator;
    public bool isDead { get; private set; } = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        currentHeals = maxHeals;
        GenerateHealIcons();
        UpdateHealthUI();
        UpdateHealUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(healKey))
        {
            UseHeal();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead || currentHealth <= 0) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void UseHeal()
    {
        if (currentHeals <= 0 || isDead || currentHealth >= maxHealth) return;

        currentHeals--;
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();
        UpdateHealUI();

        // Воспроизвести анимацию лечения
        if (animator != null)
        {
            animator.SetTrigger("Heal");
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }
    }

    private void UpdateHealUI()
    {
        for (int i = 0; i < healIcons.Length; i++)
        {
            if (healIcons[i] != null)
            {
                healIcons[i].color = (i < currentHeals) ? Color.white : Color.gray;
            }
        }
    }

    private void GenerateHealIcons()
    {
        if (healContainer == null || healIconPrefab == null) return;

        // Очистить старые иконки
        foreach (Transform child in healContainer)
        {
            Destroy(child.gameObject);
        }

        healIcons = new Image[maxHeals];

        for (int i = 0; i < maxHeals; i++)
        {
            GameObject icon = Instantiate(healIconPrefab, healContainer);
            Image img = icon.GetComponent<Image>();
            if (img != null)
            {
                healIcons[i] = img;
            }
        }
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        Invoke(nameof(ReloadLevel), 2.5f);
    }

    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
