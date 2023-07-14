using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("ProgrammingGym_LD"); // add scene name here 
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
