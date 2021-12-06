using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioClip birdSounds;
    private static AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        birdSounds = Resources.Load<AudioClip>("SoundEffects/Scene1/BirdsSounds");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.clip = birdSounds;
        audioSource.loop = true;
        audioSource.volume = 0.5f;
        audioSource.Play();
        
    }
}
