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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    { 
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        rb.velocity = moveInput * moveSpeed * 10 * Time.deltaTime;
    }

}
 
