using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    private GameObject canvasUI;
    public GameObject recipePrefab;
    private const float startRecipeHeight = 40.0f;
    private float startRecipeDist = 60.0f + Screen.width;
    private const float endRecipeDestConst = 60.0f;
    private const float endRecipeDestMult = 110.0f;

    private const float slideAnimLen = 0.75f;

    private List<Recipe> activeRecipes;
    private int activeRecipiesNum = 0;

    public Recipe offScreenContainer;
    public bool offScreenContainerNull;

    [System.Serializable]
    public class Recipe
    {
        public GameObject recipeCardRef;
        public string name;
        public int timer;

        public Vector3 lerpDest;
        public Vector3 lerpStart;
        public float personalCardTimer;

        public Recipe(GameObject recipeRef)
        {
            recipeCardRef = recipeRef;

            name = recipeCardRef.transform.GetChild(0).GetComponent<Text>().text.ToString();
            timer = int.Parse(recipeCardRef.transform.GetChild(1).GetComponent<Text>().text);

            lerpDest = lerpStart = new Vector3(0.0f, 0.0f, 0.0f);

            personalCardTimer = 0.0f;
        }

        public void resetLerp()
        {
            lerpStart = recipeCardRef.GetComponent<RectTransform>().anchoredPosition;
            personalCardTimer = 0.0f;
        }

        public void setLerpDest(int index)
        {
            lerpDest = new Vector3(endRecipeDestConst + (index * endRecipeDestMult), startRecipeHeight, 0.0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        canvasUI = GameObject.Find("Canvas");

        activeRecipes = new List<Recipe>();

        offScreenContainerNull = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Change for general insertion/recipe generation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject recipe = Instantiate(recipePrefab, canvasUI.transform);
            RectTransform startRecipePos = recipe.GetComponent<RectTransform>();
            startRecipePos.anchoredPosition = new Vector3(startRecipeDist, startRecipeHeight, 0.0f);

            /* Write code here to randomly generate recipe details, possibly take preset values from a database randomly */

            Recipe container = new Recipe(recipe);
            container.resetLerp();

            activeRecipes.Add(container);
            activeRecipiesNum++;
        }

        // Change for general removal
        /*if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Destroy(activeRecipes[0].recipeCardRef);
            activeRecipes.RemoveAt(0);

            if (activeRecipiesNum > 0)
                activeRecipiesNum--;

            for (int i = 0; i < activeRecipiesNum; i++)
                activeRecipes[i].resetLerp();
        }*/

        if (!offScreenContainerNull && Input.GetKeyUp(KeyCode.Mouse0))
        {
            GameObject storedRecipe = Instantiate(recipePrefab, canvasUI.transform);
            RectTransform storedRecipePos = storedRecipe.GetComponent<RectTransform>();
            storedRecipePos.anchoredPosition = new Vector3(startRecipeDist, startRecipeHeight, 0.0f);
            
            activeRecipes.Add(new Recipe(storedRecipe));
            activeRecipiesNum++;

            activeRecipes[activeRecipiesNum - 1].resetLerp();

            offScreenContainerNull = true;
        }

        for (int i = 0; i < activeRecipiesNum; i++)
        {
            Recipe tempContainer = activeRecipes[i];
            tempContainer.setLerpDest(i);

            if (tempContainer.recipeCardRef.GetComponent<RectTransform>().anchoredPosition.x != tempContainer.lerpDest.x)
            {

                tempContainer.personalCardTimer += Time.deltaTime;
                float animPercentage = tempContainer.personalCardTimer / slideAnimLen;

                tempContainer.recipeCardRef.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(tempContainer.lerpStart, tempContainer.lerpDest, animPercentage);
            }
        }
    }

    public void HoldRecipeCard(int index)
    {
        offScreenContainer = activeRecipes[index];
        offScreenContainerNull = false;

        Destroy(activeRecipes[index].recipeCardRef);
        activeRecipes.RemoveAt(index);

        if (activeRecipiesNum > 0)
            activeRecipiesNum--;

        for (int i = 0; i < activeRecipiesNum; i++)
            activeRecipes[i].resetLerp();
    }
}
