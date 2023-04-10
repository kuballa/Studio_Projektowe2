using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    float movementSpeed = 6f;
    float lookingSpeed = 6f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // looking around

        yaw += lookingSpeed * Input.GetAxis("Mouse X");
        pitch -= lookingSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3 (pitch, yaw, 0.0f);

        // moving around

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float heightInput = Convert.ToInt32(Input.GetKey(KeyCode.Space)) - Convert.ToInt32(Input.GetKey(KeyCode.LeftShift));

        float angle = transform.eulerAngles[1] * Mathf.PI / 180;

        transform.position += new Vector3(Time.deltaTime * movementSpeed * (verticalInput * Mathf.Sin(angle) + horizontalInput * Mathf.Cos(angle)),
                                          Time.deltaTime * movementSpeed * heightInput, 
                                          Time.deltaTime * movementSpeed * (verticalInput * Mathf.Cos(angle) - horizontalInput * Mathf.Sin(angle)));
        // transform.position += transform.TransformVector(horizontalInput * Time.deltaTime * movementSpeed, 0, verticalInput * Time.deltaTime * movementSpeed);
    }
}
