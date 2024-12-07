using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of the character

    private Vector2 moveInput;  // Input from the player
    private Rigidbody2D rb;     // Rigidbody2D component for movement

    //private PlayerControls controls;

    [SerializeField]
    private InputActionAsset inputs;


    private InputAction moveAction;
    private InputAction upAction;
    private InputAction downAction;
    private InputAction leftAction;
    private InputAction rightAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = inputs.FindActionMap("Player").FindAction("Move");
        upAction = inputs.FindActionMap("Player").FindAction("UP");
        downAction = inputs.FindActionMap("Player").FindAction("DOWN");
        leftAction = inputs.FindActionMap("Player").FindAction("LEFT");
        rightAction = inputs.FindActionMap("Player").FindAction("RIGHT");
    }

    private void FixedUpdate()
    {
        Vector2 move = moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }

    // Called by the Input System
    public void OnMoveGamepad(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnMoveW(InputAction.CallbackContext context)
    {
        moveInput.y = context.ReadValue<float>();
    }
    public void OnMoveA(InputAction.CallbackContext context)
    {
        moveInput.x = -context.ReadValue<float>();
    }
    public void OnMoveD(InputAction.CallbackContext context)
    {
        moveInput.x = context.ReadValue<float>();
    }
    public void OnMoveS(InputAction.CallbackContext context)
    {
        moveInput.y = -context.ReadValue<float>();
    }

    

    private void OnEnable()
    {
        moveAction.performed += OnMoveGamepad;
        moveAction.canceled += OnMoveGamepad;
        upAction.performed += OnMoveW;
        upAction.canceled += OnMoveW;
        downAction.performed += OnMoveS;
        downAction.canceled += OnMoveS;
        leftAction.performed += OnMoveA;
        leftAction.canceled += OnMoveA;
        rightAction.performed += OnMoveD;
        rightAction.canceled += OnMoveD;
        moveAction.Enable();
        upAction.Enable();
        downAction.Enable();
        leftAction.Enable();
        rightAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        upAction.Disable();
        downAction.Disable();
        leftAction.Disable();
        rightAction.Disable();
    }
    
}
 
