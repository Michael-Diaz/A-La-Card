using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipe : MonoBehaviour
{
    public string title;
    public string[] ingredients;
    public bool vegan;
    public int difficulty; // Ranges from 1 -> 3
    public int timer;

    void Update()
    {
        gameObject.transform.GetChild(0).GetComponent<Text>().text = title;
        gameObject.transform.GetChild(1).GetComponent<Text>().text = timer.ToString();

        GameObject ingredientList = gameObject.transform.GetChild(2).gameObject;
        if (difficulty == 3)
        {
            ingredientList.transform.GetChild(0).GetComponent<Text>().text = "1) " + ingredients[0] + "\n    [Carve -> Cook]";
            ingredientList.transform.GetChild(1).GetComponent<Text>().text = "2) " + ingredients[3] + "\n    [Chop -> Sear]";
            ingredientList.transform.GetChild(2).GetComponent<Text>().text = "3) " + ingredients[1] + "\n    [Boil]";
            ingredientList.transform.GetChild(3).GetComponent<Text>().text = "4) " + ingredients[2] + "\n    [Add to Pan/Pot]";
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (ingredients[i] != "N/a")
                {
                    string ingredientInstruction = "";

                    switch (i)
                    {
                        case 0:
                            ingredientInstruction = vegan ? "Chop -> Sear" : "Carve -> Cook";
                            break;
                        case 1:
                            ingredientInstruction = "Boil";
                            break;
                        case 2:
                            ingredientInstruction = "Add to Pan/Pot";
                            break;
                    }

                    ingredientList.transform.GetChild(i).GetComponent<Text>().text = (i + 1).ToString() + ") " + ingredients[i] + "\n    [" + ingredientInstruction + "]";
                }
                else
                    ingredientList.transform.GetChild(i).GetComponent<Text>().text = "";
            }
        }

    }

    public void timerCountdown()
    {
        if (timer == 1)
        {
            if (gameObject.name == "Recipe Card(Clone)")
            {
                GameObject.Find("\"Waiter\"").GetComponent<RecipeManager>().removeRecipe(gameObject.GetComponent<RectTransform>());

            }
            else
                Destroy(gameObject.transform.parent.gameObject);
        }
        else
            timer--;
    }
}
