using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TimeMotext : MonoBehaviour
{
    public GameObject textdong;
    public GameObject textmo;
    public float time = 7f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            textdong.SetActive(false);
            textmo.SetActive(true);
        }
    }
}
