using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller; // Reference to the CharacterController
    public Animator animator; // Reference to the Animator
    public AudioSource src;
    public AudioClip sfx1;

    public float speed = 6f; // Movement speed
    public float jumpSpeed = 4f; // Initial upward speed during jump
    public float gravity = -9.81f; // Gravity value
    public float turnSmoothTime = 0.1f; // Smooth turning time
    private float turnSmoothVelocity; // Reference velocity for SmoothDamp
    public float ySpeed = 0f; // Current vertical speed
    private Vector3 velocity; // Combined movement velocity
    public float coyoteTime = 0.2f;
    public float? lastGroundedTime;
    private float? jumpButtonPressTime;

    public float bounceForce = 20f;  // Bounce force applied by the spring

    public float downwardRaycastDistance = 4f; // How far down the ray will check when the player is above the spring
    public LayerMask springLayer; // Layer mask to specify which layers the ray should check

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Handle Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        // Ground check and jump input handling
        if (controller.isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= coyoteTime)
        {
            // Reset vertical speed when grounded
            ySpeed = -0.5f;

            // Check for jump input
            if (Time.time - jumpButtonPressTime <= coyoteTime)
            {
                animator.SetBool("isJumping", true);
                src.clip = sfx1;
                src.Play();
                ySpeed = jumpSpeed;
                jumpButtonPressTime = null;
                lastGroundedTime = null;
            }
            else
            {
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            // Apply gravity if not grounded
            ySpeed += gravity * Time.deltaTime;
        }

        // Move the player (horizontal movement)
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

        // Perform Downward Raycast to detect the Spring directly below the player
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, downwardRaycastDistance, springLayer))
        {
            // Check if the raycast hit a spring directly below the player
            if (hit.collider.CompareTag("Spring"))
            {
                // Only apply bounce if the player is falling or not moving upwards
                if (ySpeed <= 0)  // Only bounce when falling or stationary
                {
                    ySpeed = bounceForce;  // Set the vertical speed to bounce force
                    Debug.Log("Player bounced off the spring below!");
                    animator.SetBool("isJumping", true); // Trigger jump animation
                    src.clip = sfx1;  // Play bounce sound
                    src.Play();
                }
            }
        }

        // Check if the player is on top of the spring to amplify jump force
        if (Physics.Raycast(transform.position, Vector3.down, out hit, downwardRaycastDistance, springLayer))
        {
            // If the player is on top of the spring, amplify the jump force by 10
            if (hit.collider.CompareTag("Spring"))
            {
                Debug.Log("Is spring");
                // Amplify the vertical jump force by a factor of 10
                if (ySpeed <=0) // Only trigger when falling or stationary
                {
                    ySpeed = jumpSpeed * 10f;  // Amplify the jump force
                    animator.SetBool("isJumping", true); // Trigger jump animation
                    src.clip = sfx1;  // Play bounce sound
                    src.Play();
                    Debug.Log("Player jumped with amplified force!");
                }
            }
        }
    }
}
