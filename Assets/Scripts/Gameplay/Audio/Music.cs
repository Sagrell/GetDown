using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Music
{
    public string name;
    [Range(0f, 1f)]
    public float volume;
    [Range(0f, 1f)]
    public float maxVolume;
    public bool isLooping;
    
    [HideInInspector]
    public AudioClip clip;

}