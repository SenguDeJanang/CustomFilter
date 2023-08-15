using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontScrollOutOfBounds : MonoBehaviour
{
    int timer;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Stretch()
    {
        if (transform.childCount > 36)
        {
            float integerInFloat = Mathf.Ceil(((float)transform.childCount) / 4f);
            GetComponent<RectTransform>().sizeDelta = new Vector2(integerInFloat * 200f, GetComponent<RectTransform>().sizeDelta.y);
        }
        else
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(1800f, GetComponent<RectTransform>().sizeDelta.y);
        }
    }
    // Update is called once per frame
    void Update()
    {
        Stretch();
        float theSize = GetComponent<RectTransform>().sizeDelta.x;
        float acceptableScrollDistance = (theSize - 1800f) / 2f;
        if (transform.localPosition.x > acceptableScrollDistance)
        {
            transform.localPosition = new Vector3(acceptableScrollDistance, transform.localPosition.y);
        }
        else if (transform.localPosition.x < -acceptableScrollDistance)
        {
            transform.localPosition = new Vector3(-acceptableScrollDistance, transform.localPosition.y);
        }
        if (ImageProcessingManager.instance.showTheRightFile)
        {
            timer = 3;
            ImageProcessingManager.instance.showTheRightFile = false;
        }
        if (timer > 0)
        {
            timer -= 1;
            transform.localPosition = new Vector3(-acceptableScrollDistance, transform.localPosition.y);
        }
    }
}
