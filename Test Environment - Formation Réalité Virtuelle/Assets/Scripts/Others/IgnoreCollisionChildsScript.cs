using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Titre : ignore Collision Childs Script
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     Permet à un Gameobjet d'ignorer les collisions avec les enfants de celui-ci.
///     
/// </summary>

public class IgnoreCollisionChildsScript : MonoBehaviour {

    public GameObject[] childs = new GameObject[2];

    void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), childs[0].GetComponent<Collider>(), true);
        Physics.IgnoreCollision(this.GetComponent<Collider>(), childs[1].GetComponent<Collider>(), true);
    }
}
