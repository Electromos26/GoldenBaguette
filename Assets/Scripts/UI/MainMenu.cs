using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// For the Main Menu, you want to be loading scenes from names of levels and not things like (2) for consistency's sake.
/// 
/// I think this screen could use a bit of work and definitely some sound effects to it to add ambience. As well, I want to see which buttons I have hovered over, so look into the button's highlighted and pressed colors.
/// UI Consistency is an issue right now artistically. I think that you need to make sure that the buttons are all the same size, the same visual style, etc., and I recommend coming up with a stylized button prefab that you use and then just change the words for. That would save you a lot of time and deal with the inconsistency issues.
/// </summary>
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
