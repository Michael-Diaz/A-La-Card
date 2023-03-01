using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardMovement_Ingredient : MonoBehaviour
{
    public bool cooked;
    public bool[] combinations = new bool[5];
    public bool tryingCombo;
    public string ingDesc;
    public string ingType;
    private Dictionary<float, string> gColorToIngredient = new Dictionary<float, string>()
    {
        {0.0f, "meats"},
        {0.6f, "veggies"},
        {0.3f, "grains"},
        {1.0f, "fats"},
        {0.8f, "dairy"}
    };

    private GameObject ingredientUI;
    private GameObject refCard;
    private int refCardID;
    private bool followingMouse;

    private bool refLess;
    private bool overSlot;
    private bool jumpBack;
    private bool destroyIng;
    private GameObject slotPos;
    private GameObject slotPosLast;

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
                    if (slotPos.transform.parent.name == "Drain_1")
                        destroyIng = true;

                    if (tryingCombo)
                        tryCombo(slotPos.transform.GetChild(0).gameObject);

                    tryingCombo = false;

                    GameObject.Find("\"Chef\"").GetComponent<Clicker>().heldObject = null;

                    gameObject.transform.SetParent(slotPos.transform);
                    transform.localPosition = new Vector3(0.36f, 0.0f, -0.36f);
                    transform.localRotation = Quaternion.identity;
                    transform.localScale = new Vector3(0.03f, 1.0f, 0.06f);

                    if (jumpBack)
                    {
                        GameObject.Find("\"Chef\"").GetComponent<Clicker>().heldObject = null;

                        gameObject.transform.SetParent(slotPosLast.transform);
                        transform.localPosition = new Vector3(0.36f, 0.0f, -0.36f);
                        transform.localRotation = Quaternion.identity;
                        transform.localScale = new Vector3(0.03f, 1.0f, 0.06f);
                    }
                    
                    if (refLess && !jumpBack)
                        slotPosLast = slotPos;

                    jumpBack = false;
                }
            }
            else
            {
                if (!refLess)
                {
                    refCard.SetActive(true);
                    refCard.GetComponent<IngredientControl>().resetCardPos();

                    refCard.GetComponent<IngredientControl>().reassign = false;

                    Destroy(gameObject);
                }
                else
                {
                    GameObject.Find("\"Chef\"").GetComponent<Clicker>().heldObject = null;

                    gameObject.transform.SetParent(slotPosLast.transform);
                    transform.localPosition = new Vector3(0.36f, 0.0f, -0.36f);
                    transform.localRotation = Quaternion.identity;
                    transform.localScale = new Vector3(0.03f, 1.0f, 0.06f);
                }
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
        cooked = false;
        refLess = false;

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
        ingDesc = ingredientUIContainer.GetChild(2).gameObject.GetComponent<Text>().text = refCard.transform.GetChild(1).gameObject.GetComponent<Text>().text;

        ingType = gColorToIngredient[ingredientUIContainer.GetChild(0).gameObject.GetComponent<Image>().color.g];

        refCard.SetActive(false);
    }

    public void timerCountdown()
    {
        refCard.SetActive(true);

        if (destroyIng)
            Destroy(gameObject);

        refLess = true;
        slotPosLast = slotPos;

        string slotLocation = transform.parent.parent.parent.name;

        switch (ingType)
        {
            case "meats":
                if (ingDesc.Substring(0, 3) == "Raw" && slotLocation == "Cutting Board")
                    ingDesc = ingDesc.Replace("Raw", "Carved");
                else if (ingDesc.Substring(0, 6) == "Carved" && slotLocation == "Stove")
                {
                    ingDesc = ingDesc.Replace("Carved", "Seared");
                    cooked = true;
                    combinations[0] = true;
                }
                break;
            case "veggies":
                if (ingDesc.Substring(0, 3) == "Raw" && slotLocation == "Cutting Board")
                    ingDesc = ingDesc.Replace("Raw", "Chopped");
                else if (ingDesc.Substring(0, 7) == "Chopped" && slotLocation == "Stove")
                {
                    ingDesc = ingDesc.Replace("Chopped", "Roasted");
                    cooked = true;
                    combinations[1] = true;
                }
                break;
            case "grains":
                if (ingDesc.Substring(0, 3) == "Dry" && slotLocation == "Stove")
                {
                    ingDesc = ingDesc.Replace("Dry", "Boiled");
                    cooked = true;
                    combinations[2] = true;
                }
                break;
            case "fats":
                if (ingDesc.Substring(0, 4) == "Cold" && slotLocation == "Mixing Bowls")
                {
                    ingDesc = ingDesc.Replace("Cold", "Warm");
                    cooked = true;
                    combinations[2] = true;
                }
                break;
            case "dairy":
                if (ingDesc.Substring(0, 4) == "Cold" && slotLocation == "Mixing Bowls")
                {
                    ingDesc = ingDesc.Replace("Cold", "Whisked");
                    cooked = true;
                    combinations[2] = true;
                }
                break;
        }

        transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text = ingDesc;
        if (cooked)
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
            transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }
    }

    public void tryCombo(GameObject target)
    {
        CardMovement_Ingredient targetSpecs = target.GetComponent<CardMovement_Ingredient>();

        int ingIndex = -1;
        switch (ingType)
        {
            case "meats":
                ingIndex = 0;
                break;
            case "veggies":
                ingIndex = 1;
                break;
            case "grains":
                ingIndex = 2;
                break;
            case "fats":
                ingIndex = 3;
                break;
            case "dairy":
                ingIndex = 4;
                break;
        }

        if ((combinations[ingIndex] != targetSpecs.combinations[ingIndex]) && targetSpecs.cooked)
        {
            targetSpecs.ingDesc += ", " + ingDesc;
            targetSpecs.combinations[ingIndex] = true;

            Destroy(gameObject);
        }

        jumpBack = true;
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
