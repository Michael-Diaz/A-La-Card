using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMovement_Ingredient : MonoBehaviour
{
    private GameObject ingredientUI;
    private GameObject refCard;
    private int refCardID;
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
                    GameObject.Find("\"Chef\"").GetComponent<Clicker>().heldObject = null;

                    gameObject.transform.SetParent(slotPos.transform);
                    transform.localPosition = new Vector3(0.36f, 0.0f, -0.36f);
                    transform.localRotation = Quaternion.identity;
                    transform.localScale = new Vector3(0.03f, 1.0f, 0.06f);
                }

            }
            else
            {
                refCard.SetActive(true);
                refCard.GetComponent<IngredientControl>().resetCardPos();

                refCard.GetComponent<IngredientControl>().reassign = false;

                Destroy(gameObject);
            }
        }

        if (followingMouse)
        {
            gameObject.transform.parent = null;

            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1));
            transform.rotation = Quaternion.Euler(-90.0f * (Input.mousePosition.y / Screen.height), 0.0f, 0.0f);
        }
    }

    public void assignRef(int cardID)
    {
        ingredientUI = GameObject.Find("Ingredient Bar");

        refCardID = cardID;
        string cardName = "Card_" + refCardID.ToString();

        for (int i = 0; i < 7; i++)
        {
            if (ingredientUI.transform.GetChild(i).gameObject.name == cardName)
            {
                refCard = ingredientUI.transform.GetChild(i).gameObject;
            }
        }

        Transform ingredientUIContainer = gameObject.transform.GetChild(0);
        ingredientUIContainer.GetChild(0).gameObject.GetComponent<Image>().color = refCard.GetComponent<Image>().color;
        ingredientUIContainer.GetChild(1).gameObject.GetComponent<Image>().color = refCard.transform.GetChild(0).gameObject.GetComponent<Image>().color;
        ingredientUIContainer.GetChild(2).gameObject.GetComponent<Text>().text = refCard.transform.GetChild(1).gameObject.GetComponent<Text>().text;

        refCard.SetActive(false);
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
