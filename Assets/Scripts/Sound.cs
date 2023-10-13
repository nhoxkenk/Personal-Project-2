using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0,1)]
    public float volume;
    [Range(0.1f,1.3f)]
    public float pitch;

    [HideInInspector]
    public AudioSource audioSource;
}
