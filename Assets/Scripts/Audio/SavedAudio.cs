using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedAudio
{
    public AudioSource audioSource;
    float initialVolume;
    float newVolume;

    public SavedAudio(AudioSource source)
    {
        this.audioSource = source;
        this.initialVolume = source.volume; //might be different
    }
    public void SetAudioQuiet()
    {
        this.audioSource.volume = initialVolume * 0.1f; //do somethign to reduce

    }
    public void RestoreVolume()
    {
        this.audioSource.volume = initialVolume;
    }
}
