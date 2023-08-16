using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private AudioClip _menuClip;

    private AudioSource _audioSource;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        mainMusic();
    }
    private void mainMusic()
    {
        if (_audioSource != null)
        {
            _audioSource.clip = _menuClip;
            _audioSource.Play();
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene"); // open level
    }
}
