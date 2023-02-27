using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeManager : MonoBehaviour
{
    private GameObject canvasUI;
    public GameObject recipePrefab;

    private Clicker mousePointer;
    private Cookbook cookbook;

    private int turnsRemaining = 80;

    private List<GameObject> activeRecipes;
    private int activeRecipesNum = 0;

    public GameObject offScreenContainer;
    private GameObject offScreenTitle;
    private GameObject offScreenTimer;
    public bool offScreenContainerNull;

    // Start is called before the first frame update
    void Start()
    {
        canvasUI = GameObject.Find("Canvas");

        float canvasWidth = canvasUI.GetComponent<RectTransform>().rect.width;
        float canvasHeight = canvasUI.GetComponent<RectTransform>().rect.height;

        canvasUI.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(canvasWidth, canvasHeight);

        mousePointer = GameObject.Find("\"Chef\"").GetComponent<Clicker>();
        cookbook = GameObject.Find("\"Recipe Vault\"").GetComponent<Cookbook>();

        activeRecipes = new List<GameObject>();

        offScreenContainer = canvasUI.transform.GetChild(0).GetChild(0).gameObject;
        offScreenContainerNull = true;

        for (int i = 0; i < 3; i++)
        {
            newRecipe();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!offScreenContainerNull && Input.GetKeyUp(KeyCode.Mouse0))
        {
            GameObject storedRecipe = Instantiate(recipePrefab, canvasUI.transform.GetChild(0));
            cookbook.recreateRecipe(offScreenContainer, storedRecipe);
            storedRecipe.GetComponent<RecipeControl>().setLerpDest(activeRecipesNum);
            
            activeRecipes.Add(storedRecipe);
            activeRecipesNum++;

            offScreenContainerNull = true;
            mousePointer.heldObject = null;
        }

        for (int i = 0; i < activeRecipesNum; i++)
        {
            GameObject tempContainer = activeRecipes[i];
            RecipeControl tempControl = tempContainer.GetComponent<RecipeControl>();

            if (tempControl.GetComponent<RectTransform>().anchoredPosition.x != tempControl.targetHorizPos.x)
            {

                tempControl.passedTimeSliding += Time.deltaTime;
                float animPercentage = tempControl.passedTimeSliding / tempControl.slideDuration;

                tempContainer.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(tempControl.snapshotHorizPos, tempControl.targetHorizPos, animPercentage);
            }
        }
    }

    public void newRecipe()
    {
        GameObject container = cookbook.generateRecipe(turnsRemaining);
        container.GetComponent<RecipeControl>().setLerpDest(activeRecipesNum);

        activeRecipes.Add(container);
        activeRecipesNum++;
    }

    public void HoldRecipeCard(int index)
    {
        cookbook.recreateRecipe(activeRecipes[index], mousePointer.heldObject);
        cookbook.recreateRecipe(activeRecipes[index], offScreenContainer);
        offScreenContainerNull = false;

        removeRecipe(activeRecipes[index].GetComponent<RectTransform>());
    }

    public void removeRecipe(RectTransform position)
    {
        int posIndex = (int) (position.anchoredPosition.x - 60) / 110;

        GameObject removal = activeRecipes[posIndex];
        activeRecipes.RemoveAt(posIndex);
        Destroy(removal);

        if (activeRecipesNum > 0)
            activeRecipesNum--;

        for (int i = 0; i < activeRecipesNum; i++)
            activeRecipes[i].GetComponent<RecipeControl>().setLerpDest(i);
    }

    public void HoldPhysicalCard(GameObject physRecipeCard)
    {
        GameObject recipeLocation = physRecipeCard.transform.GetChild(0).gameObject;

        cookbook.recreateRecipe(recipeLocation, mousePointer.heldObject);
        cookbook.recreateRecipe(recipeLocation, offScreenContainer);
    }

    public void endTurn()
    {
        if (turnsRemaining > 1)
        {
            turnsRemaining--;

            int activeRecipesSnapshot = activeRecipesNum;
            if ((activeRecipesSnapshot < 7))
            {
                for (int i = 1; i < Random.Range(1, Mathf.Min(5, 7 - activeRecipesSnapshot + 2)); i++)
                    newRecipe();
            }
        }
    }
}
