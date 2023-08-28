using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    /*
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
    */

    public bool IsSprinting => canSprint && Input.GetKey(sprintKey);
    public bool ShouldJump => Input.GetKey(jumpKey) && characterController.isGrounded;


    public CameraLook cameraLook;

    [Header("Functional Options")]
    [SerializeField] private bool canSprint = true;
    [SerializeField] private bool canJump = true;

    [Header("Controls")]
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    [Header("Movement Parameters")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintSpeed = 6.0f;


    [Header("Look Parameters")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 100)] private float upperLookLimit = 80.0f;
    [SerializeField, Range(1, 100)] private float lowerLookLimit = 80.0f;

    [Header("Jumping Parameters")]
    [SerializeField] private float gravity = 30.0f;
    [SerializeField] private float jumpForce = 8.0f;


    private Camera playerCamera;
    private CharacterController characterController;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    private float rotationX = 0;

    private Vector3 spawnPoint;

    public enum PlayerState { CanMove };
    public PlayerState playerState;

    void Start() {
        cameraLook = GetComponentInChildren<CameraLook>();

        playerCamera = GetComponentInChildren<Camera>();
        characterController = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerState = PlayerState.CanMove;
    }

    void Update() {
        

        switch (playerState)
        {
            case PlayerState.CanMove:
                HandleMovementInput();
                HandleMouseLook();
                if (canJump)
                {
                    HandleJump();
                }
               
                ApplyFinalMovements();
                break;
        }
    }


    /*
    * Detects user input and sets the movement vector based on key inputs and speeds based on current actions.
    */
    private void HandleMovementInput()
    {
        currentInput = new Vector2((IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"),
            (IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;
    }

    /*
     * Detects mouse movement and rotates the camera accordingly to match vision. Also clamps movement to a certain view range.
     */
    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    /*
     * Applies jump force to the user if they are grounded and the right key was pressed.
     */
    private void HandleJump()
    {
        if (ShouldJump)
            moveDirection.y = jumpForce;
    }


    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        if (characterController.velocity.y < -0.01f && characterController.isGrounded)
        {
            moveDirection.y = 0;

        }
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
