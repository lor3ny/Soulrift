using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of the character
    public GameObject sprite;

    private Vector2 moveInput;  // Input from the player
    private Rigidbody2D rb;     // Rigidbody2D component for movement
    private float m_MovementSmoothing = .05f;

    private Vector3 m_Velocity = Vector3.zero;
    private Vector3 m_Movement = Vector3.zero;

    private Animator anim;

    //private PlayerControls controls;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }


    private void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        m_Movement = (Vector3)moveInput.normalized * moveSpeed;

        if (m_Movement == Vector3.zero)
        {
            anim.SetBool("IsWalking", false);
        } else
        {
            anim.SetBool("IsWalking", true);
        }
        if (m_Movement.x > 0)
        {
            sprite.GetComponent<SpriteRenderer>().flipX = false;
        } else
        {
            sprite.GetComponent<SpriteRenderer>().flipX = true;
        }

        /*
        Vector2 mousePos = (Vector2) Input.mousePosition;
        Vector2 mouseWorldPos = (Vector2) Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane));
        Vector2 direction = (mouseWorldPos - (Vector2)gameObject.transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        */

        // Apply the angle to the sprite's rotation (assuming it's a 2D sprite)
        //sprite.transform.rotation = Quaternion.Euler(0f, 0f, angle-90);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, m_Movement, ref m_Velocity, m_MovementSmoothing);
    }


    private void FixedUpdate()
    {
        //rb.velocity = Vector3.SmoothDamp(rb.velocity, m_Movement, ref m_Velocity, m_MovementSmoothing);
    }

}
 
