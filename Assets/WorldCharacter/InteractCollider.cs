using UnityEngine;
using System.Collections.Generic;

public class InteractCollider : MonoBehaviour
{
    public KeyCode activationKey = KeyCode.E;

    private List<Interactable> interactables =new List<Interactable>();

    void OnTriggerEnter(Collider other)
    {
        // Store reference when something enters the trigger
        GameObject objectInTrigger = other.gameObject;
        Interactable receiver = objectInTrigger.GetComponent<Interactable>();
        if (receiver != null)
        {
            interactables.Add(receiver);
        }
        //Debug.Log("OnTriggerEnter:", other.gameObject);
    }
    void OnTriggerExit(Collider other)
    {
        GameObject objectInTrigger = other.gameObject;
        Interactable receiver = objectInTrigger.GetComponent<Interactable>();
        if (receiver != null)
        {
            interactables.Remove(receiver);
        }
            
    }

  
    void Update()
    {
        if (Input.GetKeyDown(activationKey))
        {
            //objectInTrigger.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            foreach (Interactable receiver in this.interactables)
            {
                Debug.Log($"InteractCollider.Update: triggering:|{receiver}|");
                receiver.OnInteract(this.gameObject);
            }
        }
    }
    

}
