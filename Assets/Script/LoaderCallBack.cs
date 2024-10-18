using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallBack : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isFistUpdate = true;

    // Update is called once per frame
    void Update()
    {
        if (isFistUpdate)
        {
            isFistUpdate=false;
            Loader.LoaderCallback();
        }
        
    }
}
