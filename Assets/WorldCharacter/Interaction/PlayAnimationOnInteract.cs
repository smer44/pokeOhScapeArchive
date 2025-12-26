using UnityEngine;

public class PlayAnimationOnInteract : Interactable
{
    public Animator receiver;
    [SerializeField] private string triggerName = "Interact";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnInteract(GameObject other)
    {
        receiver.SetTrigger(triggerName);
    }
}
