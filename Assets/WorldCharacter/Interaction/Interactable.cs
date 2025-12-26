using UnityEngine;


public abstract class Interactable : MonoBehaviour
{
    // Every interactable must implement this
    public abstract void OnInteract(GameObject other);
}