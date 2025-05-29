using System.Collections.Generic;
using UnityEngine;

public class SkillsTreeController : MonoBehaviour
{
    [System.Serializable]
    public class SkillDefinition
    {
        public string id;          // ���������� �������������, �������� "health_boost"
        public string skillName;
        public Sprite icon;
        public int price;
        [TextArea]
        public string description;

        [HideInInspector]
        public bool isPurchased = false;  // ���� ��������
    }


    public GameObject skillItemPrefab;
    public Transform skillsContainer;
    public List<SkillDefinition> skills = new List<SkillDefinition>();

    private Dictionary<string, SkillItem> instantiatedItems = new Dictionary<string, SkillItem>();
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void OnEnable()
    {
        PopulateSkills();
    }

    private void PopulateSkills()
    {
        instantiatedItems.Clear();
        foreach (Transform child in skillsContainer)
            Destroy(child.gameObject);

        foreach (var def in skills)
        {
            GameObject go = Instantiate(skillItemPrefab, skillsContainer);
            var item = go.GetComponent<SkillItem>();
            item.Setup(def.id, def.icon, def.skillName, def.price, def.description, def.isPurchased);
            item.OnPurchaseRequested += OnPurchaseRequested;
            instantiatedItems[def.id] = item;
        }
    }

    private void OnPurchaseRequested(string skillId)
    {
        var def = skills.Find(s => s.id == skillId);
        if (def == null || def.isPurchased) return;

        if (playerHealth.TrySpendSouls(def.price))
        {
            def.isPurchased = true;
            // ��������� UI-������
            instantiatedItems[skillId].MarkPurchased();
            // ��������� ������
            ApplySkillEffect(def);
        }
        else
        {
            Debug.Log("������������ ��� ��� ������� ������ " + def.skillName);
        }
    }

    private void ApplySkillEffect(SkillDefinition def)
    {
        switch (def.id)
        {
            case "extra_heal":
                // ����������� ������������ ���������� ������������� ����� �� 1
                var playerHealth1 = FindFirstObjectByType<PlayerHealth>();
                if (playerHealth1 != null)
                {
                    playerHealth1.maxHeals += 1;
                    playerHealth1.GenerateHealIcons();
                    playerHealth1.RefillHeals(); // ��������� UI � �������
                }
                break;

            case "attack_up":
                // ����������� ���� ������ �� 1
                var combat = FindFirstObjectByType<PlayerCombat>();
                if (combat != null)
                {
                    combat.attackDamage += 1;
                }
                break;

            case "heal_power":
                // ����������� ������������� ����� (healAmount) �� 1
                var playerHealth2 = FindFirstObjectByType<PlayerHealth>();
                if (playerHealth2 != null)
                {
                    playerHealth2.healAmount += 1;
                }
                break;
            case "speed_climb_up":  // ����� �����
                var playerMovement = FindFirstObjectByType<PlayerMovement>();
                var playerLadderClimb = FindFirstObjectByType<PlayerLadderClimb>();
                if (playerMovement != null)
                {
                    playerMovement.MoveSpeed += 1f; // ����������� �������� ���� �� 1
                }
                if (playerLadderClimb != null)
                {
                    playerLadderClimb.climbSpeed += 1f; // ����������� �������� ������� �� 1
                }
                break;

            default:
                Debug.LogWarning("��� ��������� ������� ��� " + def.id);
                break;
        }
    }
}
