using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(2); // open level
    }
}
