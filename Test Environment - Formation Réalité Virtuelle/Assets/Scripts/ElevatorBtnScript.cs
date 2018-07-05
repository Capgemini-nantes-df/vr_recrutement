using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class ElevatorBtnScript : MonoBehaviour {

    public enum TouchpadPressPosition
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }

    public Animator commandAnim;
    public LevelGameManager levelGameManager;
    public GameObject command;
    public Material baseMaterial;

    [Header("Sound Settings")]
    public AudioSource downBtnEffect;

    [Header("Buttons Settings")]
    public GameObject buttonUp;
    public GameObject buttonDown;
    public Material outlinedMaterialBtnDown;

    protected VRTK_ControllerEvents controllerEvents;
    protected VRTK_PanelMenuItemController currentPanelMenuItemController;
    protected GameObject interactableObject;

    // Swipe sensitivity / detection.
    protected const float AngleTolerance = 90f;

    protected readonly Vector2 xAxis = new Vector2(1, 0);
    protected readonly Vector2 yAxis = new Vector2(0, 1);
    protected Vector2 touchStartPosition;
    protected Vector2 touchEndPosition;
    protected float touchStartTime;
    protected float currentAngle;
    protected bool isTrackingSwipe = false;
    protected bool isPendingSwipeCheck = false;
    protected bool isGrabbed = false;
    protected bool isShown = false;

    protected Material baseMaterialBtnDown;

    protected virtual void Awake()
    {
        Initialize();
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

        baseMaterialBtnDown = buttonDown.GetComponent<MeshRenderer>().material;
        GetComponent<VRTK_InteractableObject>().ToggleHighlight(false);
    }

    void Update()
    {
        if (commandAnim.GetCurrentAnimatorStateInfo(0).IsName("UpBtnPush"))
        {
            commandAnim.SetBool("isUp", false);
        }

        if (commandAnim.GetCurrentAnimatorStateInfo(0).IsName("DownBtnPush"))
        {
            downBtnEffect.Play();
            levelGameManager.MotorMakeBegin();
            commandAnim.SetBool("isDown", false);
        }

        if (isGrabbed == true)
        {
            if (GetComponent<MeshRenderer>().material != baseMaterial)
            {
                GetComponent<MeshRenderer>().material = baseMaterial;
            }

            if (buttonDown.GetComponent<MeshRenderer>().material != outlinedMaterialBtnDown)
            {
                buttonDown.GetComponent<MeshRenderer>().material = outlinedMaterialBtnDown;
            }
            
        }

        if (isGrabbed == false)
        {
            buttonDown.GetComponent<MeshRenderer>().material = baseMaterialBtnDown;
        }
        
    }

    private void OnTriggerStay(Collider collider)
    {
        VRTK_InteractGrab grabbingController = (collider.gameObject.GetComponent<VRTK_InteractGrab>() ? collider.gameObject.GetComponent<VRTK_InteractGrab>() : collider.gameObject.GetComponentInParent<VRTK_InteractGrab>());
        Rigidbody rb = GetComponent<Rigidbody>();

        if (commandAnim.GetCurrentAnimatorStateInfo(0).IsName("EndDownPush"))
        {
            grabbingController.ForceRelease(false);
            Destroy(GetComponent<VRTK_InteractableObject>());

            GetComponent<MeshRenderer>().material = baseMaterial;

            transform.rotation = Quaternion.identity;
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.freezeRotation = true;
        }
    }

    protected virtual void OnDestroy()
    {
        VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void Initialize()
    {
        if (controllerEvents == null)
        {
            controllerEvents = GetComponentInParent<VRTK_ControllerEvents>();
        }
    }

    protected virtual void BindControllerEvents()
    {
        controllerEvents.TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPress);
        controllerEvents.TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouched);
        controllerEvents.TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadUntouched);
        controllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
    }

    protected virtual void UnbindControllerEvents()
    {
        controllerEvents.TouchpadPressed -= new ControllerInteractionEventHandler(DoTouchpadPress);
        controllerEvents.TouchpadTouchStart -= new ControllerInteractionEventHandler(DoTouchpadTouched);
        controllerEvents.TouchpadTouchEnd -= new ControllerInteractionEventHandler(DoTouchpadUntouched);
        controllerEvents.TouchpadAxisChanged -= new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
    }

    protected virtual void DoInteractableObjectIsGrabbed(object sender, InteractableObjectEventArgs e)
    {
        controllerEvents = e.interactingObject.GetComponentInParent<VRTK_ControllerEvents>();
        if (controllerEvents != null)
        {
            BindControllerEvents();
        }
        isGrabbed = true;
    }

    protected virtual void DoInteractableObjectIsUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = false;

        if (controllerEvents != null)
        {
            UnbindControllerEvents();
            controllerEvents = null;
        }
    }

    protected virtual void DoTouchpadPress(object sender, ControllerInteractionEventArgs e)
    {
        if (isGrabbed)
        {
            

            //Use it if you want a speculare touchpad press position// 

            var pressPosition = CalculateTouchpadPressPosition();

            
            switch (pressPosition)
            {
                case TouchpadPressPosition.Top:
                    UpBtnPushed();
                    break;

                case TouchpadPressPosition.Bottom:
                    DownBtnPushed(); 
                    break;

                /*case TouchpadPressPosition.Left:
                    
                    break;*/

                /*case TouchpadPressPosition.Right:
                    
                    break;*/
            }
        }
    }

    public void UpBtnPushed()
    {
        commandAnim.SetBool("isUp", true);
    }


    public void DownBtnPushed()
    {
        commandAnim.SetBool("isDown", true);
        
    }

    protected virtual void DoTouchpadTouched(object sender, ControllerInteractionEventArgs e)
    {
        touchStartPosition = new Vector2(e.touchpadAxis.x, e.touchpadAxis.y);
        touchStartTime = Time.time;
        isTrackingSwipe = true;
    }

    protected virtual void DoTouchpadUntouched(object sender, ControllerInteractionEventArgs e)
    {
        isTrackingSwipe = false;
        isPendingSwipeCheck = true;
    }

    protected virtual void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        ChangeAngle(CalculateAngle(e));

        if (isTrackingSwipe)
        {
            touchEndPosition = new Vector2(e.touchpadAxis.x, e.touchpadAxis.y);
        }
    }

    protected virtual void ChangeAngle(float angle, object sender = null)
    {
        currentAngle = angle;
    }


    protected virtual TouchpadPressPosition CalculateTouchpadPressPosition()
    {
        if (CheckAnglePosition(currentAngle, AngleTolerance, 0))
        {
            return TouchpadPressPosition.Top;
        }
        else if (CheckAnglePosition(currentAngle, AngleTolerance, 180))
        {
            return TouchpadPressPosition.Bottom;
        }
        else if (CheckAnglePosition(currentAngle, AngleTolerance, 270))
        {
            return TouchpadPressPosition.Left;
        }
        else if (CheckAnglePosition(currentAngle, AngleTolerance, 90))
        {
            return TouchpadPressPosition.Right;
        }

        return TouchpadPressPosition.None;
    }


    protected virtual float CalculateAngle(ControllerInteractionEventArgs e)
    {
        return e.touchpadAngle;
    }

    protected virtual float NormAngle(float currentDegree, float maxAngle = 360)
    {
        if (currentDegree < 0) currentDegree = currentDegree + maxAngle;
        return currentDegree % maxAngle;
    }

    protected virtual bool CheckAnglePosition(float currentDegree, float tolerance, float targetDegree)
    {
        float lowerBound = NormAngle(currentDegree - tolerance);
        float upperBound = NormAngle(currentDegree + tolerance);

        if (lowerBound > upperBound)
        {
            return targetDegree >= lowerBound || targetDegree <= upperBound;
        }
        return targetDegree >= lowerBound && targetDegree <= upperBound;
    }


}
