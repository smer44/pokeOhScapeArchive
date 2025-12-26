using UnityEngine;

public class TriggerCounter : MonoBehaviour
{
    // Public counter for debugging and inspector visibility
    public int counter { get; private set; } = 0;

    // Called when another collider enters this trigger collider
    private void OnTriggerEnter(Collider other)
    {
        counter++;
        //Debug.Log($"Trigger entered: {other.name}. Counter = {counter}");
    }

    // Called when another collider exits this trigger collider
    private void OnTriggerExit(Collider other)
    {
        counter--;
        //Debug.Log($"Trigger exited: {other.name}. Counter = {counter}");
    }

    public bool IsOn()
    {
        return counter > 0;
    }

    public bool IsOff()
    {
        return counter == 0;
    }

}
