using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using VRTK;

/// <summary>
/// Titre : Controller Haptic Rumble Script
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     Système de vibration de manette en fonction d'une interaction effectuée
///     A placer en componente d'une manette de HTC Vive.
/// 
/// Effets : 
/// 
///     - Vibration de la manette en fonction d'un Event
///     
/// </summary>

public class ControllerHapticRumbleScript : MonoBehaviour {

    [Header("Haptics On Touch")]

    [Tooltip("Denotes how strong the rumble in the controller will be on touch.")]
    [Range(0, 1)]
    public float strengthOnTouch = 0;
    [Tooltip("Denotes how long the rumble in the controller will last on touch.")]
    public float durationOnTouch = 0f;
    [Tooltip("Denotes interval betweens rumble in the controller on touch.")]
    public float intervalOnTouch = minInterval;

    protected const float minInterval = 0.05f;

    private VRTK_ControllerEvents events;
    private VRTK_ControllerReference controllerReference;

    // Use this for initialization
    void Start () {

        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerAppearance_Example", "VRTK_ControllerEvents", "the same"));
            return;
        }

        events = GetComponent<VRTK_ControllerEvents>();
    }

    //Fonction de vibration de la manette (à appeler en Event)
    public void StartHapticTouch()
    {
        controllerReference = VRTK_ControllerReference.GetControllerReference(VRTK_DeviceFinder.GetModelAliasController(events.gameObject));
        VRTK_ControllerHaptics.TriggerHapticPulse(controllerReference, strengthOnTouch, durationOnTouch, intervalOnTouch);

    }
}
