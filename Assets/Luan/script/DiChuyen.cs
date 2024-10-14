using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiChuyen : MonoBehaviour
{
    public float speed = 6f;

    private CharacterController controller;
    public Animator ani;

    public GameObject player;
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) { ani.SetBool("run", true); }
        else if (Input.GetKey(KeyCode.D)) { ani.SetBool("runR", true); }
        else if (Input.GetKey(KeyCode.A)) { ani.SetBool("runL", true); }
        else if(Input.GetKey(KeyCode.S)) { ani.SetBool("runB", true); }
        else {
            


            ani.SetBool("run", false);
               ani.SetBool("runR", false);
               ani.SetBool("runL", false);
               ani.SetBool("runB", false);}


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
    }
}
