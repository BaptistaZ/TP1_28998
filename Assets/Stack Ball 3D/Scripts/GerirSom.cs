using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerirSom : MonoBehaviour
{
    public static GerirSom instance;
    private AudioSource audioSource;
    public bool sound = true;

    void Awake()
    {
        MakeSingleton();
        audioSource = GetComponent<AudioSource>();
    }

    void MakeSingleton()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SoundOnOff()
    {
        sound = !sound;
    }

    public void PlaySoundFX(AudioClip clip, float voulme)
    {
        if (sound)
            audioSource.PlayOneShot(clip, voulme);     
    }
}
