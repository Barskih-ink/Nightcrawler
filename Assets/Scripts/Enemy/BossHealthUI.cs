using UnityEngine;
using UnityEngine.UI;

public class BossHealthUI : MonoBehaviour
{
    public Image fillImage;         // ������� �������
    public Enemy boss;             // ������ �� DeadBoss

    private void Update()
    {
        if (boss != null && fillImage != null)
        {
            float fill = (float)boss.CurrentHealth / boss.MaxHealth;
            fillImage.fillAmount = fill;
        }

        // ��������� � ������
        transform.rotation = Camera.main.transform.rotation;
    }
}
