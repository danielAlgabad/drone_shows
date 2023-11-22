using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float speed = 50f;
    public float sensitivity;
    private GameObject canvas;


    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("UI");
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        if(Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.down * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.right * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.UpArrow))
            transform.Translate(Vector3.up * Time.deltaTime * speed);
        if (Input.GetKey(KeyCode.DownArrow))
            transform.Translate(Vector3.down * Time.deltaTime * speed);
    }
}
