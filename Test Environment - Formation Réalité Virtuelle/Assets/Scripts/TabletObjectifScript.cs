using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class TabletObjectifScript : MonoBehaviour {

    public LevelGameManager levelGameManager;
    public GameObject tabletPanels;

    [Header("Material Settings")]
    public GameObject TabletPart1;
    public GameObject TabletPart2;
    public Material baseMaterial;

    [Header("Countdown Settings")]
    public TextMeshPro countdownText;
    public TextMeshPro tvCountdownText;
    public float timeLeft = 10.0f;
    public GameObject tvCountdownPanel;

    protected GameObject interactableObject;
    protected VRTK_InteractGrab grabbingController;

    protected bool isGrabbed = false;
    protected bool stop = true;
    private float seconds;

    protected Material baseMaterialBtnDown;

    protected virtual void Awake()
    {
        VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void Start()
    {
        interactableObject = gameObject;
        if (interactableObject == null || interactableObject.GetComponent<VRTK_InteractableObject>() == null)
        {
            VRTK_Logger.Warn(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "ElevatorCommand", "VRTK_InteractableObject", "a parent"));
            return;
        }

        interactableObject.GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(DoInteractableObjectIsGrabbed);
        interactableObject.GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(DoInteractableObjectIsUngrabbed);

        GetComponent<VRTK_InteractableObject>().ToggleHighlight(false);
    }

    private void OnTriggerStay(Collider collider)
    {
        grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());
    }

    void Update()
    {
        if (isGrabbed == true)
        {
            if (TabletPart1.GetComponent<MeshRenderer>().material != baseMaterial)
            {
                TabletPart1.GetComponent<MeshRenderer>().material = baseMaterial;
            }

            if (TabletPart2.GetComponent<MeshRenderer>().material != baseMaterial)
            {
                TabletPart2.GetComponent<MeshRenderer>().material = baseMaterial;
            }

            tabletPanels.transform.GetChild(0).gameObject.SetActive(false);
            tabletPanels.transform.GetChild(1).gameObject.SetActive(true);
            tvCountdownPanel.SetActive(true);

            stop = false;
            StartCoroutine(StartCountdown());
        }

        if (isGrabbed == false)
        {
            
        }

        if (stop == false)
        {

            timeLeft -= Time.deltaTime;
            seconds = timeLeft % 60;
        }

    }

    private IEnumerator StartCountdown()
    {
        while (!stop)
        {
            countdownText.text = string.Format("{0:0}", seconds);
            tvCountdownText.text = string.Format("{0:0}", seconds);
            if (seconds < 0)
            {
                stop = true;
                AnimationBegin();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void AnimationBegin()
    {
        tvCountdownPanel.SetActive(false);
        levelGameManager.MotorMakeBegin();
        grabbingController.ForceRelease(false);
        Destroy(gameObject);
    }


    protected virtual void OnDestroy()
    {
        VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
    }


    protected virtual void DoInteractableObjectIsGrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = true;
    }

    protected virtual void DoInteractableObjectIsUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = false;
    }

}
