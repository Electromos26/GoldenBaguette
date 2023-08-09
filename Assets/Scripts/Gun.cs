using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private AudioClip _shootClip;

    [SerializeField]
    private AudioClip _reloadClip;

    [SerializeField]
    private GameObject muzzleFlash;
   
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public void PlayShootSound()
    {
        StartCoroutine(MuzzleFlashAndPlaySound());
        if (_audioSource != null)
        {
            _audioSource.clip = _shootClip;
            _audioSource.Play();
        }
       
    }
    private IEnumerator MuzzleFlashAndPlaySound()
    {
        muzzleFlash.SetActive(true);

        // Wait for a specified duration
        yield return new WaitForSeconds(0.1f); // Change this value to your desired duration
        muzzleFlash.SetActive(false);
    }

    public void ReloadSound()
    {
        if (_audioSource.isPlaying && _audioSource.clip == _shootClip)
        {
            _audioSource.Stop();
        }

        if (_audioSource != null && !_audioSource.isPlaying)
        {
            _audioSource.clip = _reloadClip;
            _audioSource.Play();
        }
    }

}
