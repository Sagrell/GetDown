using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Audio[] sounds;
    #region Singleton
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        foreach (Audio a in sounds)
        {
            a.source = gameObject.AddComponent<AudioSource>();
            a.source.clip = a.clip;
            a.source.volume = a.volume;
            a.source.pitch = a.pitch;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
    #endregion


    public void Play(string name)
    {
        Audio a = Array.Find(sounds, sound => sound.name == name);
        a.source.Play();
    }
    // Update is called once per frame

}
