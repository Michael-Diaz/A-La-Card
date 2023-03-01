using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Clicker : MonoBehaviour
{
    private Camera viewPoint;
    int maskUI;
    LayerMask maskDynamicObj;

    public GameObject heldObject;

    // Start is called before the first frame update
    void Start()
    {
        viewPoint = Camera.main;
        maskUI = LayerMask.NameToLayer("UI");
        maskDynamicObj = LayerMask.GetMask("Held");

        heldObject = null;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.x /= (float) Screen.width;
        mousePos.y /= (float) Screen.height;

        Ray ray = viewPoint.ViewportPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
        RaycastHit hit;

        // For picking up phsyical elements
        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsPointerOverUIElement())
        {
            if (Physics.Raycast(ray, out hit))
            {
                /* send a signal to the object to move while the mouse is down via a mouseTrack() function */
                if (hit.collider.gameObject.tag == "Physical")
                {
                    heldObject = hit.collider.gameObject;
                    if (heldObject.GetComponent<CardMovement_Recipe>() != null)
                    {
                        heldObject.GetComponent<CardMovement_Recipe>().trackMouse();
                        GameObject.Find("\"Waiter\"").GetComponent<RecipeManager>().HoldPhysicalCard(heldObject);
                    }
                    else if (heldObject.GetComponent<CardMovement_Ingredient>() != null)
                        heldObject.GetComponent<CardMovement_Ingredient>().trackMouse();

                }
                else if (hit.collider.gameObject.tag == "Bell")
                {
                    Text bellTimer = hit.collider.gameObject.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
                    bellTimer.text = Mathf.Max((int.Parse(bellTimer.text)) - 1, 0).ToString();

                    GameObject[] allRecipes = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(obj => obj.name.Contains("Recipe Card(Clone)")).ToArray();
                    foreach (GameObject rec in allRecipes)
                    {
                        rec.GetComponentInChildren<Recipe>().timerCountdown();
                    }

                    GameObject[] allIngredients = GameObject.FindObjectsOfType(typeof(GameObject)).Select(g => g as GameObject).Where(obj => obj.name.Contains("3D Ingredient Card(Clone)")).ToArray();
                    foreach (GameObject ing in allIngredients)
                    {
                        ing.GetComponentInChildren<CardMovement_Ingredient>().timerCountdown();
                    }

                    GameObject.Find("\"Runner\"").GetComponent<PointTracker>().endTurn();
                    GameObject.Find("\"Waiter\"").GetComponent<RecipeManager>().endTurn();
                    GameObject.Find("\"Prep Cook\"").GetComponent<IngredientManager>().endTurn();
                }
            }
        }

        if (heldObject != null)
        {
            bool checkForSlot = false;
            GameObject slotPos = null;

            if (Physics.Raycast(ray, out hit, 1000.0f, ~maskDynamicObj))
            {
                bool objectInHand = false;

                if (((hit.collider.gameObject.tag == "Slot All" && heldObject.name == "3D Recipe Card(Clone)") 
                    || ((hit.collider.gameObject.tag == "Slot All" || hit.collider.gameObject.tag == "Slot Ing") && heldObject.name == "3D Ingredient Card(Clone)")))
                {
                    if (heldObject.name == "3D Ingredient Card(Clone)")
                    {
                        if (hit.collider.gameObject.transform.GetChild(0).gameObject.transform.childCount == 0)
                        {
                            slotPos = hit.collider.gameObject.transform.GetChild(0).gameObject;

                            checkForSlot = true;
                            objectInHand = true;

                            heldObject.GetComponent<CardMovement_Ingredient>().tryingCombo = false;
                        }
                        else
                        {
                            if (heldObject.GetComponent<CardMovement_Ingredient>().cooked)
                            {
                                slotPos = hit.collider.gameObject.transform.GetChild(0).gameObject;

                                checkForSlot = true;
                                objectInHand = true;

                                heldObject.GetComponent<CardMovement_Ingredient>().tryingCombo = true;
                            }
                            else
                            {
                                checkForSlot = false;
                                objectInHand = false;
                            }
                        }
                    }
                    else if (heldObject.name == "3D Recipe Card(Clone)" && hit.collider.gameObject.transform.GetChild(1).gameObject.transform.childCount == 0)
                    {
                        slotPos = hit.collider.gameObject.transform.GetChild(1).gameObject;

                        checkForSlot = true;
                        objectInHand = true;
                    }
                    else
                    {
                        checkForSlot = false;
                        objectInHand = false;
                    }
                }
                else
                {
                    checkForSlot = false;
                    objectInHand = false;
                }

                if (heldObject.name == "3D Recipe Card(Clone)")
                    GameObject.Find("\"Waiter\"").GetComponent<RecipeManager>().offScreenContainerNull = objectInHand;
            }
            
            if (heldObject.GetComponent<CardMovement_Recipe>() != null)
                heldObject.GetComponent<CardMovement_Recipe>().slotUpdate(checkForSlot, slotPos);
            else if (heldObject.GetComponent<CardMovement_Ingredient>() != null)
                heldObject.GetComponent<CardMovement_Ingredient>().slotUpdate(checkForSlot, slotPos);
        }
    }

    public bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            GameObject targetUIElem = eventSystemRaysastResults[index].gameObject;
            if (targetUIElem.layer == maskUI)
                return true;
        }
        return false;
    }

    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);

        return raysastResults;
    }
}
