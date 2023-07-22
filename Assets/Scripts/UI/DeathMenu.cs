using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
   
    public GameObject deathScreen;
    public bool isDead = false;
    void DiedScreen()
    {
        Time.timeScale = 0;
        deathScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;

    }
    public void TryAgain()
    {
        Time.timeScale = 1;
        deathScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        isDead = false;
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("isDead: " + isDead);
        if (isDead)
       {
         DiedScreen();
       }
       
    }
}
