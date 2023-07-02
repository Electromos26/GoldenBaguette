using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public int scoreTotal = 0;//player score
 
    public bool gameOver = false;

    public string TimerText;
    private float Timer;

    internal AISpot[] currentSpot;

    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameManager>();
                _instance.OnCreateInstance();

            }
            return _instance;
        }
    }

    void OnCreateInstance()
    {

        currentSpot = GetComponentsInChildren<AISpot>();

    }
   


    void Update()
    {
        //timer code

        //if ()
        //{
        //    Timer += Time.deltaTime;
        //    int minutes = Mathf.FloorToInt(Timer / 60F);
        //    int seconds = Mathf.FloorToInt(Timer % 60F);
        //    int milliseconds = Mathf.FloorToInt((Timer * 100F) % 100F);
        //    TimerText = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        //} 

    }
    //resume level

    //public void ResumeLevel()
    //{
    //    GameEnd.Paused = false;
    //    GameEnd.pauseMenu.SetActive(false);
    //}

    //public void QuitLevel()
    //{
    //    Application.Quit();//actual game
    //    UnityEditor.EditorApplication.isPlaying = false;//editor playMode
    //}
    //public void RestartLevel()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}
}
