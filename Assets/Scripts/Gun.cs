using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private AudioClip _shootClip;

    [SerializeField]
    private AudioClip _reloadClip;


    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayShootSound()
    {
        if (_audioSource != null)
        {
            _audioSource.clip = _shootClip;
            _audioSource.Play();
        }
    }

    public void ReloadSound()
    {
        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.clip = _reloadClip;
            _audioSource.Play();
        }
    }

}
