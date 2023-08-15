using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SwapPlacesOnADime : MonoBehaviour
{
    //bool visibleByDefault;
    public GameObject thingToSwapPlacesWith;
    Vector3 theDefaultPosition;
    public Button theSwapButton;
    public string theButtonTextsString;
    public string theButtonTextsSecondaryString;
    bool isDefaultState;
    Text theButtonsText;
    bool firstUpdateDone;
    // Start is called before the first frame update
    void Start()
    {
        isDefaultState = true;
        theDefaultPosition = transform.position;
        theSwapButton.onClick.AddListener(SwapPlaces);
        theButtonsText = theSwapButton.gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapPlaces();
        }*/
        if (!firstUpdateDone)
        {
            firstUpdateDone = true;
            theDefaultPosition = transform.position;
        }
        if (isDefaultState)
        {
            theButtonsText.text = theButtonTextsString;
        }
        else
        {
            theButtonsText.text = theButtonTextsSecondaryString;
        }
    }
    public void SwapPlaces()
    {
        if (thingToSwapPlacesWith.transform.position != theDefaultPosition)
        {
            transform.position = thingToSwapPlacesWith.transform.position;
            thingToSwapPlacesWith.transform.position = theDefaultPosition;
            isDefaultState = false;
        }
        else
        {
            thingToSwapPlacesWith.transform.position = transform.position;
            transform.position = theDefaultPosition;
            isDefaultState = true;
        }
    }
}
