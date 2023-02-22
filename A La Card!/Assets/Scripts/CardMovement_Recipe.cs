using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement_Recipe : MonoBehaviour
{
    private bool followingMouse;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && followingMouse)
        {
            followingMouse = false;
            /* Alter later so if raycast shows a "slot" to not destroy and place down*/
            Destroy(gameObject);
        }

        if (followingMouse)
        {
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1));
            transform.rotation = Quaternion.Euler(-90.0f * (Input.mousePosition.y / Screen.height), 0.0f, 0.0f);
        }
    }

    public void trackMouse()
    {
        followingMouse = true;
    }
}
