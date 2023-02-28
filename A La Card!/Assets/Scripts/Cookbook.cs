using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Cookbook : MonoBehaviour
{
    public GameObject recipePrefab;
    private GameObject canvasUI;
    private const float startRecipeHeight = 40.0f;
    private float startRecipeDist = 60.0f + Screen.width;

    private string[] meats = {"Beef", "Chicken", "Fish", "Pork"};
    private string[] veggies = {"Asparagus", "Broccoli", "Corn", "Garlic", "Mushrooms", "Onions", "Potatoes", "Spinach", "Tomatoes"};
    private string[] grains = {"Flour", "Pasta", "Rice"};
    private string[] fats = {"Butter", "Lard", "Oil"};
    private string[] dairy = {"Cheese", "Cream", "Milk", "Sourcream", "Yogurt"};
    private Color[] baseColors = {new Color(0.75f, 0.0f, 0.0f, 1.0f), 
                                  new Color(0.0f, 0.6f, 0.0f, 1.0f), 
                                  new Color(0.6f, 0.3f, 0.0f, 1.0f), 
                                  new Color(1.0f, 1.0f, 0.1f, 1.0f), 
                                  new Color(0.6f, 0.8f, 1.0f, 1.0f)};

    private bool veganDish;

    private const int totalTurns = 80;
    private const float difficultyConstant = 0.33f;

    // Start is called before the first frame update
    void Start()
    {
        canvasUI = GameObject.Find("Canvas");

        veganDish = false;
    }

    private string pickIngredient(string portion)
    {
        int[] ranges = new int[2];
        string option1 = "";
        string option2 = "";

        if (portion == "ENTREE")
        {
            ranges[0] = 13;
            ranges[1] = 3;
            option1 = meats[Random.Range(0, 4)];
            option2 = veggies[Random.Range(0, 9)];
        }
        else if (portion == "MEDIUM")
        {
            ranges[0] = 8;
            ranges[1] = 2;
            option1 = fats[Random.Range(0, 3)];
            option2 = dairy[Random.Range(0, 5)];
        }

        string choice = Random.Range(0, ranges[0]) > ranges[1] ? option1 : option2;
        if (veggies.Contains(choice))
            veganDish = true;

        return choice;
    }

    public GameObject generateRecipe(int remainingTurns)
    {
        string recipeTitle = "Recipe Name Here";
        int recipeTimer = 0;
        string[] repiceIngredients;

        float maxRange = (totalTurns - (remainingTurns + 1)) / (float) totalTurns;
        float difficultyMeter = Random.Range(0.0f, maxRange);

        string entree = "N/a";
        string side = "N/a";
        string medium = "N/a";
        string entree2 = "N/a";

        if (difficultyMeter <= difficultyConstant)
        {
            /* meat/veg and grain */
            recipeTimer = 3;

            entree = pickIngredient("ENTREE");
            side = grains[Random.Range(0, 3)];

            recipeTitle = entree + " served in " + side;
        }
        else if (difficultyMeter <= difficultyConstant * 2)
        {
            /* meat/veg and grain AND fat/dairy */
            recipeTimer = 4;

            entree = pickIngredient("ENTREE");
            side = grains[Random.Range(0, 3)];
            medium = pickIngredient("MEDIUM");

            recipeTitle = entree + " served w/ " + side + ", in " + medium;
        }
        else
        {
            /* meat AND veg and grain and fat/dairy */
            recipeTimer = 5;

            entree = meats[Random.Range(0, 4)];
            side = grains[Random.Range(0, 3)];
            medium = pickIngredient("MEDIUM");
            entree2 = veggies[Random.Range(0, 9)];

            recipeTitle = entree + " & " + entree2 + ", w/ " + side + " in " + medium;
        }

        repiceIngredients = new string[4]{entree, side, medium, entree2};

        GameObject recipe = Instantiate(recipePrefab, canvasUI.transform.GetChild(0));
        Recipe recipeInfo = recipe.GetComponentInChildren<Recipe>();

        recipeInfo.title = recipeTitle;
        recipeInfo.ingredients = repiceIngredients;
        recipeInfo.vegan = veganDish;
        recipeInfo.difficulty = recipeTimer - 2;
        recipeInfo.timer = recipeTimer;

        veganDish = false;

        return recipe;
    }

    public void recreateRecipe(GameObject originalRecipe, GameObject targetRecipe)
    {
        Recipe recipeInfoCopy = targetRecipe.GetComponentInChildren<Recipe>() ;
        Recipe recipeInfoOriginal = originalRecipe.GetComponent<Recipe>();

        recipeInfoCopy.title = recipeInfoOriginal.title;
        recipeInfoCopy.ingredients = recipeInfoOriginal.ingredients;
        recipeInfoCopy.vegan = recipeInfoOriginal.vegan;
        recipeInfoCopy.difficulty = recipeInfoOriginal.difficulty;
        recipeInfoCopy.timer = recipeInfoOriginal.timer;
    }

    public void assignIngredient(int remainingTurns, GameObject recipeCard)
    {
        string ingredientName = "";
        Color ingredientColorBase;
        Color ingredientColorTop;

        int typeScalar;
        float maxRange = (totalTurns - (remainingTurns + 1)) / (float) totalTurns;
        float difficultyMeter = Random.Range(0.0f, maxRange);

        if (difficultyMeter <= difficultyConstant)
            typeScalar = 3;
        else
            typeScalar = 5;

        int ingredientType = Random.Range(0, typeScalar);

        ingredientColorBase = baseColors[ingredientType];
        ingredientColorTop = new Color(Mathf.Min(ingredientColorBase.r + 0.1f, 1.0f), Mathf.Min(ingredientColorBase.g + 0.1f, 1.0f), Mathf.Min(ingredientColorBase.b + 0.1f, 1.0f), 1.0f);


        switch (ingredientType)
        {
            case 0:
                ingredientName = meats[Random.Range(0, 4)];
                break;
            case 1:
                ingredientName = veggies[Random.Range(0, 9)];
                break;
            case 2:
                ingredientName = grains[Random.Range(0, 3)];
                break;
            case 3:
                ingredientName = fats[Random.Range(0, 3)];
                break;
            case 4:
                ingredientName = dairy[Random.Range(0, 5)];
                break;
        }

        recipeCard.GetComponent<Image>().color = ingredientColorBase;
        recipeCard.transform.GetChild(0).gameObject.GetComponent<Image>().color = ingredientColorTop;
        recipeCard.transform.GetChild(1).gameObject.GetComponent<Text>().text = ingredientName;
    }
}
