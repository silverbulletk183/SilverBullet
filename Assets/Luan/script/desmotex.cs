using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winpanel : MonoBehaviour
{
    public GameObject panelwin;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.R)) 
        
        {

            Invoke("winn", 4);

        }
    }

    void winn()
    {
        panelwin.SetActive(true);
    }
}
