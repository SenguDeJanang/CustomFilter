using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PalettesHexToColor : MonoBehaviour
{
    Color colorInHexOnInputField;
    Text theTextToParseNumbersFrom;
    string[] theColorChangeDetection = new string[2];
    // Start is called before the first frame update
    void Start()
    {
        Transform t = transform.GetChild(0);
        theTextToParseNumbersFrom = t.GetChild(t.childCount - 1).GetComponent<Text>();
        transform.GetChild(1).GetComponent<Button>().onClick.AddListener(TryToDestroyItself);
    }
    void TryToDestroyItself()
    {
        if (transform.parent.childCount <= 2)
        {
            ImageProcessingManager.instance.SayTheyArentTheSameResolution(4);
            return;
        }
        Destroy(gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
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
        gameObject.GetComponent<RawImage>().color = colorInHexOnInputField;
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
