using UnityEngine;
using VRTK;
using VRTK.Highlighters;

/// <summary>
/// Titre : Controller Button Highlight Help
/// Auteur : GOISLOT Renaud
/// Description :
/// 
///     Montrer à l'utilisateur les interactions possibles avec un objet avec un système de Highlight
/// 
/// Effets : 
/// 
///     - Highlight du bouton trigger (bouton de grab) lorsque l'on touche un objet. 
///     - Afficher un descriptif des actions possibles avec un objet lorsque l'on touche celui-ci.
///     - Affichage d'un système de tooltip (affichages de pannels expliquant chaques touches) pendant une periode donner au début du lancement du script
///     
/// </summary>

public class ControllerBtnHighlightHelp : MonoBehaviour
{
    //Si bool = true , on active les tooltips seulement au début du script
    public bool tooltipsOnlyOneTime;
    //Si bool = true , on n'active aucun tooltips
    public bool noTooltipsMod;

    private VRTK_ControllerTooltips tooltips;
    private VRTK_ControllerHighlighter highligher;
    private VRTK_ControllerEvents events;
    private Color highlightColor = Color.yellow;
    private float highlightTimer = 0.5f;
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
        highlighted = false;
        HelpToolTips = true;

        //Si noTooltipsMod = true, on supprime l'enfant correspondant aux tooltips
        if (noTooltipsMod == true)
        {
            if(tooltips.gameObject != null)
            {
                Destroy(tooltips.gameObject);
            }
            
        }

        events.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        events.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);



    }


    //Action si Trigger Pressed
    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        //Si highlight pas activé, on l'active sur le bouton correpondant et on active son tooltip
        if (highlighted == false)
        {
            //On vérifie que les toolsTip sont activés
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

        //Si highlight pas désactivé, on le désactive sur le bouton correpondant et on desactive son tooltip
        else if (highlighted == true)
        {
            //On vérifie que les toolsTip sont activés
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

    //Action si le bouton trigger est relaché
    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        //desactivation des highlights et tooltips si les highlight sont activés
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

    //Action si la manette entre en colision avec un objet
    private void OnTriggerEnter(Collider collider)
    {
        OnTriggerStay(collider);
    }

    //Action tant que la manette est en colision avec un objet
    private void OnTriggerStay(Collider collider)
    {
        //On vérifie si l'objet en colision est un objet avec lequel on peut intéragir
        if (collider.gameObject.GetComponent<VRTK_InteractableObject>() != null)
        {
            //On vérifie si l'objet est grabbable (prennable) et si il n'y a pas de highlight déjà activé
            if (collider.gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable == true && highlighted == false)
            {
                highligher.HighlightElement(SDK_BaseController.ControllerElements.Trigger, highlightColor, highlightTimer);
                highligher.HighlightElement(SDK_BaseController.ControllerElements.Trigger, highlightColor, highlightTimer);
                VRTK_ObjectAppearance.SetOpacity(VRTK_DeviceFinder.GetModelAliasController(events.gameObject), dimOpacity);

                highlighted = true;
            }
        }

    }

    //Action si la manette n'est plus en colision avec un objet
    private void OnTriggerExit(Collider collider)
    {
        //On vérifie si l'objet en colision est un objet avec lequel on peut intéragir
        if (collider.gameObject.GetComponent<VRTK_InteractableObject>() != null)
        {
            //On vérifie si l'objet est grabbable (prennable) 
            if (collider.gameObject.GetComponent<VRTK_InteractableObject>().isGrabbable == true)
            {

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