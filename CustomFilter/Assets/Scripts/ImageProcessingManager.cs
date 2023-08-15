using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class ImageProcessingManager : MonoBehaviour
{
    public static ImageProcessingManager instance = null;
    public string adjustThisThing;
    public string basedOnThisThing;
    public string theExploredDirectory;
    public string deviceFirstStorageName;
    public string theErrorMessageToTheUser;
    public GameObject theFileExplorer;
    public GameObject fileFolderGrid;
    public GameObject lobbyQuestionMark;
    public bool showTheRightFile;
    public GameObject fileIconPrefab;
    public GameObject folderIconPrefab;
    public string selectedFile;
    public bool isTheFileExplorerOn;
    public bool reloadTheImageDisplayer1;
    public bool reloadTheImageDisplayer2;
    public GameObject objectToPool;
    public int howMuchFileIconIsEstimatedToBeNeeded;
    public int howMuchFolderIconIsEstimatedToBeNeeded;
    public List<GameObject> pooledFileIconObjects;
    public List<GameObject> pooledFolderIconObjects;
    public UnityAction theButtonsForget;
    public Texture2D theResultImageDisplayer;
    public GameObject palettePanel;
    public float canvasAdjusterScaleFactor;
    public static ImageProcessingManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    IEnumerator WaitAFrame2()
    {
        yield return 2;
        ReloadTheGrid();    
        reloadTheImageDisplayer1 = true;
        reloadTheImageDisplayer2 = true;
    }
    public void ReloadTheGridDelayed()
    {
        StartCoroutine(WaitAFrame());
    }
    public void ReloadStuff()
    {
        StartCoroutine(WaitAFrame2());
    }
    IEnumerator WaitAFrame()
    {
        yield return 2;
        ReloadTheGrid();
    }
    void ReloadTheGrid()
    {
        showTheRightFile = true;
        for (int i = 0; i < fileFolderGrid.transform.childCount; i++)
        {
            Destroy(fileFolderGrid.transform.GetChild(i).gameObject);
        }
        string[] folderEntries = Directory.GetDirectories(theExploredDirectory);
        for (int i = 0; i < folderEntries.Length; i++)
        {
            GameObject folderObj = Instantiate(folderIconPrefab);
            folderObj.transform.SetParent(fileFolderGrid.transform);
            if (canvasAdjusterScaleFactor != 0)
            {
                folderObj.transform.localScale *= canvasAdjusterScaleFactor;
            }            
            folderObj.GetComponent<RememberDirectoryShowFolder>().theFolderDirectory = folderEntries[i];
            folderObj.GetComponent<RememberDirectoryShowFolder>().theButton = folderObj.GetComponent<Button>();
            folderObj.GetComponent<RememberDirectoryShowFolder>().theButton.onClick.AddListener(folderObj.GetComponent<RememberDirectoryShowFolder>().HaveTheSingletonMimicThis);
        }
        string[] fileEntriesUnfiltered = Directory.GetFiles(theExploredDirectory);
        string[] fileEntries = FilterThoseThatArntPNG(fileEntriesUnfiltered);
        for (int i = 0; i < fileEntries.Length; i++)
        {
            GameObject fileObj = Instantiate(fileIconPrefab);
            fileObj.transform.SetParent(fileFolderGrid.transform);
            if (canvasAdjusterScaleFactor != 0)
            {
                fileObj.transform.localScale *= canvasAdjusterScaleFactor;
            }
            fileObj.GetComponent<RememberDirectoryShowFile>().theFileDirectory = fileEntries[i];
            byte[] theByteArray = File.ReadAllBytes(fileObj.GetComponent<RememberDirectoryShowFile>().theFileDirectory);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(theByteArray);
            tex.Apply();
            fileObj.GetComponent<RememberDirectoryShowFile>().thePicture = tex;
            fileObj.GetComponent<RememberDirectoryShowFile>().theButton = fileObj.GetComponent<Button>();
            fileObj.GetComponent<RememberDirectoryShowFile>().theButton.onClick.AddListener(fileObj.GetComponent<RememberDirectoryShowFile>().HaveTheSingletonMimicThis);
        }
    }
    string[] FilterThoseThatArntPNG(string[] theUnfiltered)
    {
        List<string> listOfStrings = new List<string>();
        for (int i = 0; i < theUnfiltered.Length; i++)
        {
            string theThingToTest = theUnfiltered[i];
            if (theThingToTest.Length > 3)
            {
                if ((theThingToTest.Remove(0, theThingToTest.Length - 3).ToUpper()).Contains("PNG"))
                {
                    listOfStrings.Add(theUnfiltered[i]);
                }
            }
        }
        return listOfStrings.ToArray();
    }
    // Start is called before the first frame update
    void Start()
    {
        deviceFirstStorageName = DataPathButcherer(Application.persistentDataPath);
        theExploredDirectory = deviceFirstStorageName;
        theErrorMessageToTheUser = "Welcome to... whatever this is.";
        adjustThisThing = "";
        basedOnThisThing = "";
        secondUpdateAlreadyDone = false;
        ReloadTheGrid();
        //Thing();
    }
    void AssignSomeButtons()
    {
        theFileExplorer.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(BacktrackTheDirectory);
        theFileExplorer.transform.GetChild(5).GetComponent<Button>().onClick.AddListener(QuitThisThing);
        theFileExplorer.transform.GetChild(6).GetComponent<Button>().onClick.AddListener(ToggleTheFileExplorer);
        theFileExplorer.transform.GetChild(7).GetComponent<Button>().onClick.AddListener(SetAsTheBase);
        theFileExplorer.transform.GetChild(8).GetComponent<Button>().onClick.AddListener(SetAsTheAdjusted);
        lobbyQuestionMark.transform.GetChild(3).GetComponent<Button>().onClick.AddListener(ToggleTheFileExplorer);
        lobbyQuestionMark.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(QuitThisThing);
        Transform theButtonPanel = lobbyQuestionMark.transform.GetChild(5);
        theButtonPanel.GetChild(0).GetComponent<ButtonThatStoreTexture>().myMultiDelegate += CopyPasteTranslucency;
        theButtonPanel.GetChild(1).GetComponent<Button>().onClick.AddListener(BleachImage);
        theButtonPanel.GetChild(2).GetComponent<Button>().onClick.AddListener(ShadeToShine);
        theButtonPanel.GetChild(3).GetComponent<Button>().onClick.AddListener(ChangeToPitchBlack);
        theButtonPanel.GetChild(4).GetComponent<Button>().onClick.AddListener(InvertColors);
        theButtonPanel.GetChild(5).GetComponent<ButtonThatStoreTexture>().myMultiDelegate += ToSetColor;
        theButtonPanel.GetChild(6).GetComponent<ButtonThatStoreTexture>().myMultiDelegate += ToPitchBlackAndPureWhite;
        theButtonPanel.GetChild(7).GetComponent<ButtonThatStoreTexture>().myMultiDelegate += RemoveSimilar;
        theButtonPanel.GetChild(8).GetComponent<ButtonThatStoreTexture>().myMultiDelegate += RemoveDissimilar;
        Transform thePalettePanel = lobbyQuestionMark.transform.GetChild(6);
        palettePanel = thePalettePanel.gameObject;
        thePalettePanel.GetChild(thePalettePanel.childCount - 1).GetChild(1).GetComponent<Button>().onClick.AddListener(ApplyStrictPalette);
    }
    public void ApplyStrictPalette()
    {
        if (adjustThisThing == "")
        {
            SayTheyArentTheSameResolution(0);
            return;
        }
        List<Vector3> vector3sFromPalette = new List<Vector3>();
        for (int i = 0; i < palettePanel.transform.childCount - 1; i++)
        {
            Color32 col = palettePanel.transform.GetChild(i).
                GetComponent<RawImage>().color;
            int colR = col.r;
            int colG = col.g;
            int colB = col.b;
            vector3sFromPalette.Add(new Vector3((float)colR, 
                (float)colG, (float)colB));
        }
        Vector3[] v3Array = vector3sFromPalette.ToArray();
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        for (int j = 0; j < texToSave.width; j++)
        {
            for (int k = 0; k < texToSave.height; k++)
            {
                Color32 theColor = texToSave.GetPixel(j, k);
                int theTranslucencyAlpha = theColor.a;
                int theR = theColor.r;
                int theG = theColor.g;
                int theB = theColor.b;
                Vector3 theColorInVector = new Vector3((float)theR, 
                    (float)theG, (float)theB);
                //벡터로 치환했을 때 어느 색에서 나온 벡터가 가장 가까울까?
                float minDist = Mathf.Infinity;
                Vector3 currentPos = theColorInVector;
                foreach (Vector3 v3 in v3Array)
                {
                    float dist = Vector3.Distance(v3, currentPos);
                    if (dist < minDist)
                    {
                        theColorInVector = v3;
                        minDist = dist;
                    }
                }
                //벡터로 치환했을 때 어느 색에서 나온 벡터가 가장 가까울까?
                theR = Mathf.FloorToInt(theColorInVector.x);
                theG = Mathf.FloorToInt(theColorInVector.y);
                theB = Mathf.FloorToInt(theColorInVector.z);
                
                texToSave.SetPixel(j, k, new Color32((byte)theR, (byte)theG, 
                    (byte)theB, (byte)theTranslucencyAlpha));
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }

    public void SetAsTheBase()
    {
        basedOnThisThing = selectedFile;
        selectedFile = "";
    }
    public void SetAsTheAdjusted()
    {
        adjustThisThing = selectedFile;
        selectedFile = "";
        StartCoroutine(WaitAFrame3());
    }
    IEnumerator WaitAFrame3()
    {
        yield return 2;
        if (theButtonsForget != null)
        {
            theButtonsForget.Invoke();
        }
    }
    public void QuitThisThing()
    {
        Application.Quit();
    }
    public void ToggleTheFileExplorer()
    {
        if (!isTheFileExplorerOn)
        {
            isTheFileExplorerOn = true;
        }
        else
        {
            isTheFileExplorerOn = false;
        }
    }
    public void BacktrackTheDirectory()
    {
        theExploredDirectory = BacktrackDirectory(theExploredDirectory);
        ReloadTheGridDelayed();
    }
    string BacktrackDirectory(string directoryToBacktrack)
    {
        if (directoryToBacktrack == deviceFirstStorageName)
        {
            return directoryToBacktrack;
        }
        char[] theCharArray = directoryToBacktrack.ToCharArray();
        char someChar = '/';
        char anotherChar = '\\';
        string theRemainingCharacters = "";
        int ceaseTheWhile = 0;
        int i = directoryToBacktrack.Length - 1;
        while (i > 0 && ceaseTheWhile < 1)
        {
            theRemainingCharacters = directoryToBacktrack.Remove(i, directoryToBacktrack.Length - i);
            if (theCharArray[i] == someChar || theCharArray[i] == anotherChar)
            {
                ++ceaseTheWhile;
            }
            --i;
        }
        return theRemainingCharacters;
    }
    string DataPathButcherer(string thePath)
    {
        string theDirectoryNameTextProxy = thePath;
        char someChar = '/';
        char anotherChar = '\\';
        string theRemainingCharacters = "";
        char[] theCharArray = theDirectoryNameTextProxy.ToCharArray();
        int ceaseTheWhile = 0;
        int i = 0;
        while (i < theDirectoryNameTextProxy.Length && ceaseTheWhile < 2)
        {
            theRemainingCharacters = theDirectoryNameTextProxy.Remove(i, theDirectoryNameTextProxy.Length - i);
            if (theCharArray[i] == someChar || theCharArray[i] == anotherChar)
            {
                ++ceaseTheWhile;
            }
            ++i;
        }

        return theRemainingCharacters;
    }
    public void SayTheyArentTheSameResolution(int errorMessageType)
    {
        if (errorMessageType == 3)
        {
            theErrorMessageToTheUser = "Those two pictures have different resolutions. resolution needs to be the same.";
        }
        else if (errorMessageType == 2)
        {
            theErrorMessageToTheUser = "Those two pictures are actually the same file. they need to be different.";
        }
        else if (errorMessageType == 1)
        {
            theErrorMessageToTheUser = "You only picked what would change, please choose what it would be based on.";
        }
        else if (errorMessageType == 0)
        {
            theErrorMessageToTheUser = "You only picked what it would be based on, please choose what would change.";
        }
        else if (errorMessageType == 4)
        {
            theErrorMessageToTheUser = "You gotta have at least one color.";
        }
        else if (errorMessageType == 5)
        {
            theErrorMessageToTheUser = "You already have the maximum color.";
        }
    }
    void BleachImage()
    {
        if (basedOnThisThing == "")
        {
            SayTheyArentTheSameResolution(1);
            return;
        }
        else if (adjustThisThing == "")
        {
            SayTheyArentTheSameResolution(0);
            return;
        }
        else if (basedOnThisThing == adjustThisThing)
        {
            SayTheyArentTheSameResolution(2);
            return;
        }
        byte[] theByteArray = File.ReadAllBytes(basedOnThisThing);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(theByteArray);
        tex.Apply();
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        if (tex.width != texToSave.width || tex.height != texToSave.height)
        {
            SayTheyArentTheSameResolution(3);
            return;
        }
        for (int j = 0; j < tex.width; j++)
        {
            for (int k = 0; k < tex.height; k++)
            {
                Color32 theColor = tex.GetPixel(j, k);
                float H, S, V;
                Color.RGBToHSV(theColor, out H, out S, out V);
                S = 0f;
                theColor = Color.HSVToRGB(H, S, V);
                float theBrightnessOfTheColor = ((float)theColor.r + 
                    (float)theColor.g + (float)theColor.b) / 3f;
                float theDarknessOfTheColor = 255f - theBrightnessOfTheColor;
                
                Color32 theShading = texToSave.GetPixel(j, k);
                byte theShadingsAlpha = theShading.a;
                Color.RGBToHSV(theShading, out H, out S, out V);
                S = 0f;
                theShading = Color.HSVToRGB(H, S, V);
                int newRed = theShading.r;
                int newGreen = theShading.g;
                int newBlue = theShading.b;
                if (theBrightnessOfTheColor >= 127.5f)
                {
                    //대비 계산:
                    theDarknessOfTheColor /= 2f * 259f / 255f;
                    float factor = (259f * (theDarknessOfTheColor + 255f)) / 
                        (255f * (259f - theDarknessOfTheColor));
                    newRed = Mathf.FloorToInt(factor * (theShading.r - 128) + 128);
                    newGreen = Mathf.FloorToInt(factor * (theShading.g - 128) + 128);
                    newBlue = Mathf.FloorToInt(factor * (theShading.b - 128) + 128);
                    theDarknessOfTheColor *= 2f * 259f / 255f;

                    //밝기 계산:
                    newRed = Mathf.FloorToInt(newRed + 
                        Mathf.FloorToInt(theDarknessOfTheColor));
                    newGreen = Mathf.FloorToInt(newGreen + 
                        Mathf.FloorToInt(theDarknessOfTheColor));
                    newBlue = Mathf.FloorToInt(newBlue + 
                        Mathf.FloorToInt(theDarknessOfTheColor));
                }//아랫쪽 else문에서는 밝기를 계산한 다음 대비를 계산한다.
                else
                {
                    theDarknessOfTheColor /= 2f * 259f / 255f;
                    newRed = Mathf.FloorToInt(theShading.r + Mathf.FloorToInt(theDarknessOfTheColor));
                    newGreen = Mathf.FloorToInt(theShading.g + Mathf.FloorToInt(theDarknessOfTheColor));
                    newBlue = Mathf.FloorToInt(theShading.b + Mathf.FloorToInt(theDarknessOfTheColor));

                    theDarknessOfTheColor *= 2f * 259f / 255f;
                    float factor = (259f * (theDarknessOfTheColor + 255f)) / (255f * (259f - theDarknessOfTheColor));
                    newRed = Mathf.FloorToInt(factor * (newRed - 128) + 128);
                    newGreen = Mathf.FloorToInt(factor * (newGreen - 128) + 128);
                    newBlue = Mathf.FloorToInt(factor * (newBlue - 128) + 128);
                }                
                if (newRed > 255){ newRed = 255; }
                else if (newRed < 0){ newRed = 0; }
                if (newGreen > 255){ newGreen = 255; }
                else if (newGreen < 0){ newGreen = 0; }
                if (newBlue > 255){ newBlue = 255; }
                else if (newBlue < 0){ newBlue = 0; }
                texToSave.SetPixel(j, k, new Color32((byte)newRed, (byte)newGreen, 
                    (byte)newBlue, theShadingsAlpha));
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
    void ToSetColor(int meaninglessInteger, Color32 theColorSetThere)
    {
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        for (int j = 0; j < texToSave.width; j++)
        {
            for (int k = 0; k < texToSave.height; k++)
            {
                Color32 theColor = texToSave.GetPixel(j, k);
                texToSave.SetPixel(j, k, new Color32(theColorSetThere.r, theColorSetThere.g, theColorSetThere.b, theColor.a));
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
    void ShadeToShine()
    {
        if (basedOnThisThing == "")
        {
            SayTheyArentTheSameResolution(1);
            return;
        }
        else if (adjustThisThing == "")
        {
            SayTheyArentTheSameResolution(0);
            return;
        }
        else if (basedOnThisThing == adjustThisThing)
        {
            SayTheyArentTheSameResolution(2);
            return;
        }
        byte[] theByteArray = File.ReadAllBytes(basedOnThisThing);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(theByteArray);
        tex.Apply();
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        if (tex.width != texToSave.width || tex.height != texToSave.height)
        {
            SayTheyArentTheSameResolution(3);
            return;
        }
        for (int j = 0; j < tex.width; j++)
        {
            for (int k = 0; k < tex.height; k++)
            {
                float H, S, V;
                Color32 theColor = texToSave.GetPixel(j, k);
                Color32 theTranslucency = tex.GetPixel(j, k);
                int integer = theTranslucency.a;
                float theOriginalTranslucency = integer;
                Color.RGBToHSV(theTranslucency, out H, out S, out V);
                S = 0f;
                theTranslucency = Color.HSVToRGB(H, S, V);
                integer = theTranslucency.r;
                float theAverage = integer;
                float theResultingTranslucency = theOriginalTranslucency - theAverage;
                if (theResultingTranslucency <= 0f)
                {
                    theResultingTranslucency = 0f;
                }
                int theResultingTranslucencyInteger = Mathf.FloorToInt(theResultingTranslucency);
                texToSave.SetPixel(j, k, new Color32(theColor.r, theColor.g, theColor.b, (byte)theResultingTranslucencyInteger));
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
    void CopyPasteTranslucency(int copypasteMode, Color32 unusedHex)
    {
        byte[] theByteArray = File.ReadAllBytes(basedOnThisThing);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(theByteArray);
        tex.Apply();
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        if (tex.width != texToSave.width || tex.height != texToSave.height)
        {
            SayTheyArentTheSameResolution(3);
            return;
        }
        bool dontBecomeMoreOpaque = copypasteMode == 1;
        for (int j = 0; j < tex.width; j++)
        {
            for (int k = 0; k < tex.height; k++)
            {
                Color32 theColor = texToSave.GetPixel(j, k);
                Color32 theTranslucency = tex.GetPixel(j, k);
                if (dontBecomeMoreOpaque)
                {
                    int theAlphaToSteal = theTranslucency.a;
                    int theirOwnAlpha = theColor.a;
                    if (theirOwnAlpha > theAlphaToSteal)
                    {
                        texToSave.SetPixel(j, k, new Color32(theColor.r, theColor.g, theColor.b, theTranslucency.a));
                    }
                    else
                    {
                        texToSave.SetPixel(j, k, new Color32(theColor.r, theColor.g, theColor.b, theColor.a));
                    }
                }
                else
                {
                    texToSave.SetPixel(j, k, new Color32(theColor.r, theColor.g, theColor.b, theTranslucency.a));
                }               
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
    void InvertColors()
    {
        if (adjustThisThing == "")
        {
            SayTheyArentTheSameResolution(0);
            return;
        }
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        for (int j = 0; j < texToSave.width; j++)
        {
            for (int k = 0; k < texToSave.height; k++)
            {
                Color32 theColor = texToSave.GetPixel(j, k);
                int theColorR = theColor.r;
                int theColorG = theColor.g;
                int theColorB = theColor.b;
                int invertR = 255 - theColorR;
                int invertG = 255 - theColorG;
                int invertB = 255 - theColorB;
                texToSave.SetPixel(j, k, new Color32((byte)invertR, (byte)invertG, (byte)invertB, theColor.a));
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
    void ChangeToPitchBlack()
    {
        if (adjustThisThing == "")
        {
            SayTheyArentTheSameResolution(0);
            return;
        }
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        for (int j = 0; j < texToSave.width; j++)
        {
            for (int k = 0; k < texToSave.height; k++)
            {
                Color32 theColor = texToSave.GetPixel(j, k);
                texToSave.SetPixel(j, k, new Color32(0, 0, 0, theColor.a));
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
    // Update is called once per frame
    bool firstUpdateAlreadyDone = false;
    public bool secondUpdateAlreadyDone = false;
    void Update()
    {
        if (!firstUpdateAlreadyDone)
        {
            firstUpdateAlreadyDone = true;
            AssignSomeButtons();
        }
        else
        {
            if (!secondUpdateAlreadyDone)
            {
                secondUpdateAlreadyDone = true;
            }
        }
        if (!isTheFileExplorerOn)
        {
            theFileExplorer.transform.localPosition = new Vector3(0f, 1440f);
        }
        else
        {
            theFileExplorer.transform.localPosition = new Vector3(0f, 0f);
        }
        lobbyQuestionMark.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = theErrorMessageToTheUser;
        theFileExplorer.transform.GetChild(0).GetComponent<Text>().text = theExploredDirectory;
    }
    void RemoveSimilar(int tolerance, Color32 unusedHex)
    {
        byte[] theByteArray = File.ReadAllBytes(basedOnThisThing);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(theByteArray);
        tex.Apply();
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        if (tex.width != texToSave.width || tex.height != texToSave.height)
        {
            SayTheyArentTheSameResolution(3);
            return;
        }
        if (tolerance > 100)
        {
            tolerance = 100;
        }
        float theTolerance = tolerance;
        theTolerance /= 100f;
        float theMaxDist = Vector3.Distance(new Vector3(0f, 0f, 0f), new Vector3(255f, 255f, 255f));
        theTolerance *= theMaxDist;
        for (int j = 0; j < tex.width; j++)
        {
            for (int k = 0; k < tex.height; k++)
            {
                Color32 theColor = texToSave.GetPixel(j, k);
                Color32 theTranslucency = tex.GetPixel(j, k);
                int theColorAlphaUsuallyNotUsed = theColor.a;
                int theTranslucencyAlpha = 0;
                int theR = theColor.r;
                int theG = theColor.g;
                int theB = theColor.b;
                Vector3 theColorInVector = new Vector3((float)theR, (float)theG, (float)theB);
                theR = theTranslucency.r;
                theG = theTranslucency.g;
                theB = theTranslucency.b;
                Vector3 theTranslucencyInVector = new Vector3((float)theR, (float)theG, (float)theB);
                float toCompareWithTolerance = Vector3.Distance(theColorInVector, theTranslucencyInVector);
                if (toCompareWithTolerance > theTolerance)
                {
                    //try to UNremove
                    theTranslucencyAlpha = theColorAlphaUsuallyNotUsed;
                }
                texToSave.SetPixel(j, k, new Color32(theColor.r, theColor.g, theColor.b, (byte)theTranslucencyAlpha));
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
    void RemoveDissimilar(int tolerance, Color32 unusedHex)
    {
        byte[] theByteArray = File.ReadAllBytes(basedOnThisThing);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(theByteArray);
        tex.Apply();
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        if (tex.width != texToSave.width || tex.height != texToSave.height)
        {
            SayTheyArentTheSameResolution(3);
            return;
        }
        if (tolerance > 100)
        {
            tolerance = 100;
        }
        float theTolerance = tolerance;
        theTolerance /= 100f;
        float theMaxDist = Vector3.Distance(new Vector3(0f, 0f, 0f), new Vector3(255f, 255f, 255f));
        theTolerance *= theMaxDist;
        for (int j = 0; j < tex.width; j++)
        {
            for (int k = 0; k < tex.height; k++)
            {
                Color32 theColor = texToSave.GetPixel(j, k);
                Color32 theTranslucency = tex.GetPixel(j, k);
                int theColorAlpha = theColor.a;
                int theR = theColor.r;
                int theG = theColor.g;
                int theB = theColor.b;
                Vector3 theColorInVector = new Vector3((float)theR, (float)theG, (float)theB);
                theR = theTranslucency.r;
                theG = theTranslucency.g;
                theB = theTranslucency.b;
                Vector3 theTranslucencyInVector = new Vector3((float)theR, (float)theG, (float)theB);
                float toCompareWithTolerance = Vector3.Distance(theColorInVector, theTranslucencyInVector);
                if (toCompareWithTolerance > theTolerance)
                {
                    //try to remove
                    theColorAlpha = 0;
                }
                texToSave.SetPixel(j, k, new Color32(theColor.r, theColor.g, theColor.b, (byte)theColorAlpha));
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
    void ToPitchBlackAndPureWhite(int thresholdInPercentage, Color32 unusedHex)
    {
        byte[] theByteArray2 = File.ReadAllBytes(adjustThisThing);
        Texture2D texToSave = new Texture2D(2, 2);
        texToSave.LoadImage(theByteArray2);
        texToSave.Apply();
        if (thresholdInPercentage > 100)
        {
            thresholdInPercentage = 100;
        }
        else if (thresholdInPercentage < 0)
        {
            thresholdInPercentage = 0;
        }
        thresholdInPercentage = 100 - thresholdInPercentage;
        float thePercentageInDecimals = thresholdInPercentage;
        thePercentageInDecimals /= 100f;
        float f = thePercentageInDecimals * 255f;
        for (int j = 0; j < texToSave.width; j++)
        {
            for (int k = 0; k < texToSave.height; k++)
            {
                Color32 theColor = texToSave.GetPixel(j, k);
                float theColorsAverage = theColor.r + theColor.g + theColor.b;
                theColorsAverage /= 3f;
                if (theColorsAverage > f)
                {
                    texToSave.SetPixel(j, k, new Color32(255, 255, 255, theColor.a));
                }
                else
                {
                    texToSave.SetPixel(j, k, new Color32(0, 0, 0, theColor.a));
                }
            }
        }
        texToSave.Apply();
        byte[] bytes = texToSave.EncodeToPNG();
        File.WriteAllBytes(adjustThisThing, bytes);
        theErrorMessageToTheUser = "";
        ReloadStuff();
    }
}
