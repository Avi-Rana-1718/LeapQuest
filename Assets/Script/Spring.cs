using UnityEngine;

public class Spring : MonoBehaviour
{
    public float bounceForce = 20f;
    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Spring");
        if (collision.gameObject.name=="Player") {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                // Apply the bounce by modifying the player's ySpeed
                Debug.Log("Player bounced!");
            } else {
                Debug.Log("Player not!");
            }
        }
    }
}
