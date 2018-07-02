using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class NewDescriptionObjScript : MonoBehaviour
{

    public enum TouchpadPressPosition
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }

    public GameObject descPanel;

    protected List<VRTK_PanelMenuItemController> allPanelMenuItemController;
    protected GameObject presentationPanel;

    private int currentPanelId;
    protected bool statutDesc;

    protected VRTK_ControllerEvents controllerEvents;
    protected VRTK_PanelMenuItemController currentPanelMenuItemController;
    protected GameObject interactableObject;

    // Swipe sensitivity / detection.
    protected const float AngleTolerance = 30f;
    protected const float SwipeMinDist = 0.2f;
    protected const float SwipeMinVelocity = 4.0f;

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

    protected virtual void Awake()
    {
        Initialize();
        VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
    }

    protected virtual void Start()
    {
        statutDesc = false;
        currentPanelId = 0;

        presentationPanel = descPanel.transform.parent.gameObject;

        allPanelMenuItemController = new List<VRTK_PanelMenuItemController>();

        for(int i = 0; i < descPanel.transform.GetChild(0).gameObject.transform.childCount; i++)
        {
            allPanelMenuItemController.Add(descPanel.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<VRTK_PanelMenuItemController>());
        }

        interactableObject = gameObject;
        if (interactableObject == null || interactableObject.GetComponent<VRTK_InteractableObject>() == null)
        {
            VRTK_Logger.Warn(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "ElevatorCommand", "VRTK_InteractableObject", "a parent"));
            return;
        }

        interactableObject.GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(DoInteractableObjectIsGrabbed);
        interactableObject.GetComponent<VRTK_InteractableObject>().InteractableObjectUngrabbed += new InteractableObjectEventHandler(DoInteractableObjectIsUngrabbed);
        
    }

    void Update()
    {
        if (isPendingSwipeCheck)
        {
            CalculateSwipeAction();
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
        controllerEvents = e.interactingObject.GetComponent<VRTK_ControllerEvents>();
        if (controllerEvents != null)
        {
            BindControllerEvents();
        }
        isGrabbed = true;

        statutDesc = true;

        for(int i = 0; i < presentationPanel.transform.childCount; i++)
        {
            if (presentationPanel.transform.GetChild(i).gameObject.activeSelf == true)
            {
                presentationPanel.transform.GetChild(i).gameObject.SetActive(false);
            } 
        }

        for(int i = 1; i < allPanelMenuItemController.Count; i++)
        {
            if (allPanelMenuItemController[i].gameObject.activeSelf == true)
            {
                allPanelMenuItemController[i].gameObject.SetActive(false);
            }
        }

        currentPanelMenuItemController = allPanelMenuItemController[0];
        descPanel.SetActive(true);
        currentPanelMenuItemController.gameObject.SetActive(true);
    }

    protected virtual void DoInteractableObjectIsUngrabbed(object sender, InteractableObjectEventArgs e)
    {
        isGrabbed = false;

        statutDesc = false;
        currentPanelId = 0;

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

            currentPanelId = 0;

            if (descPanel.activeSelf == true)
            {
                statutDesc = false;
            }
            else
            {
                statutDesc = true;
            }


            if (statutDesc == true)
            {
                for (int i = 1; i < allPanelMenuItemController.Count; i++)
                {
                    if (allPanelMenuItemController[i].gameObject.activeSelf == true)
                    {
                        allPanelMenuItemController[i].gameObject.SetActive(false);
                    }
                }

                for (int i = 0; i < presentationPanel.transform.childCount; i++)
                {
                    if (presentationPanel.transform.GetChild(i).gameObject.activeSelf == true)
                    {
                        presentationPanel.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                descPanel.SetActive(true);

                currentPanelMenuItemController = allPanelMenuItemController[0];
                currentPanelMenuItemController.gameObject.SetActive(true);
            }
            else
            {
                currentPanelMenuItemController.gameObject.SetActive(false);

                for (int i = 0; i < allPanelMenuItemController.Count; i++)
                {
                    if (allPanelMenuItemController[i].gameObject.activeSelf == true)
                    {
                        allPanelMenuItemController[i].gameObject.SetActive(false);
                    }
                }

                for (int i = 1; i < presentationPanel.transform.childCount; i++)
                {
                    if (presentationPanel.transform.GetChild(i).gameObject.activeSelf == true)
                    {
                        presentationPanel.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
                presentationPanel.transform.GetChild(0).gameObject.SetActive(true);
            }




            /* switch (pressPosition)
             {
                 case TouchpadPressPosition.Top:
                     UpBtnPushed();
                     break;

                 case TouchpadPressPosition.Bottom:
                     DownBtnPushed();
                     break;

                     case TouchpadPressPosition.Left:

                         break;

                     case TouchpadPressPosition.Right:

                         break;
             }*/
        }
    }

    public void UpBtnPushed()
    {
       
    }


    public void DownBtnPushed()
    {
     
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

    protected virtual void ChangeAngle(float angle, object sender = null)
    {
        currentAngle = angle;
    }

    protected virtual void CalculateSwipeAction()
    {
        isPendingSwipeCheck = false;

        float deltaTime = Time.time - touchStartTime;
        Vector2 swipeVector = touchEndPosition - touchStartPosition;
        float velocity = swipeVector.magnitude / deltaTime;

        if ((velocity > SwipeMinVelocity) && (swipeVector.magnitude > SwipeMinDist))
        {
            swipeVector.Normalize();
            float angleOfSwipe = Vector2.Dot(swipeVector, xAxis);
            angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;

            // Left / right
            if (angleOfSwipe < AngleTolerance)
            {
                OnSwipeRight();
            }
            else if ((180.0f - angleOfSwipe) < AngleTolerance)
            {
                OnSwipeLeft();
            }
            else
            {
                // Top / bottom
                angleOfSwipe = Vector2.Dot(swipeVector, yAxis);
                angleOfSwipe = Mathf.Acos(angleOfSwipe) * Mathf.Rad2Deg;
                if (angleOfSwipe < AngleTolerance)
                {
                    OnSwipeTop();
                }
                else if ((180.0f - angleOfSwipe) < AngleTolerance)
                {
                    OnSwipeBottom();
                }
            }
        }
    }

    protected virtual void OnSwipeLeft()
    {
        if (currentPanelMenuItemController != null)
        {
            //currentPanelMenuItemController.SwipeLeft(interactableObject);

            if (currentPanelId - 1 >= 0)
            {
                currentPanelId -= 1;
                currentPanelMenuItemController.gameObject.SetActive(false);
                currentPanelMenuItemController = allPanelMenuItemController[currentPanelId];
                currentPanelMenuItemController.gameObject.SetActive(true);
            }
        }
    }

    protected virtual void OnSwipeRight()
    {
        if (currentPanelMenuItemController != null)
        {
            //currentPanelMenuItemController.SwipeRight(interactableObject);

            if (currentPanelId + 1 < allPanelMenuItemController.Count)
            {
                currentPanelId += 1;
                currentPanelMenuItemController.gameObject.SetActive(false);
                currentPanelMenuItemController = allPanelMenuItemController[currentPanelId];
                currentPanelMenuItemController.gameObject.SetActive(true);
            }
        }
    }

    protected virtual void OnSwipeTop()
    {
        if (currentPanelMenuItemController != null)
        {
            currentPanelMenuItemController.SwipeTop(interactableObject);
        }
    }

    protected virtual void OnSwipeBottom()
    {
        if (currentPanelMenuItemController != null)
        {
            currentPanelMenuItemController.SwipeBottom(interactableObject);
        }
    }


}
