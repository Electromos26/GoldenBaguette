using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
 
    public bool gameOver = false;

    //  public string TimerText;
    //  private float Timer;

    [SerializeField]
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
    public void OnCreateInstance()
    {

        currentSpot = GetComponentsInChildren<AISpot>();

    }
   


    void Update()
    {

    }

    public void QuitLevel()
    {
        Application.Quit();//actual game
        //UnityEditor.EditorApplication.isPlaying = false;//editor playMode
    }
    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
