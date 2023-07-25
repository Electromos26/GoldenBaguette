using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{

    public GameObject deathScreen;
    public bool isDead = false;
    public void TryAgain()
    {
        isDead = false;
        Time.timeScale = 1f;
        deathScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        
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
            Cursor.visible = true;
            Time.timeScale = 0f;
            deathScreen.SetActive(true);
           
        }

    }
}
