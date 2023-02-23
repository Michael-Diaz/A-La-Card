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

    public GameObject physicalInteract;
    private bool mouseSelected = false;

    private Clicker mousePointer;

    // Start is called before the first frame update
    void Start()
    {
        recipePos = GetComponent<RectTransform>();
        snapshotRecipePos = targetRecipePos = new Vector3(recipePos.anchoredPosition.x, miniRecipeHeight, 0.0f);

        physicalInteract = Resources.Load("3D Recipe Card") as GameObject;

        mousePointer = GameObject.Find("Pointer Controller").GetComponent<Clicker>();
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

        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseSelected)
        {
            GameObject.Find("\"Waiter\"").GetComponent<RecipeManager>().HoldRecipeCard((int) (Input.mousePosition.x / 60.0f));
            
            GameObject physCard = Instantiate(physicalInteract, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1)), Quaternion.Euler(-90.0f, 0.0f, 0.0f));
            physCard.GetComponent<CardMovement_Recipe>().trackMouse();

            mousePointer.heldObject = physCard;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseSelected = true;

        snapshotRecipePos = recipePos.anchoredPosition;
        targetRecipePos = new Vector3(recipePos.anchoredPosition.x, fullRecipeHeight, 0.0f);
        passedTime = 0.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseSelected = false;

        snapshotRecipePos = recipePos.anchoredPosition;
        targetRecipePos = new Vector3(recipePos.anchoredPosition.x, miniRecipeHeight, 0.0f);
        passedTime = 0.0f;
    }
}
