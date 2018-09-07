using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Titre : RespawnObjet
/// Auteur : GOISLOT Renaud
/// 
/// Description : 
/// 
///     Script a mettre en component de chaque objets ou l'on souhaite avoir la fonctionnalité de respawn.
///     Permet de faire respawn à la position d'origine l'objet quand RespawnToStart() est appelé.
/// 
/// Effets : 
/// 
///     - Sauvegarde de la position d'origine de l'objet
///     - Respawn de l'objet à son emplacement d'origine
///     
/// </summary>

public class RespawnObject : MonoBehaviour {

    private Vector3 startPos;
    private Quaternion startRot;

	// Use this for initialization
	void Start () {
        startPos = transform.position;
        startRot = transform.rotation;
	}
	
    public void RespawnToStart()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
