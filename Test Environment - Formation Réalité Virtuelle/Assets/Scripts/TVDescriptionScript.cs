using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VRTK;

public class TVDescriptionScript : MonoBehaviour
{

    public enum TouchpadPressPosition
    {
        None,
        Top,
        Bottom,
        Left,
        Right
    }

    public GameObject presentationPanel;
    public AudioSource swapEffectSource;
    public AudioClip swapEffectClip;
    [Header("Controllers Settings")]
    public VRTK_ControllerEvents rightControllerEvents;
    public VRTK_ControllerEvents leftControllerEvents;

    protected List<VRTK_PanelMenuItemController> allPanelMenuItemController;
    protected GameObject currentDescObjPanel;

    private int currentPanelId;

    
    protected VRTK_PanelMenuItemController currentPanelMenuItemController;

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

    protected int objectsIsGrabbed = 0;
    protected GameObject previousDescObjPanel;


    protected virtual void Start()
    {
        currentPanelId = 0;

        BindControllerEvents();
    }

    void Update()
    {
        if (isPendingSwipeCheck)
        {
            CalculateSwipeAction();
        }
    }

    protected virtual void BindControllerEvents()
    {
        rightControllerEvents.TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPress);
        rightControllerEvents.TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouched);
        rightControllerEvents.TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadUntouched);
        rightControllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

        leftControllerEvents.TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPress);
        leftControllerEvents.TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouched);
        leftControllerEvents.TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadUntouched);
        leftControllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
    }


    public void ActiveObjDescPanel(GameObject descObjPanel)
    {
        objectsIsGrabbed += 1;
        if(objectsIsGrabbed == 2)
        {
            previousDescObjPanel = currentDescObjPanel;
        }

        currentDescObjPanel = descObjPanel;

        allPanelMenuItemController = new List<VRTK_PanelMenuItemController>();

        for (int i = 0; i < currentDescObjPanel.transform.GetChild(0).gameObject.transform.childCount; i++)
        {
            allPanelMenuItemController.Add(currentDescObjPanel.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<VRTK_PanelMenuItemController>());
        }

        for (int i = 0; i < presentationPanel.transform.childCount; i++)
        {
            if (presentationPanel.transform.GetChild(i).gameObject.activeSelf == true)
            {
                presentationPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 1; i < allPanelMenuItemController.Count; i++)
        {
            if (allPanelMenuItemController[i].gameObject.activeSelf == true)
            {
                allPanelMenuItemController[i].gameObject.SetActive(false);
            }
        }


        currentPanelMenuItemController = allPanelMenuItemController[0];
        currentDescObjPanel.SetActive(true);
        currentPanelMenuItemController.gameObject.SetActive(true);
    }

    public virtual void DesactiveObjDescPanel(GameObject descObjPanel)
    {
        currentPanelId = 0;
        objectsIsGrabbed -= 1;

        if(objectsIsGrabbed ==  1)
        {
            if(descObjPanel == currentDescObjPanel)
            {
                currentDescObjPanel = previousDescObjPanel;
            } 

            allPanelMenuItemController = new List<VRTK_PanelMenuItemController>();

            for (int i = 0; i < currentDescObjPanel.transform.GetChild(0).gameObject.transform.childCount; i++)
            {
                allPanelMenuItemController.Add(currentDescObjPanel.transform.GetChild(0).gameObject.transform.GetChild(i).gameObject.GetComponent<VRTK_PanelMenuItemController>());
            }

            for (int i = 0; i < presentationPanel.transform.childCount; i++)
            {
                if (presentationPanel.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    presentationPanel.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            for (int i = 1; i < allPanelMenuItemController.Count; i++)
            {
                if (allPanelMenuItemController[i].gameObject.activeSelf == true)
                {
                    allPanelMenuItemController[i].gameObject.SetActive(false);
                }
            }


            currentPanelMenuItemController = allPanelMenuItemController[0];
            currentDescObjPanel.SetActive(true);
            currentPanelMenuItemController.gameObject.SetActive(true);
        }
        
    }

    protected virtual void DoTouchpadPress(object sender, ControllerInteractionEventArgs e)
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
            if (currentPanelId - 1 >= 0)
            {
                currentPanelId -= 1;
                swapEffectSource.PlayOneShot(swapEffectClip);
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
            if (currentPanelId + 1 < allPanelMenuItemController.Count)
            {
                currentPanelId += 1;
                swapEffectSource.PlayOneShot(swapEffectClip);
                currentPanelMenuItemController.gameObject.SetActive(false);
                currentPanelMenuItemController = allPanelMenuItemController[currentPanelId];
                currentPanelMenuItemController.gameObject.SetActive(true);
            }
        }
    }

    protected virtual void OnSwipeTop()
    {
       
    }

    protected virtual void OnSwipeBottom()
    {
        
    }


}
