using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

/// <summary>
/// Titre : Tooltips Event
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     DEPRECATED ON THIS PROJET
///     (Préférer créer un script général de description d'un objet en fonction d'un temps donné avec l'inspector)    
/// 
///     Destruction d'un objet tooltip en fonction d'un temps donné.
///     
/// </summary>

public class TooltipsEventScript : MonoBehaviour
{

    [Header("Controller tooltips Settings")]

    public GameObject controllerTooltips;
    public float timeVisibleTooltips;

    void Update ()
    {
        Destroy(controllerTooltips, timeVisibleTooltips);
    }

}
