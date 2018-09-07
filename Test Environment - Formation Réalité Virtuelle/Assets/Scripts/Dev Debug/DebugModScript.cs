using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

/// <summary>
/// Titre : DebugModScript
/// Auteur : VRTK, modifier par GOISLOT Renaud
/// Description :
/// 
///     Script de debug permettant de suivre dans les logs les appels d'event en fonction des interactions faites avec une manette de HTC Vive.
///     A placer en component d'un objet Manette HTC Vive
///     
/// </summary>

public class DebugModScript : MonoBehaviour {

    public string boutonDebugTag;

    private GameObject[] btnsDebug;

    private VRTK_ControllerEvents events;
    private bool isTriggerPressed;
    private bool isGripPressed;
    private bool isTouchpadPressed;

    private bool isDebugActive;
    private bool isBtnReleased;
    private bool isMenuActive;

    // Use this for initialization
    void Start () {
        isDebugActive = false;
        isTriggerPressed = false;
        isGripPressed = false;
        isTouchpadPressed = false;

        isBtnReleased = true;

        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerAppearance_Example", "VRTK_ControllerEvents", "the same"));
            return;
        }

        events = GetComponent<VRTK_ControllerEvents>();

        //Setup controller event listeners

        events.GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        events.GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

        events.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        events.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        events.TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        events.TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

    }

    // Update is called once per frame
    void Update () {
        IsDebugMenuActive();

        if(isMenuActive == true)
        {
            if (isBtnReleased == true)
            {
                if (isTriggerPressed == true && isGripPressed == true && isTouchpadPressed == true)
                {
                    isDebugActive = !isDebugActive;
                    Debug.Log("Debug Mode " + isDebugActive);
                    StatutDebugBtn(isDebugActive);

                    isBtnReleased = false;
                    
                }
            }
            else if (isTriggerPressed == false || isGripPressed == false || isTouchpadPressed == false)
            {
                isBtnReleased = true;
            }
        }
		
	}

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.Log("Btn Trigger Enfoncé");
        isTriggerPressed = true;
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.Log("Btn Trigger Relaché");
        isTriggerPressed = false;
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.Log("Btn Grip Enfoncé");
        isGripPressed = true;
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.Log("Btn Grip Relaché");
        isGripPressed = false;
    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.Log("Btn Touchpad Enfoncé");
        isTouchpadPressed = true;
    }

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        //Debug.Log("Btn Touchpad Relaché");
        isTouchpadPressed = false;
    }

    private void StatutDebugBtn(bool isDebugActive)
    {
        foreach (GameObject btn in btnsDebug)
        {
            btn.GetComponent<Button>().interactable = isDebugActive;
        }
    }

    private void IsDebugMenuActive()
    {
        if(GameObject.FindGameObjectsWithTag(boutonDebugTag).Length != 0)
        {
            btnsDebug = GameObject.FindGameObjectsWithTag(boutonDebugTag);
            isMenuActive  = true;
        }
        else
        {
            isMenuActive = false;
        }

    }


}
