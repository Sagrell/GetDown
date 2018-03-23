using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    
    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float maxVolume;
    [HideInInspector]
    public int id;
    [HideInInspector]
    public AudioClip clip;
    [HideInInspector]
    public AudioSource source;
}