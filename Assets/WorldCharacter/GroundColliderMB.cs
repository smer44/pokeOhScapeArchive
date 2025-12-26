using UnityEngine;

public class GroundColliderMB : MonoBehaviour
{
    

    [Header("Grounding")]
    [SerializeField] private Collider groundCollider;
    public int groundContacts = 0;
    //public bool IsGrounded => groundContacts > 0;
    [SerializeField] public bool isGrounded;// { get; private set; }


    private void OnTriggerEnter(Collider other)
    {
        groundContacts++;
        isGrounded = true;
        
    }

    void OnTriggerStay(Collider other)
    {
        isGrounded = true;
    }

    private void OnTriggerExit(Collider other)
    {
        groundContacts = Mathf.Max(groundContacts - 1, 0);
        if (groundContacts == 0)
        {
            isGrounded = false;
        }
    } 
    
}
