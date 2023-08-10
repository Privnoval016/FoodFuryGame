using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float WALK_SPEED = 12f;
    public float SPRINT_SPEED = 17f;
    private float speed;

    public CharacterController controller;
    public float gravity = -9.8f;
    public Transform ground;
    public float groundDist = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGround;
    public CameraLook cameraLook;

    void Start() {
        speed = WALK_SPEED;
        cameraLook = GetComponentInChildren<CameraLook>();
    }

    void Update() {
        isGround = Physics.CheckSphere(ground.position, groundDist, groundMask);
        if (isGround && velocity.y < 0) {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift)) {
            speed = SPRINT_SPEED;
        } else {
            speed = WALK_SPEED;
        }

        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
