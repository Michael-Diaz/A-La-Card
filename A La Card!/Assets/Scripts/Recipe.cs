using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    public string title;
    public int timer;

    void Update()
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().text = title;
        gameObject.transform.GetChild(1).GetComponent<Text>().text = timer.ToString();
    }

    public void timerCountdown()
    {
        if (timer == 1)
            Destroy(gameObject);
        else
            timer--;
    }
}
