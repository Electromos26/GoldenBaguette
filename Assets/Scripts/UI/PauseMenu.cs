using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool paused;
    public GameObject pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//get enter button
        {
            /*if (!Paused)
            {
                Paused = true;
                pauseMenu.SetActive(true);
            }
            // If the pause menu is currently active, deactivate it
            else
            {
                Paused = false;
                pauseMenu.SetActive(false);
            }*/
        }
    }
}
