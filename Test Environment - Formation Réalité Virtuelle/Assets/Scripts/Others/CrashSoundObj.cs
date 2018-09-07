using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Titre : Crash Sound Obj
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///    Active un audio source quand le gameobjet entre en collision avec un autre gameobject
///     
/// </summary>

public class CrashSoundObj : MonoBehaviour {

    //récupération de l'audio
    public AudioClip crashSound;

    private AudioSource source;

    //Randomisation aigu / grave pour donner une illusion de son différent (valeur max / mini possibles)
    private float lowPitchRange = .75F;
    private float highPitchRange = 1.5F;

    //Magnitude minimal pour activation du son
    private float lowMagnitude = 0.5F;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    //Action si entre en collision avec un objet
    private void OnCollisionEnter(Collision collision)
    {
        //On vérifie si le son n'est pas déjà joué et si la force de collision est supérieur à lowMagnitude
        if(!source.isPlaying && collision.relativeVelocity.magnitude >= lowMagnitude)
        {
            //Volume adapter en fonction de la force de collision
            source.volume = collision.relativeVelocity.magnitude / 20;
            //Randomisation du pitch
            source.pitch = Random.Range(lowPitchRange, highPitchRange);
            source.PlayOneShot(crashSound, 1F);
        }
        
    }
}
