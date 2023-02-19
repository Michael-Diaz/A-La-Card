using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    private Camera viewPoint;
    public Object targetCheck;

    // Start is called before the first frame update
    void Start()
    {
        viewPoint = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.x /= (float) Screen.width;
            mousePos.y /= (float) Screen.height;

            Ray ray = viewPoint.ViewportPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Instantiate(targetCheck, hit.point, Quaternion.identity);
                Debug.Log("Hit; " + mousePos);
            }
            else
            {
                Debug.Log("Miss; " + mousePos);
            }
        }
    }
}
