using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientControl : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private int cardID;
    private RectTransform ingredientPos;
    public bool reassign;

    private Clicker mousePointer;
    public GameObject physicalInteract;
    private bool mouseSelected = false;

    private Vector3 cardCurrent;
    private Vector3 cardShrink;
    private Vector3 cardZoom;
    private Quaternion currentRotation;
    private float shrunkRotation;
    private Vector3 currentScale;
    private const float zoomScalar = 1.1f;
    private float passedTime;
    private const float focusingDuration = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        mousePointer = GameObject.Find("\"Chef\"").GetComponent<Clicker>();

        cardID = int.Parse(gameObject.name.Substring(gameObject.name.Length - 1));
        ingredientPos = gameObject.GetComponent<RectTransform>();
        reassign = false;

        cardCurrent = cardShrink = ingredientPos.anchoredPosition;
        cardZoom = new Vector3(cardShrink.x, 0.0f, 0.0f);

        currentRotation = ingredientPos.rotation;
        shrunkRotation = currentRotation.eulerAngles.z;

        physicalInteract = Resources.Load("3D Ingredient Card") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (passedTime <= focusingDuration)
        {
            passedTime += Time.deltaTime;
            float animPercentage = passedTime / focusingDuration;

            if (mouseSelected)
            {
                ingredientPos.anchoredPosition = Vector3.Lerp(cardCurrent, cardZoom, animPercentage);
                ingredientPos.rotation = Quaternion.Lerp(currentRotation, Quaternion.identity, animPercentage);
                ingredientPos.localScale = Vector3.Lerp(currentScale, new Vector3(zoomScalar, zoomScalar, 1.0f), animPercentage);
            }
            else
            {
                ingredientPos.anchoredPosition = Vector3.Lerp(cardCurrent, cardShrink, animPercentage); 
                ingredientPos.rotation = Quaternion.Lerp(currentRotation, Quaternion.Euler(0.0f, 0.0f, shrunkRotation), animPercentage);           
                ingredientPos.localScale = Vector3.Lerp(currentScale, Vector3.one, animPercentage);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseSelected)
        {            
            GameObject physCard = Instantiate(physicalInteract, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1)), Quaternion.Euler(-90.0f, 0.0f, 0.0f)); 
            physCard.GetComponent<CardMovement_Ingredient>().trackMouse();
            physCard.GetComponent<CardMovement_Ingredient>().assignRef(cardID);

            mousePointer.heldObject = physCard;

            reassign = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
        cardCurrent = ingredientPos.anchoredPosition;
        currentRotation = ingredientPos.rotation;
        currentScale = ingredientPos.localScale;

        mouseSelected = true;
        passedTime = 0.0f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        resetCardPos();
    }

    public void resetCardPos()
    {
        cardCurrent = ingredientPos.anchoredPosition;
        currentRotation = ingredientPos.rotation;
        currentScale = ingredientPos.localScale;

        mouseSelected = false;
        passedTime = 0.0f;
    }
}
