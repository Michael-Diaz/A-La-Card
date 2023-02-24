using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cookbook : MonoBehaviour
{
    public GameObject recipePrefab;
    private GameObject canvasUI;
    private const float startRecipeHeight = 40.0f;
    private float startRecipeDist = 60.0f + Screen.width;

    enum Meats {Beef, Chicken, Fish, Pork};
    enum Veggies {Asparagus, Broccoli, Corn, Garlic, Mushrooms, Onions, Potatoes, Spinach, Tomatoes};
    enum Grains {Flour, Pasta, Rice};
    enum Fats {Butter, Lard, Oil};
    enum Dairy {Cheese, Cream, Milk, Sourcream, Yogurt};

    private const int totalTurns = 80;
    private const float difficultyConstant = 0.33f;

    // Start is called before the first frame update
    void Start()
    {
        canvasUI = GameObject.Find("Canvas");
    }

    public GameObject generateRecipe(int remainingTurns)
    {
        string recipeTitle = "Recipe Name Here";
        int recipeTimer = 0;

        float maxRange = (totalTurns - (remainingTurns + 1)) / (float) totalTurns;
        float difficultyMeter = Random.Range(0.0f, maxRange);

        if (difficultyMeter <= difficultyConstant)
        {
            /* meat/veg and grain */
            recipeTitle = "Easy Recipe #";
            recipeTimer = 3;
        }
        else if (difficultyMeter <= difficultyConstant * 2)
        {
            /* meat/veg and grain AND fat/dairy */
            recipeTitle = "Medium Recipe #";
            recipeTimer = 4;
        }
        else
        {
            /* meat AND veg and grain and fat/dairy */
            recipeTitle = "Hard Recipe #";
            recipeTimer = 5;
        }
        recipeTitle += difficultyMeter.ToString();

        GameObject recipe = Instantiate(recipePrefab, canvasUI.transform);
        Recipe recipeInfo = recipe.GetComponentInChildren<Recipe>();

        recipeInfo.title = recipeTitle;
        recipeInfo.timer = recipeTimer;

        return recipe;
    }

    public void recreateRecipe(GameObject originalRecipe, GameObject targetRecipe)
    {
        Recipe recipeInfoCopy = targetRecipe.GetComponentInChildren<Recipe>() ;
        Recipe recipeInfoOriginal = originalRecipe.GetComponent<Recipe>();

        recipeInfoCopy.title = recipeInfoOriginal.title;
        recipeInfoCopy.timer = recipeInfoOriginal.timer;
    }
}
