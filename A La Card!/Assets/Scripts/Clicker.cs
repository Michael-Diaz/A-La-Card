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
    // Start is called before the first frame update
    void Start()
    {
        viewPoint = Camera.main;
        maskUI = LayerMask.NameToLayer("UI");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !IsPointerOverUIElement())
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x /= (float) Screen.width;
            mousePos.y /= (float) Screen.height;

            Ray ray = viewPoint.ViewportPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(targetCheck, hit.point, Quaternion.identity);
                /* send a signal to the object to move while the mouse is down via a mouseTrack() function */
            }
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
