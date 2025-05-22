using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // Имя вашей игровой сцены
    }

    public void ExitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
    }
}
