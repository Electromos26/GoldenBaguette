using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private AudioClip _shootClip;

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
}
