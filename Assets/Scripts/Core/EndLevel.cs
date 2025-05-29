using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [Tooltip("�����, �������� ����� ������")]
    public GameObject player;

    [Tooltip("��� ����� �������� ����")]
    public string mainMenuSceneName = "MainMenu";

    private bool levelEnded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (levelEnded) return;

        if (other.CompareTag("Player"))
        {
            levelEnded = true;

            Debug.Log("����� �������� �����. ���������� ������.");

            if (player != null)
            {
                player.SetActive(false); // ������ ������
            }

            // ��������� ����
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
