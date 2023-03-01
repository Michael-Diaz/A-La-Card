using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTracker : MonoBehaviour
{
    //private int pointTotal = 0;
    GameObject[] plates = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        GameObject plateParent = GameObject.Find("Plates");

        for (int i = 0; i < 3; i++)
        {
            plates[i] = plateParent.transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    public void endTurn()
    {
        for (int i = 0; i < 3; i++)
        {
            if ((plates[i].transform.GetChild(0).childCount != 0) && (plates[i].transform.GetChild(1).childCount != 0))
            {
                Debug.Log("Deleting plate #" + i);

                Destroy(plates[i].transform.GetChild(0).GetChild(0).gameObject);
                Destroy(plates[i].transform.GetChild(1).GetChild(0).gameObject);
            }
        }
    }
}
