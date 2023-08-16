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
        SceneManager.LoadScene("MainMenu"); // open level
        Time.timeScale = 1f;

    }

    public void Die()
    {
        Invoke("DisplayDeath", 4f);
    }

    private void DisplayDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        deathScreen.SetActive(true);

    }
    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
           
        }

    }
}
