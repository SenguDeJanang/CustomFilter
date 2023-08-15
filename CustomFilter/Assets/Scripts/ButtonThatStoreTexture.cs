using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ButtonThatStoreTexture : MonoBehaviour
{
    public Texture2D theTextureStored;
    //bool resistForgetting;
    //Texture2D theTextureToResistForgetting;
    public delegate void MultiDelegate(int theNumberOnInputField, Color32 theColorInHexOnInputField);
    //나중에 object 사용하도록 바
    
    public MultiDelegate myMultiDelegate;
    int numberOnInputField;
    Color colorInHexOnInputField;
    string[] theColorChangeDetection = new string[2];
    Text theTextToParseNumbersFrom;
    // Start is called before the first frame update
    public bool needsBase;
    RawImage theRawImage;
    bool takesHexadecimal;
    public void ForgetTheTexture()
    {
        theTextureStored = null;
    }
    void Start()
    {
        theRawImage = gameObject.GetComponent<RawImage>();
        ImageProcessingManager.instance.theButtonsForget += ForgetTheTexture;
        Transform t = transform.GetChild(0).GetChild(0).GetChild(0);
        takesHexadecimal = t.GetComponent<InputField>().contentType == InputField.ContentType.Alphanumeric;
        theTextToParseNumbersFrom = t.GetChild(t.childCount - 1).GetComponent<Text>();
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(RestoreButton);
        transform.GetChild(2).GetComponent<Button>().onClick.AddListener(PressApplyButton);
    }
    void PressApplyButton()
    {
        DoTheButtonsJob();
    }
    public void StoreTheTexture()
    {
        theTextureStored = ImageProcessingManager.instance.theResultImageDisplayer;
        //theTextureToResistForgetting = theTextureStored;
        //resistForgetting = true;
        //StartCoroutine(WaitFiveFrames());
    }
    /*IEnumerator WaitFiveFrames()
    {
        yield return 5;
        resistForgetting = false;
    }*/
    // Update is called once per frame
    void Update()
    {
        if (!needsBase)
        {
            theRawImage.color = Color.magenta;
        }
        else
        {
            theRawImage.color = Color.white;
        }
        string toZeroIfEmpty = theTextToParseNumbersFrom.text;
        if (toZeroIfEmpty == "" || takesHexadecimal)
        {
            toZeroIfEmpty = "0";
        }
        numberOnInputField = int.Parse(toZeroIfEmpty);
        string toStringOfZerosIfEmpty = theTextToParseNumbersFrom.text;
        if (toStringOfZerosIfEmpty.Length != 6)
        {
            toStringOfZerosIfEmpty = "000000";
        }
        string emptyString = "";
        char[] charArray = toStringOfZerosIfEmpty.ToCharArray();
        for (int k = 0; k < charArray.Length; k++)
        {
            emptyString += ForHexadecimal(charArray[k]);
        }
        toStringOfZerosIfEmpty = emptyString;
        theColorChangeDetection[0] = toStringOfZerosIfEmpty;
        if (theColorChangeDetection[0] != theColorChangeDetection[1])
        {
            colorInHexOnInputField = HexToColor(toStringOfZerosIfEmpty);
        }
        theColorChangeDetection[1] = theColorChangeDetection[0];
        /*if (resistForgetting)
        {
            if (theTextureToResistForgetting != null)
            {
                theTextureStored = theTextureToResistForgetting;
            }
            //
        }
        else
        {
            theTextureToResistForgetting = null;
        }*/
    }
    
    public void RestoreButton()
    {
        if (theTextureStored == null)
        {
            ImageProcessingManager.instance.theErrorMessageToTheUser = "The Button Forgor (Skull Emoji)";
            return;
        }
        Texture2D texToSave = theTextureStored;
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(ImageProcessingManager.instance.adjustThisThing, bytes);
        ImageProcessingManager.instance.ReloadStuff();
        ImageProcessingManager.instance.theErrorMessageToTheUser = "";
    }
    public void DoTheButtonsJob()
    {
        if (myMultiDelegate != null)
        {
            if (ImageProcessingManager.instance.adjustThisThing == "")
            {
                ImageProcessingManager.instance.SayTheyArentTheSameResolution(0);
                return;
            }
            if (needsBase)
            {
                if (ImageProcessingManager.instance.basedOnThisThing == "")
                {
                    ImageProcessingManager.instance.SayTheyArentTheSameResolution(1);
                    return;
                }
                else if (ImageProcessingManager.instance.basedOnThisThing == ImageProcessingManager.instance.adjustThisThing)
                {
                    ImageProcessingManager.instance.SayTheyArentTheSameResolution(2);
                    return;
                }
            }
            StoreTheTexture();
            StartCoroutine(WaitAFrame());
        }       
    }
    IEnumerator WaitAFrame()
    {
        yield return 2;
        myMultiDelegate(numberOnInputField, colorInHexOnInputField);
    }
    char ForHexadecimal(char unfilteredCharacter)
    {
        char c = unfilteredCharacter;
        c = char.ToUpper(c);
        bool isOneOFTheAllowedCharacters0 = c == '0' || c == '1' || c == '2' || c == '3';
        bool isOneOFTheAllowedCharacters1 = c == '4' || c == '5' || c == '6' || c == '7';
        bool isOneOFTheAllowedCharacters2 = c == '8' || c == '9' || c == 'A' || c == 'B';
        bool isOneOFTheAllowedCharacters3 = c == 'C' || c == 'D' || c == 'E' || c == 'F';
        bool isOneOfTheAllowedCharacters = isOneOFTheAllowedCharacters0 || isOneOFTheAllowedCharacters1 || isOneOFTheAllowedCharacters2 || isOneOFTheAllowedCharacters3;
        if (isOneOfTheAllowedCharacters)
        {
            return c;
        }
        else
        {
            return '0';
        }
    }
    private Color HexToColor(string hex)
    {
        hex = hex.Replace("0x", "");
        hex = hex.Replace("#", "");
        byte a = 255;
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, a);
    }
}
