using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    AudioSource[] allFiles;
    List<SavedAudio> allSavedAudio = new List<SavedAudio>();

    [SerializeField]
    GameObject tape;

    // Start is called before the first frame update
    void Start()
    {
        allFiles = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource source in allFiles)
        {
            allSavedAudio.Add(new SavedAudio(source));
        }
    }

    public void PlayFocusedAudio(AudioSource focusSound)
    {
        foreach (SavedAudio savedAudio in allSavedAudio)
        {
            if (savedAudio.audioSource != focusSound)
            {
                savedAudio.SetAudioQuiet();
            }
        }
        focusSound.Play();
        tape.SetActive(true);

        Invoke("ResumeAudio" ,focusSound.clip.length);

        //figure out a way to know when that audio source ends (should be event or use duration of the file)
        //once it's done, go over each saved audio file and return their volume to normal
    }

    private void ResumeAudio()
    {
        tape.SetActive(false);

        foreach (SavedAudio savedAudio in allSavedAudio)
        {
            if (savedAudio.audioSource != null)
            {
                savedAudio.RestoreVolume();
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
