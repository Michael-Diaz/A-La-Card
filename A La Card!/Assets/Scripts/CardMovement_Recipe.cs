using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement_Recipe : MonoBehaviour
{
    private bool followingMouse;
    private bool overSlot;
    private GameObject slotPos;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && followingMouse)
        {
            followingMouse = false;

            if (overSlot)
            {
                if (slotPos != null)
                {
                    gameObject.transform.SetParent(slotPos.transform);
                    transform.localPosition = new Vector3(0.36f, 0.0f, -0.36f);
                    transform.localRotation = Quaternion.identity;
                    transform.localScale = new Vector3(0.03f, 1.0f, 0.06f);
                }

            }
            else
                Destroy(gameObject);
        }

        if (followingMouse)
        {
            gameObject.transform.parent = null;

            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1));
            transform.rotation = Quaternion.Euler(-90.0f * (Input.mousePosition.y / Screen.height), 0.0f, 0.0f);
        }
    }

    public void trackMouse()
    {
        followingMouse = true;
    }

    public void slotUpdate(bool pointerUpdate, GameObject snapPosition)
    {
        overSlot = pointerUpdate;
        slotPos = snapPosition;
    }
}
