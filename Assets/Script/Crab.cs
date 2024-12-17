using UnityEngine;

public class CrabEnemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float changeDirectionTime = 3f;

    private float direction = 1f; // 1 for right, -1 for left
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        // Change direction randomly
        if (timer >= changeDirectionTime)
        {
            direction *= -1;
            timer = 0f;
        }

        // Move the crab horizontally
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);
    }
     void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {

                // Check if the player is above the crab and high enough to kill it
                if (collision.transform.position.y > transform.position.y + 1f)
                {
                    Destroy(gameObject);
                }
            }
        }
}