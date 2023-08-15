using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAdjuster : MonoBehaviour
{
    // Start is called before the first frame update
    private float theConstantOf16And9 = 4f / 3f;
    //이거보다 크면 Height로 맞춤
    private void Awake()
    {
        //StuffToStuff();
    }
    void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Screen.orientation = ScreenOrientation.Landscape;
        }
        StuffToStuff();
    }
    void StuffToStuff()
    {
        float setHeightTo = 0f;
        float setWidthTo = 0f;
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        float floatOfX = canvasScaler.referenceResolution.x;
        float floatOfY = canvasScaler.referenceResolution.y;
        theConstantOf16And9 = floatOfX / floatOfY;
        float theWidth = Screen.width;
        float theHeight = Screen.height;       
        if (theConstantOf16And9 < theWidth / theHeight)
        {
            setHeightTo = theHeight;
            //canvasScaler.matchWidthOrHeight = 1f;
        }
        else
        {
            setWidthTo = theWidth;
            //canvasScaler.matchWidthOrHeight = 0f;
        }
        if (setHeightTo != 0f)
        {
            //float anotherConstant = setHeightTo / 1440f;
            float anotherConstant = setHeightTo / canvasScaler.referenceResolution.y;
            ImageProcessingManager.instance.canvasAdjusterScaleFactor = anotherConstant;
            //canvasScaler.scaleFactor = anotherConstant;
            
        }
        else if (setWidthTo != 0f)
        {
            //float anotherConstant = setWidthTo / 1920f;
            float anotherConstant = setWidthTo / canvasScaler.referenceResolution.x;
            ImageProcessingManager.instance.canvasAdjusterScaleFactor = anotherConstant;
            //canvasScaler.scaleFactor = anotherConstant;
        }
    }
    // Update is called once per frame
    void Update()
    {
        StuffToStuff();
    }
}
