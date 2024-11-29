using UnityEngine;

public class Star : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

        if(playerInventory!=null) {
            playerInventory.StarCollected();
            gameObject.SetActive(false);
        }
    }
}
