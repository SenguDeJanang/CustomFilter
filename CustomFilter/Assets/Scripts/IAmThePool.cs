using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAmThePool : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ImageProcessingManager.instance.objectToPool = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
