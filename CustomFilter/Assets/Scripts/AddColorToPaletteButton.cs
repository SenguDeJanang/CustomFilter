using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddColorToPaletteButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<Button>().onClick.AddListener(TryToAddPalette);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void TryToAddPalette()
    {
        if (transform.parent.childCount >= 20)
        {
            ImageProcessingManager.instance.SayTheyArentTheSameResolution(5);
            return;
        }
        Transform t = Instantiate(transform.parent.GetChild(0));
        t.GetChild(0).GetComponent<InputField>().text = "";
        t.SetParent(transform.parent);
        t.localScale *= ImageProcessingManager.instance.canvasAdjusterScaleFactor;
        transform.SetAsLastSibling();
    }
}
