using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Souls UI")]
    public int currentSouls = 0;
    public TMP_Text soulsCountText;  // Ссылка на UI-текст в правом верхнем углу

    [Header("Экран смерти")]
    public GameObject deathScreenUI;


    private Animator animator;
    public bool isDead { get; private set; } = false;

    private PlayerMovement playerMovement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>(); // получаем ссылку
        currentHealth = maxHealth;

        currentHeals = maxHeals;
        GenerateHealIcons();
        UpdateHealthUI();
        UpdateHealUI();
        UpdateSoulsUI();
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

    public bool TrySpendSouls(int amount)
    {
        if (currentSouls >= amount)
        {
            currentSouls -= amount;
            UpdateSoulsUI();
            return true;
        }
        return false;
    }

    // Метод для добавления душ
    public void AddSouls(int amount)
    {
        currentSouls += amount;
        UpdateSoulsUI();
    }

    // Метод для сброса душ при смерти
    public void ResetSouls()
    {
        currentSouls = 0;
        UpdateSoulsUI();
    }

    private void UpdateSoulsUI()
    {
        if (soulsCountText != null)
        {
            soulsCountText.text = currentSouls.ToString();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead || currentHealth <= 0) return;

        // Если игрок перекатывается, игнорируем урон
        if (playerMovement != null && playerMovement.IsRolling)
        {
            return;
        }

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

    public void GenerateHealIcons()
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
        ResetSouls();
        animator.SetTrigger("Die");

        if (deathScreenUI != null)
        {
            deathScreenUI.SetActive(true);
        }

        StartCoroutine(HandleDeathRoutine());
    }

    private System.Collections.IEnumerator HandleDeathRoutine()
    {
        yield return new WaitForSeconds(2.5f);

        if (CheckpointManager.Instance != null && CheckpointManager.Instance.hasCheckpoint)
        {
            // Телепорт к костру
            transform.position = CheckpointManager.Instance.respawnPosition;

            HealToFull();
            RefillHeals();

            isDead = false;
            animator.ResetTrigger("Die");
            animator.Play("idle");

            if (deathScreenUI != null)
            {
                deathScreenUI.SetActive(false);
            }
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


    private void ReloadLevel()
    {
        if (CheckpointManager.Instance != null && CheckpointManager.Instance.hasCheckpoint)
        {
            // Телепортируем на чекпоинт
            transform.position = CheckpointManager.Instance.respawnPosition;

            // Восстановим здоровье и хилки
            HealToFull();
            RefillHeals();

            // Сброс состояний анимации
            isDead = false;
            animator.ResetTrigger("Die");
            animator.Play("idle"); // или "Respawn" — убедись, что такое состояние есть

            // Можно добавить небольшую неуязвимость после возрождения
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }




    public void HealToFull()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void RefillHeals()
    {
        currentHeals = maxHeals;
        UpdateHealUI();
    }

}
