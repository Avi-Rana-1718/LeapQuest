using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public AudioSource src;
    public AudioClip sfx;

    public int numberOfStar {get; private set;}

    public void StarCollected() {
        src.clip=sfx;
        src.Play();
        numberOfStar++;
    }
}
