using UnityEngine;

public class Spring : MonoBehaviour
{
    public float bounceForce = 20f; // The force of the bounce

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Spring triggered!");

        // Check if the object that collided is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
            {
                // Only apply bounce if player is grounded (to prevent bouncing in mid-air)
                if (player.controller.isGrounded)
                {
                    player.ySpeed = bounceForce; 
                    Debug.Log("Player bounced!");
                }
            }
            else
            {
                Debug.Log("Player component not found!");
            }
        }
    }
}
