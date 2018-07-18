using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollisionChildsScript : MonoBehaviour {

    public GameObject[] childs = new GameObject[2];

    void Start()
    {
        Physics.IgnoreCollision(this.GetComponent<Collider>(), childs[0].GetComponent<Collider>(), true);
        Physics.IgnoreCollision(this.GetComponent<Collider>(), childs[1].GetComponent<Collider>(), true);
    }
}
