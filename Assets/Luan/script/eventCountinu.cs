using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class eventCountinu : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKey(KeyCode.Return)) {
            SceneManager.LoadScene(1);
        }
    }
}
