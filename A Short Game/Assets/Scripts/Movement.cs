using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] CharacterController controller;


    // Movement
    [SerializeField] private float smoothInputSpeed = 0.2f;
    Vector2 movementInput;
    Vector2 currentVelocity;
    float currentRotation;
    private Vector2 smoothInputVelocity;
    [SerializeField] private float moveSpeed;


    // Gravity
    [SerializeField] private float gravity = -30f;
    Vector3 verticalVelocity;
    [SerializeField] LayerMask groundMask;
    [HideInInspector] public bool grounded;


    //Jumping
    //[SerializeField] private float jumpHeight = 3.5f;
    //bool jumping;


    /// <summary>
    /// Input Calls
    /// </summary>
    public void OnMovement(InputValue value)
    {
        movementInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        //jumping = true;
    }


    /// <summary>
    /// Function Calls
    /// </summary>
    private void Update()
    {
        Move();
        VertMove();
    }


    /// <summary>
    /// Movement Code
    /// </summary>
    // Horizontal Movement
    void Move()
    {
        currentRotation += movementInput.x *3;


        currentVelocity = Vector2.SmoothDamp(currentVelocity, movementInput, ref smoothInputVelocity, smoothInputSpeed);
        Vector3 velocity = (transform.right * currentVelocity.x + transform.forward * currentVelocity.y);
        controller.Move(velocity * Time.deltaTime * moveSpeed);
    }

    // Vertical Movement
    void VertMove()
    {
        grounded = Physics.Raycast(transform.position, -Vector3.up, controller.bounds.extents.y + 0.1f);


        // This section could probably be refined

        // Reset vertical velocity when you touch the ground so you dont build up downwards velocity
        if (grounded)
        {
            verticalVelocity.y = 0;
        }

        /**
        // Jumping
        if (jumping)
        {
            if (grounded)
            {
                verticalVelocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
            }
        }
        jumping = false;
        **/

        verticalVelocity.y += gravity * Time.deltaTime;
        controller.Move(verticalVelocity * Time.deltaTime);
    }

}
