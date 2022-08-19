using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;


    [Header("Stats")]

    [SerializeField] private float moveSpeed;
    private Vector2 currentVelocity;
    public Vector2 moveInput;
    private Vector2 smoothInputVelocity;
    public Vector3 velocity;
    [SerializeField] private float accelDeccel;
    [SerializeField] private float jumpSpeed;

    [SerializeField] private float gravity;
    [SerializeField] private LayerMask groundLayer;
    private bool grounded;
    private bool jumping;



    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

    }

    public void OnJump(InputValue value)
    {
        jumping = true;
    }


    private void Update()
    {
        CheckGrounded();
        Smoothing();
        Move();
        VerticalMove();
    }

    void CheckGrounded()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, controller.height / 2 + 0.2f, groundLayer);
    }

    void Smoothing()
    {
        currentVelocity = Vector2.SmoothDamp(currentVelocity, moveInput, ref smoothInputVelocity, accelDeccel);
    }

    void Move()
    {
        velocity = new Vector3(currentVelocity.x, velocity.y, currentVelocity.y);
        velocity = (currentVelocity.x * transform.right + velocity.y * transform.up + currentVelocity.y * transform.forward);

        velocity.y -= gravity * Time.deltaTime;

        if (grounded && !jumping)
        {
            velocity.y = 0;
        }

        if (grounded && jumping)
        {
            velocity.y += jumpSpeed;
            jumping = false;
        }

        controller.Move(velocity * Time.deltaTime * moveSpeed);


    }

    void VerticalMove()
    {

    }
}
