using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Clicker : MonoBehaviour
{
    private Camera viewPoint;
    public Object targetCheck;
    public GameObject heldObject;

    int maskUI;
    LayerMask maskDynamicObj;
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsPointerOverUIElement())
        {
            if (Physics.Raycast(ray, out hit))
            {
                // Instantiate(targetCheck, hit.point, Quaternion.identity);
                /* send a signal to the object to move while the mouse is down via a mouseTrack() function */
                if (hit.collider.gameObject.tag == "Physical")
                {
                    heldObject = hit.collider.gameObject;
                    heldObject.GetComponent<CardMovement_Recipe>().trackMouse();
                }
            }
        }

        if (heldObject != null)
        {
            Debug.Log("Holding object");
            bool checkForSlot = false;
            GameObject slotPos = null;

            if (Physics.Raycast(ray, out hit, 1000.0f, ~maskDynamicObj))
            {
                bool objectInHand = false;

                if (hit.collider.gameObject.tag == "Slot" && (hit.collider.gameObject.transform.GetChild(0).gameObject.transform.childCount == 0))
                {
                    checkForSlot = true;
                    slotPos = hit.collider.gameObject.transform.GetChild(0).gameObject;

                    objectInHand = true;
                }
                else
                {
                    checkForSlot = false;

                    objectInHand = false;
                }

                if (heldObject.name == "3D Recipe Card(Clone)")
                    GameObject.Find("\"Waiter\"").GetComponent<RecipeManager>().offScreenContainerNull = objectInHand;
            }

            heldObject.GetComponent<CardMovement_Recipe>().slotUpdate(checkForSlot, slotPos);
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
