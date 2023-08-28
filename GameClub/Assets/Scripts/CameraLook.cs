using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {
    public float mouseSensitivity = 300f;
    public Transform playerBody;

    float currentXRotation = 0f;
    public float rotationSmoothness = 5f;
    public float PitchAngle { get { return -currentXRotation; } }

    void Start() {
        currentXRotation = transform.localRotation.eulerAngles.x;
    }

    void Update() {
        
        currentXRotation = transform.localRotation.eulerAngles.x;
    }
}