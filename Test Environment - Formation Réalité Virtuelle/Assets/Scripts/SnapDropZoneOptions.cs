using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapDropZoneOptions : MonoBehaviour {

    public bool colliderIsTriggerOption;

    public bool initialAppearanceSnapDrop;

    private GameObject childObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(colliderIsTriggerOption == true)
        {
            if (transform.childCount == 2)
            {
                childObj = this.transform.GetChild(1).gameObject;
                ColliderObjectIsTriggerOn();
            }
            else 
            {
                ColliderObjectIsTriggerOff();
            }
        }

        if(initialAppearanceSnapDrop == true)
        {
            if (transform.childCount == 2)
            {
                childObj = this.transform.GetChild(1).gameObject;
                InitAppearanceObjIsTriggerOn();
            }
            /*else
            {
                InitAppearanceObjIsTriggerOff();
            }
            */
        }
       
    }
    
    //To desable/enable triggerCollider of a child object in a SnapDropZone
    void ColliderObjectIsTriggerOn()
    {
        childObj.GetComponent<MeshCollider>().isTrigger = true;
    }

    void ColliderObjectIsTriggerOff()
    {
        childObj.GetComponent<MeshCollider>().isTrigger = false;
    }


    //Obj Snap take initial appearance of the SnapDropZone
    void InitAppearanceObjIsTriggerOn()
    {
        if(childObj.GetComponent<MeshRenderer>())
        {
            childObj.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            Renderer[] rs = childObj.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
                r.enabled = false;

        }

        childObj.GetComponent<Collider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    /*void InitAppearanceObjIsTriggerOff()
    {
        if (childObj.GetComponent<MeshRenderer>())
        {
            childObj.GetComponent<MeshRenderer>().enabled = true;
        }
        else
        {
            Renderer[] rs = childObj.GetComponentsInChildren<Renderer>();
            foreach (Renderer r in rs)
                r.enabled = true;

        }

        this.GetComponent<MeshRenderer>().enabled = false;
    }
    */
}

