using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ImageDisplayer : MonoBehaviour
{
    public Texture2D thePicture;
    public Texture2D theDefaultPicture;
    public bool thePictureSelected;
    string[] arrayAdjustThisThingOrBased = new string[2];
    int theIndexOfThisSibling;
    // Start is called before the first frame update
    void Start()
    {
        theIndexOfThisSibling = transform.GetSiblingIndex();
    }

    // Update is called once per frame
    void Update()
    {
        if (theIndexOfThisSibling == 0)
        {
            arrayAdjustThisThingOrBased[0] = ImageProcessingManager.instance.adjustThisThing;
        }
        else if (theIndexOfThisSibling == 1)
        {
            arrayAdjustThisThingOrBased[0] = ImageProcessingManager.instance.basedOnThisThing;
        }
        bool theStuffChanged = arrayAdjustThisThingOrBased[1] != arrayAdjustThisThingOrBased[0];
        if (theIndexOfThisSibling == 0 && ImageProcessingManager.instance.reloadTheImageDisplayer1)
        {
            ImageProcessingManager.instance.reloadTheImageDisplayer1 = false;
            theStuffChanged = true;
        }
        else if (theIndexOfThisSibling == 1 && ImageProcessingManager.instance.reloadTheImageDisplayer2)
        {
            ImageProcessingManager.instance.reloadTheImageDisplayer2 = false;
            theStuffChanged = true;
        }
        if (!ImageProcessingManager.instance.secondUpdateAlreadyDone)
        {
            theStuffChanged = false;
        }
        if (theStuffChanged && arrayAdjustThisThingOrBased[0] != "")
        {
            thePictureSelected = true;
        }
        arrayAdjustThisThingOrBased[1] = arrayAdjustThisThingOrBased[0];
        if (theStuffChanged && arrayAdjustThisThingOrBased[0] != "")
        {
            byte[] theByteArray = File.ReadAllBytes(arrayAdjustThisThingOrBased[0]);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(theByteArray);
            tex.Apply();
            thePicture = tex;
            transform.GetChild(3).GetChild(0).GetComponent<Text>().text=
            _ButcherDirectory(arrayAdjustThisThingOrBased[0]);
        }
        if (thePictureSelected)
        {
            transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = thePicture;
            Vector2 someVector = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
            float theXOfPicture = thePicture.width;
            float theYOfPicture = thePicture.height;
            if (theXOfPicture > someVector.x)
            {
                float f = theXOfPicture;
                theXOfPicture = someVector.x * theXOfPicture / f;
                theYOfPicture = someVector.x * theYOfPicture / f;
            }
            if (theYOfPicture > someVector.y)
            {
                float f = theYOfPicture;
                theXOfPicture = someVector.y * theXOfPicture / f;
                theYOfPicture = someVector.y * theYOfPicture / f;
            }
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = thePicture.width.ToString();
            transform.GetChild(1).GetChild(1).GetComponent<Text>().text = thePicture.height.ToString();
            transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(theXOfPicture, theYOfPicture);
            if (theIndexOfThisSibling == 0)
            {
                ImageProcessingManager.instance.theResultImageDisplayer = thePicture;
            }
            
        }
        else
        {
            transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture = theDefaultPicture;
            transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(300f, 300f);
            transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "";
            transform.GetChild(1).GetChild(1).GetComponent<Text>().text = "";
        }
    }
    string _ButcherDirectory(string theDirectoryToButcher)
    {
        string theDirectoryNameTextProxy = theDirectoryToButcher;
        char someChar = '/';
        char anotherChar = '\\';
        string theRemainingCharacters = "";
        char[] theCharArray = theDirectoryNameTextProxy.ToCharArray();
        bool ceaseTheWhile = false;
        int i = theDirectoryNameTextProxy.Length - 1;
        while (i >= 0 && !ceaseTheWhile)
        {
            theRemainingCharacters = theDirectoryNameTextProxy.Remove(0, i + 1);
            if (theCharArray[i] == someChar || theCharArray[i] == anotherChar)
            {
                ceaseTheWhile = true;
            }
            --i;
        }
        theRemainingCharacters = theRemainingCharacters.Remove(theRemainingCharacters.Length - 4, 4);
        return theRemainingCharacters;
    }
}
