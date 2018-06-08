﻿using UnityEngine;
using VRTK;
using VRTK.Highlighters;

public class ControllerBtnHighlightHelp : MonoBehaviour
{
    public bool tooltipsOnlyOneTime;

    public bool noTooltipsMod;

    private VRTK_ControllerTooltips tooltips;
    private VRTK_ControllerHighlighter highligher;
    private VRTK_ControllerEvents events;
    private Color highlightColor = Color.yellow;
    private Color pulseColor = Color.black;
    private Color currentPulseColor;
    private float highlightTimer = 0.5f;
    private float pulseTimer = 0.75f;
    private float dimOpacity = 0.8f;
    private float defaultOpacity = 1f;
    private bool highlighted;
    private bool HelpToolTips;


    private void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerAppearance_Example", "VRTK_ControllerEvents", "the same"));
            return;
        }

        events = GetComponent<VRTK_ControllerEvents>();
        highligher = GetComponent<VRTK_ControllerHighlighter>();
        tooltips = GetComponentInChildren<VRTK_ControllerTooltips>();
        currentPulseColor = pulseColor;
        highlighted = false;
        HelpToolTips = true;

        if(noTooltipsMod == true)
        {
            tooltips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
        }

        //Setup controller event listeners

        /*events.GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        events.GripReleased += new ControllerInteractionEventHandler(DoGripReleased);*/

        events.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        events.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);



    }

       
    /*private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (highlighted == false)
        {
            tooltips.ToggleTips(true, VRTK_ControllerTooltips.TooltipButtons.GripTooltip);
            highligher.HighlightElement(SDK_BaseController.ControllerElements.GripLeft, highlightColor, highlightTimer);
            highligher.HighlightElement(SDK_BaseController.ControllerElements.GripRight, highlightColor, highlightTimer);
            VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), dimOpacity);
            highlighted = true;
        }
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (highlighted == true)
        {
            tooltips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.GripTooltip);
            highligher.UnhighlightElement(SDK_BaseController.ControllerElements.GripLeft);
            highligher.UnhighlightElement(SDK_BaseController.ControllerElements.GripRight);
            if (!events.AnyButtonPressed())
            {
                VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), defaultOpacity);
            }
            highlighted = false;
        }
    }*/

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        if (highlighted == false)
        {
            if(noTooltipsMod == false)
            {
                if (HelpToolTips == true && tooltipsOnlyOneTime == true)
                {
                    tooltips.ToggleTips(true, VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
                }
                else if (tooltipsOnlyOneTime == false)
                {
                    tooltips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
                }
            }
            
            
            highligher.HighlightElement(SDK_BaseController.ControllerElements.Trigger, highlightColor, highlightTimer);
            highligher.HighlightElement(SDK_BaseController.ControllerElements.Trigger, highlightColor, highlightTimer);
            VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), dimOpacity);
            highlighted = true;
        }
        else if (highlighted == true)
        {
            if (noTooltipsMod == false)
            {
                if (HelpToolTips == true && tooltipsOnlyOneTime == true)
                {
                    tooltips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
                    HelpToolTips = false;
                }
                else if (tooltipsOnlyOneTime == false)
                {
                    tooltips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
                }
            }
            


            highligher.UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
            highligher.UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
            if (!events.AnyButtonPressed())
            {
                VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), defaultOpacity);
            }
            highlighted = false;
        }
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        if (highlighted == true)
        {
            if (noTooltipsMod == false)
            {
                if (HelpToolTips == true && tooltipsOnlyOneTime == true)
                {
                    tooltips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
                    HelpToolTips = false;
                }
                else if (tooltipsOnlyOneTime == false)
                {
                    tooltips.ToggleTips(true, VRTK_ControllerTooltips.TooltipButtons.TriggerTooltip);
                }
            }

            highligher.UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
            highligher.UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
            if (!events.AnyButtonPressed())
            {
                VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), defaultOpacity);
            }
            highlighted = false;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        OnTriggerStay(collider);
    }

    /*private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<VRTK_InteractableObject>() != null)
        {
            if (collider.gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable == true && highlighted == false)
            {
                //tooltips.ToggleTips(true, VRTK_ControllerTooltips.TooltipButtons.GripTooltip);
                highligher.HighlightElement(SDK_BaseController.ControllerElements.GripLeft, highlightColor, highlightTimer);
                highligher.HighlightElement(SDK_BaseController.ControllerElements.GripRight, highlightColor, highlightTimer);
                VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), dimOpacity);

                highlighted = true;
            }
        }
            
    }


    private void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject.GetComponent<VRTK_InteractableObject>() != null)
        {
            if (collider.gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable == true)
            {

                //tooltips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.GripTooltip);
                highligher.UnhighlightElement(SDK_BaseController.ControllerElements.GripLeft);
                highligher.UnhighlightElement(SDK_BaseController.ControllerElements.GripRight);
                if (!events.AnyButtonPressed())
                {
                    VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), defaultOpacity);
                }

                highlighted = false;
            }
        }
    }*/

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.GetComponent<VRTK_InteractableObject>() != null)
        {
            if (collider.gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable == true && highlighted == false)
            {
                //tooltips.ToggleTips(true, VRTK_ControllerTooltips.TooltipButtons.GripTooltip);
                highligher.HighlightElement(SDK_BaseController.ControllerElements.Trigger, highlightColor, highlightTimer);
                highligher.HighlightElement(SDK_BaseController.ControllerElements.Trigger, highlightColor, highlightTimer);
                VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), dimOpacity);

                highlighted = true;
            }
        }

    }


    private void OnTriggerExit(Collider collider)
    {

        if (collider.gameObject.GetComponent<VRTK_InteractableObject>() != null)
        {
            if (collider.gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable == true)
            {

                //tooltips.ToggleTips(false, VRTK_ControllerTooltips.TooltipButtons.GripTooltip);
                highligher.UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
                highligher.UnhighlightElement(SDK_BaseController.ControllerElements.Trigger);
                if (!events.AnyButtonPressed())
                {
                    VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), defaultOpacity);
                }

                highlighted = false;
            }
        }
    }
}