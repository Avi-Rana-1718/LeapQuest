using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int numberOfStar {get; private set;}

    public void StarCollected() {
        numberOfStar++;
    }
}
