using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourceOnce;
    [SerializeField] private AudioSource audioSourceLoop;
 
    void Start()
    {
        if (audioSourceOnce != null)
        {
            audioSourceOnce.volume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        }
        if (audioSourceLoop != null)
        {
            audioSourceLoop.volume = PlayerPrefs.GetFloat("MasterVolume", 1.0f);
        }
    }

    private void Update()
    {
        audioSourceOnce.volume = PlayerPrefs.GetFloat("MasterVolume", 0.3f);
        audioSourceLoop.volume = PlayerPrefs.GetFloat("MasterVolume", 0.3f);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSourceOnce.loop = false;
        audioSourceOnce.PlayOneShot(clip);
    }

    public void PlaySoundLoop(AudioClip clip)
    {
        audioSourceLoop.loop = true;
        audioSourceLoop.clip = clip;
        audioSourceLoop.Play();
    }

    public void StopLoopSound()
    {
        audioSourceLoop.Stop();
    }
}
