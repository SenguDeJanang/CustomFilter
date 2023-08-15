using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAmTheFileExplorer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ImageProcessingManager.instance.theFileExplorer = gameObject;
        ImageProcessingManager.instance.lobbyQuestionMark = transform.parent.GetChild(0).gameObject;
        ImageProcessingManager.instance.fileFolderGrid = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
