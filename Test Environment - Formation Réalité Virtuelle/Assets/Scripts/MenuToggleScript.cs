﻿using UnityEngine;
using VRTK;

public class MenuToggleScript : MonoBehaviour {

    [Header("Menu Toggle Settings")]
    
    public GameObject pauseMenu;

    [Header("Change pointer Controller Settings")]

    public GameObject leftController;
    public GameObject rightController;

    bool pauseMenuState = false;

    private VRTK_Pointer left_VRTK_Pointer;
    private VRTK_Pointer right_VRTK_Pointer;

    private VRTK_ControllerEvents leftControllerEvents;
    private VRTK_ControllerEvents rightControllerEvents;

    // Use this for initialization
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        leftControllerEvents = leftController.GetComponent<VRTK_ControllerEvents>();
        leftControllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;

        rightControllerEvents = rightController.GetComponent<VRTK_ControllerEvents>();
        rightControllerEvents.ButtonTwoReleased += ControllerEvents_ButtonTwoReleased;
    }

    private void OnDisable()
    {
        leftControllerEvents = leftController.GetComponent<VRTK_ControllerEvents>();
        leftControllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;

        rightControllerEvents = rightController.GetComponent<VRTK_ControllerEvents>();
        rightControllerEvents.ButtonTwoReleased -= ControllerEvents_ButtonTwoReleased;
    }

    private void ControllerEvents_ButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        HideShowMenu();
       
    }

    public void HideShowMenu()
    {
        pauseMenuState = !pauseMenuState;
        pauseMenu.SetActive(pauseMenuState);

        //leftController.GetComponent<VRTK_BezierPointerRenderer>().enabled = !pauseMenuState;
        leftController.GetComponent<VRTK_Pointer>().enabled = pauseMenuState;
        leftController.GetComponent<VRTK_StraightPointerRenderer>().enabled = pauseMenuState;

        //rightController.GetComponent<VRTK_BezierPointerRenderer>().enabled = !pauseMenuState;
        rightController.GetComponent<VRTK_StraightPointerRenderer>().enabled = pauseMenuState;
        rightController.GetComponent<VRTK_Pointer>().enabled = pauseMenuState;

        left_VRTK_Pointer = leftController.GetComponent<VRTK_Pointer>();
        right_VRTK_Pointer = rightController.GetComponent<VRTK_Pointer>();

        if (pauseMenuState == true)
        {
            left_VRTK_Pointer.pointerRenderer = leftController.GetComponent<VRTK_StraightPointerRenderer>();
            left_VRTK_Pointer.enableTeleport = false;

            right_VRTK_Pointer.pointerRenderer = rightController.GetComponent<VRTK_StraightPointerRenderer>();
            right_VRTK_Pointer.enableTeleport = false;

            leftController.GetComponent<VRTK_InteractTouch>().enabled = false;
            leftController.GetComponent<VRTK_InteractGrab>().enabled = false;
            leftController.GetComponent<VRTK_InteractUse>().enabled = false;

            rightController.GetComponent<VRTK_InteractTouch>().enabled = false;
            rightController.GetComponent<VRTK_InteractGrab>().enabled = false;
            rightController.GetComponent<VRTK_InteractUse>().enabled = false;
        }
        else
        {
            left_VRTK_Pointer.pointerRenderer = leftController.GetComponent<VRTK_BezierPointerRenderer>();
            left_VRTK_Pointer.enableTeleport = true;

            right_VRTK_Pointer.pointerRenderer = rightController.GetComponent<VRTK_BezierPointerRenderer>();
            right_VRTK_Pointer.enableTeleport = true;

            leftController.GetComponent<VRTK_InteractTouch>().enabled = true;
            leftController.GetComponent<VRTK_InteractGrab>().enabled = true;
            leftController.GetComponent<VRTK_InteractUse>().enabled = true;

            rightController.GetComponent<VRTK_InteractTouch>().enabled = true;
            rightController.GetComponent<VRTK_InteractGrab>().enabled = true;
            rightController.GetComponent<VRTK_InteractUse>().enabled = true;
        }
    }

}
