using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotorAnimController : MonoBehaviour {

    [Header("Motor Animation Settings")]

    public Animator motorAnim;

    public GameObject MotorPresentation;
    public GameObject MotorAssemblage;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        /* DEBUG Activation de l'animation de la présentation des pièces du moteur
        if (Input.GetKeyDown("space"))
        {
            motorAnim.SetBool("isBeginPresentation", true);
        }*/

        if (motorAnim.GetCurrentAnimatorStateInfo(0).IsName("motor_end_pres"))
        {
            motorAnim.SetBool("isBeginPresentation", false);
        }

        /* DEBUG Activation de l'animation du placement des pièces du moteur avant assemblage
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            motorAnim.SetBool("isBeginPuzzle", true);
        }*/

        //Destruction du gameObject à la fin de l'animation du placement des pièces 
        if (motorAnim.GetCurrentAnimatorStateInfo(0).IsName("motor_end_placement"))
        {
            Destroy(MotorPresentation);
            MotorAssemblage.SetActive(true);
        }


    }

    //Activation de l'animation de la présentation des pièces du moteur
    public void MotorPresentationBegin()
    {
        motorAnim.SetBool("isBeginPresentation", true);
    }

    //Activation de l'animation du placement des pièces du moteur avant assemblage
    public void MotorMakeBegin()
    {
        motorAnim.SetBool("isBeginPlacement", true);
    }
}
