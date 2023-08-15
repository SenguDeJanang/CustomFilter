using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RememberDirectoryShowFile : MonoBehaviour
{
    // Start is called before the first frame update
    public string theFileDirectory;
    public Button theButton;
    public Texture2D thePicture;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _StuffIWouldStuffInUpdate();
        if (ImageProcessingManager.instance.selectedFile == theFileDirectory)
        {
            transform.GetChild(0).GetComponent<RawImage>().color = Color.cyan;
        }
        else
        {
            transform.GetChild(0).GetComponent<RawImage>().color = Color.white;
        }
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
        transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(theXOfPicture, theYOfPicture);
    }
    public void HaveTheSingletonMimicThis()
    {
        if (ImageProcessingManager.instance.selectedFile != theFileDirectory)
        {
            ImageProcessingManager.instance.selectedFile = theFileDirectory;
        }
        else
        {
            ImageProcessingManager.instance.selectedFile = "";//ºóÄ­
        }
    }
    void _StuffIWouldStuffInUpdate()
    {
        Text directoryName = transform.GetChild(1).GetComponent<Text>();
        string theDirectoryNameTextProxy = theFileDirectory;
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
        directoryName.text = theRemainingCharacters;
    }
}
