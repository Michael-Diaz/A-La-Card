using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform recipePos;
    private Vector3 snapshotRecipePos;
    private Vector3 targetRecipePos;
    private const float miniRecipeHeight = 40.0f;
    private const float fullRecipeHeight = -50.0f;

    private const float openDuration = 0.25f;
    private float passedTime;

    // Start is called before the first frame update
    void Start()
    {
        recipePos = GetComponent<RectTransform>();
        snapshotRecipePos = targetRecipePos = new Vector3(recipePos.anchoredPosition.x, miniRecipeHeight, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (passedTime <= openDuration)
        {
            passedTime += Time.deltaTime;
            float animPercentage = passedTime / openDuration;

            recipePos.anchoredPosition = Vector3.Lerp(snapshotRecipePos, targetRecipePos, animPercentage);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Recipe moused over");

        snapshotRecipePos = recipePos.anchoredPosition;
        targetRecipePos = new Vector3(recipePos.anchoredPosition.x, fullRecipeHeight, 0.0f);
        passedTime = 0.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Recipe moused off");

        snapshotRecipePos = recipePos.anchoredPosition;
        targetRecipePos = new Vector3(recipePos.anchoredPosition.x, miniRecipeHeight, 0.0f);
        passedTime = 0.0f;
    }
}
