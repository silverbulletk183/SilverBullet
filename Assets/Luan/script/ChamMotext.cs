using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ChamMotext : MonoBehaviour
{
    public GameObject textdong;
    public GameObject textmo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            textdong.SetActive(false);
            textmo.SetActive(true);
        }
    }
}
