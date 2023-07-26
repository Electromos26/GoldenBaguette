using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{

    private Light flashLight;
    private bool isOn;

    [SerializeField]
    private AudioClip _flashlightClip;

    [SerializeField]
    private GameObject flashlightIcon;

    [SerializeField]
    private GameObject gunLight;

    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        flashLight = GetComponent<Light>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_audioSource != null)
            {
                _audioSource.clip = _flashlightClip;
                _audioSource.loop = false;
                _audioSource.Play();
            }

            isOn = !isOn;
            flashlightIcon.gameObject.SetActive(isOn);

            
        }
        if (Input.GetButton("Fire2"))
        {

            gunLight.gameObject.SetActive(isOn);
            flashLight.enabled = false;

        }
        else
        {
            gunLight.gameObject.SetActive(false);
            flashLight.enabled = isOn;
        }
    }

}
