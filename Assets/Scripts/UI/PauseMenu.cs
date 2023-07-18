using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool paused;
    public GameObject pauseMenu;

     void Pause()
     {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        paused = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        paused = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//get enter button
        {
            if (!paused)
            {
               Pause();
            }
            // If the pause menu is currently active, deactivate it
            else
            {
                Resume();
            }
        }
    }
 
}