using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("ProgrammingGym_Backup"); // open level
    }
}
