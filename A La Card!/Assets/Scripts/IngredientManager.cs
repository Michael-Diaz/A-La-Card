using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientManager : MonoBehaviour
{
    private bool showCards;
    private const float showThreshold = 0.2f;
    private const float hideThreshold = 0.35f;
    private const float hideOffset = 0.25f;
    private float passedTime;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 showingCards;
    private Vector3 hidingCards;
    private const float toggleDuration = 0.25f;

    private GameObject ingredientsUI;
    private RectTransform ingredientsUIPos;

    // Start is called before the first frame update
    void Start()
    {
        showCards = false;

        showingCards = Vector3.zero;
        hidingCards = new Vector3(0.0f, -Screen.height * hideOffset, 0.0f);
        startPosition = endPosition = hidingCards;

        GameObject canvasUI = GameObject.Find("Canvas");
        float canvasWidth = canvasUI.GetComponent<RectTransform>().rect.width;
        float canvasHeight = canvasUI.GetComponent<RectTransform>().rect.height;

        ingredientsUI = canvasUI.transform.GetChild(1).gameObject;
        ingredientsUIPos = ingredientsUI.GetComponent<RectTransform>();
        ingredientsUIPos.sizeDelta = new Vector2(canvasWidth, canvasHeight);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;

        if (!showCards && (mousePos.y <= Screen.height * showThreshold))
        {
            showCards = true;

            startPosition = ingredientsUIPos.anchoredPosition;
            endPosition = showingCards;
            passedTime = 0.0f;
        }
        else if (showCards && (mousePos.y >= Screen.height * hideThreshold))
        {
            showCards = false;

            startPosition = ingredientsUIPos.anchoredPosition;
            endPosition = hidingCards;
            passedTime = 0.0f;
        }

        if (passedTime <= toggleDuration)
        {
            passedTime += Time.deltaTime;
            float animPercentage = passedTime / toggleDuration;

            ingredientsUIPos.anchoredPosition = Vector3.Lerp(startPosition, endPosition, animPercentage);
        }
    }
}
