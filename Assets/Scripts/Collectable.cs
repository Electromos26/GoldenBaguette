using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Collectable : MonoBehaviour
{
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _collectableClip;

    [SerializeField]
    private UnityEvent events;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void PlayTrack()
    {
        if (_audioSource != null && !_audioSource.isPlaying)
        {

            _audioSource.clip = _collectableClip;
            //_audioSource.Play();
        }
    }
}
