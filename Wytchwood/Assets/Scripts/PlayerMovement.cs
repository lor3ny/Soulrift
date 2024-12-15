using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of the character

    private Vector2 moveInput;  // Input from the player
    private Rigidbody2D rb;     // Rigidbody2D component for movement
    private float m_MovementSmoothing = .05f;

    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_Movement = Vector3.zero;

    //private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    private void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        m_Movement = (Vector3)moveInput.normalized * moveSpeed;
    }


    private void FixedUpdate()
    {
        rb.velocity = Vector3.SmoothDamp(rb.velocity, m_Movement, ref m_Velocity, m_MovementSmoothing);
    }

}
 
