using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {
    public float mouseSensitivity = 300f;
    public Transform playerBody;
    float targetXRotation = 0f;
    float currentXRotation = 0f;
    public float rotationSmoothness = 5f;
    public float PitchAngle { get { return -currentXRotation; } }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        targetXRotation = transform.localRotation.eulerAngles.x;
        currentXRotation = targetXRotation;
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        targetXRotation -= mouseY;
        targetXRotation = Mathf.Clamp(targetXRotation, -90f, 90f);
        currentXRotation = Mathf.Lerp(currentXRotation, targetXRotation, rotationSmoothness);
        transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}