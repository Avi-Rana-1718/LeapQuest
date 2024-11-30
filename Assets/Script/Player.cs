using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller; // Reference to the CharacterController
    public Animator animator; // Reference to the Animator

    public float speed = 6f; // Movement speed
    public float jumpSpeed = 4f; // Initial upward speed during jump
    public float gravity = -100.81f; // Gravity value
    public float turnSmoothTime = 0.1f; // Smooth turning time
    private float turnSmoothVelocity; // Reference velocity for SmoothDamp
    public float ySpeed = 0f; // Current vertical speed
    private Vector3 velocity; // Combined movement velocity
    public float coyoteTime =0.2f;
    public float? lastGroundedTime;
    private float? jumpButtonPressTime;



    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if(controller.isGrounded) {
            lastGroundedTime = Time.time;
        }

        if(Input.GetButtonDown("Jump")) {
            jumpButtonPressTime=Time.time;
        }

        if (Time.time - lastGroundedTime <= coyoteTime)
        {
            // Reset vertical speed when grounded
            ySpeed = -0.5f;

            // Check for jump input
            if (Time.time - jumpButtonPressTime <= coyoteTime)
            {
                animator.SetBool("isJumping", true);
                ySpeed = jumpSpeed;
                jumpButtonPressTime=null;
                lastGroundedTime=null;
            } else {
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            // Apply gravity if not grounded
            ySpeed += gravity * Time.deltaTime;
        }

        if (direction.magnitude >= 0.1f)
        {
            // Calculate target rotation angle
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            // Smoothly interpolate the rotation
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // Set walking animation
            animator.SetBool("isMoving", true);
        }
        else
        {
            // Stop walking animation
            animator.SetBool("isMoving", false);
        }

        // Apply movement
        velocity = direction * speed;
        velocity.y = ySpeed; // Add vertical velocity for jumping and gravity
        controller.Move(velocity * Time.deltaTime);
    }
}
