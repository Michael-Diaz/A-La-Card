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

    private List<GameObject> activeRecipes;
    private int activeRecipiesNum = 0;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Change for general insertion/recipe generation
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject container = cookbook.generateRecipe(0);
            container.GetComponent<RecipeControl>().setLerpDest(activeRecipiesNum);

            activeRecipes.Add(container);
            activeRecipiesNum++;
        }

        if (!offScreenContainerNull && Input.GetKeyUp(KeyCode.Mouse0))
        {
            GameObject storedRecipe = Instantiate(recipePrefab, canvasUI.transform.GetChild(0));
            cookbook.recreateRecipe(offScreenContainer, storedRecipe);
            storedRecipe.GetComponent<RecipeControl>().setLerpDest(activeRecipiesNum);
            
            activeRecipes.Add(storedRecipe);
            activeRecipiesNum++;

            offScreenContainerNull = true;
            mousePointer.heldObject = null;
        }

        for (int i = 0; i < activeRecipiesNum; i++)
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

        if (activeRecipiesNum > 0)
            activeRecipiesNum--;

        for (int i = 0; i < activeRecipiesNum; i++)
            activeRecipes[i].GetComponent<RecipeControl>().setLerpDest(i);
    }

    public void HoldPhysicalCard(GameObject physRecipeCard)
    {
        GameObject recipeLocation = physRecipeCard.transform.GetChild(0).gameObject;

        cookbook.recreateRecipe(recipeLocation, mousePointer.heldObject);
        cookbook.recreateRecipe(recipeLocation, offScreenContainer);
    }
}
