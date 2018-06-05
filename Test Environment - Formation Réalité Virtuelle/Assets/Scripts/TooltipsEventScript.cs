using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

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
