using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecipeControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Clicker mousePointer;
    private RectTransform recipePos;

    // Vertical Movement Variables
    private Vector3 snapshotVertPos;
    private Vector3 targetVertPos;
    private const float miniRecipeHeight = 40.0f;
    private float fullRecipeHeight = -62.5f;
    private const float heightLimiter = 25.0f;
    private const float openDuration = 0.25f;
    private float passedTimeOpening;

    // Horizontal Moment Variables
    public Vector3 targetHorizPos;
    public Vector3 snapshotHorizPos;
    private float startRecipeDist = 60.0f;
    private const float endRecipeDestConst = 60.0f;
    private const float endRecipeDestMult = 110.0f;
    public float slideDuration = 0.75f;
    public float passedTimeSliding;

    // Variables for generating a physical recipe
    public GameObject physicalInteract;
    private bool mouseSelected = false;

    void Awake()
    {
        mousePointer = GameObject.Find("\"Chef\"").GetComponent<Clicker>();

        startRecipeDist += GameObject.Find("Canvas").GetComponent<RectTransform>().rect.width;
        recipePos = GetComponent<RectTransform>();
        recipePos.anchoredPosition = snapshotVertPos = targetVertPos = snapshotHorizPos = targetHorizPos = new Vector3(startRecipeDist, miniRecipeHeight, 0.0f);

        physicalInteract = Resources.Load("3D Recipe Card") as GameObject;
    }

    void Start()
    {
        string[] recipeIngredients = gameObject.GetComponent<Recipe>().ingredients;
        for (int i = 0; i < 4; i++)
        {
            if (recipeIngredients[i] == "N/a")
                fullRecipeHeight += heightLimiter;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Controls the vertical movement that expands the recipe card
        if (passedTimeOpening <= openDuration)
        {
            passedTimeOpening += Time.deltaTime;
            float animPercentage = passedTimeOpening / openDuration;

            recipePos.anchoredPosition = Vector3.Lerp(snapshotVertPos, targetVertPos, animPercentage);
        }

        if (passedTimeSliding <= slideDuration)
        {
            passedTimeSliding += Time.deltaTime;
            float animPercentage = passedTimeSliding / slideDuration;

            recipePos.anchoredPosition = Vector3.Lerp(snapshotHorizPos, targetHorizPos, animPercentage);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseSelected)
        {            
            GameObject physCard = Instantiate(physicalInteract, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1)), Quaternion.Euler(-90.0f, 0.0f, 0.0f)); 
            physCard.GetComponent<CardMovement_Recipe>().trackMouse();

            mousePointer.heldObject = physCard;

            GameObject.Find("\"Waiter\"").GetComponent<RecipeManager>().HoldRecipeCard((int) Input.mousePosition.x / ((Screen.width - 60) / 7));
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseSelected = true;

        snapshotVertPos = recipePos.anchoredPosition;
        targetVertPos = new Vector3(recipePos.anchoredPosition.x, fullRecipeHeight, 0.0f);
        passedTimeOpening = 0.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseSelected = false;

        snapshotVertPos = recipePos.anchoredPosition;
        targetVertPos = new Vector3(recipePos.anchoredPosition.x, miniRecipeHeight, 0.0f);
        passedTimeOpening = 0.0f;
    }

    public void setLerpDest(int index)
    {
        snapshotHorizPos = recipePos.anchoredPosition;
        targetHorizPos = new Vector3(endRecipeDestConst + (index * endRecipeDestMult), miniRecipeHeight, 0.0f);
        passedTimeSliding = 0.0f;
    }
}
