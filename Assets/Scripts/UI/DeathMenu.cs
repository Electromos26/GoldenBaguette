using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public GameObject deathScreen;
    public bool isDead = false;
    public void TryAgain()
    {
        Time.timeScale = 1;
        deathScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isDead = false;
    }
    public void ExitGame()
    {
        Application.Quit();//actual game
        UnityEditor.EditorApplication.isPlaying = false;//editor playMode
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("isDead: " + isDead);
        if (isDead)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            deathScreen.SetActive(true);
           
        }

    }
}
