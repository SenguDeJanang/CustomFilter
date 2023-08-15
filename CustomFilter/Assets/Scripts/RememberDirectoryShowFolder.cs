using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RememberDirectoryShowFolder : MonoBehaviour
{
    public string theFolderDirectory;
    public Button theButton;
    public void HaveTheSingletonMimicThis()
    {
        ImageProcessingManager.instance.theExploredDirectory = theFolderDirectory;
        ImageProcessingManager.instance.ReloadTheGridDelayed();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _StuffIWouldStuffInUpdate();
    }
    void _StuffIWouldStuffInUpdate()
    {
        Text directoryName = transform.GetChild(1).GetComponent<Text>();
        string theDirectoryNameTextProxy = theFolderDirectory;
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
        directoryName.text = theRemainingCharacters;
    }
}
