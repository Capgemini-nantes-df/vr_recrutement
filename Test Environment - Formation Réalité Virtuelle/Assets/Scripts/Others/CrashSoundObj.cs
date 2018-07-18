using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrashSoundObj : MonoBehaviour {

    public AudioClip crashSound;

    private AudioSource source;
    private float lowPitchRange = .75F;
    private float highPitchRange = 1.5F;
    private float lowMagnitude = 0.5F;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!source.isPlaying && collision.relativeVelocity.magnitude >= lowMagnitude)
        {
            source.volume = collision.relativeVelocity.magnitude / 20;
            source.pitch = Random.Range(lowPitchRange, highPitchRange);
            source.PlayOneShot(crashSound, 1F);
        }
        
    }
}
