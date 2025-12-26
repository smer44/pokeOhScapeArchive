using UnityEngine;
using UnityEngine.InputSystem;

public class WorldCharacterControlls : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string movingParam = "moving";
    [SerializeField] private string jumpingParam = "jumping";
    [SerializeField] private string fallingParam = "falling";
    [SerializeField] private string cliffHangingParam = "falling";


    [Header("References:")]
    [SerializeField] private Transform cameraTransform; // Assign your child Camera transform here

    [SerializeField] private Transform characterVisuals; // Assign your child Camera transform here

    [SerializeField] private GroundColliderMB groundColliderMB;

    [Header("Look")]
    [SerializeField] private float mouseSensitivity = 200f; // degrees per second at Mouse X/Y of 1
    [SerializeField] private float minPitch = -80f;
    [SerializeField] private float maxPitch = 80f;
    [SerializeField] private bool invertY = false;

    [Header("Turn")]
    
    [SerializeField] private float turningSpeed = 50f; //Speed with what caracter visuals will change facing

    [Header("Move")]
    [SerializeField] private float moveSpeed = 5f; // meters per second
                                                   //[SerializeField] private float gravity = -9.81f; // m/s^2
    [Header("Jump")]
    [SerializeField] private float jumpSpeed = 6f;

    [Header("Cliff climbing")]
    [SerializeField] private TriggerCounter negativeCliff;
    [SerializeField] private TriggerCounter positiveCliff;

    private bool isCliffHanging = false;
    private bool isCliffHangingUpdated = false;
    private float blockHangingTimeMax = 0.2f;
    private float blockHangingTime = 0f;
    private bool activeControlls = true;




    [Header("UX")]
    [SerializeField] private bool lockCursor = true;
    private float cameraPitch = 0f;
    private float cameraYaw = 0f;
    private Vector3 moveDir;
    private bool isMoving;
    private bool isJumping;

    private bool isFalling;
    private bool isMovingChanged;
    public InputAction newInput;
    private Rigidbody rb;

    private void UpdateCliffHangingValue()
    {
        bool isCliffHangingNew = blockHangingTime <= 0f && negativeCliff.IsOff() && positiveCliff.IsOn();
        if (isCliffHanging != isCliffHangingNew)
        {
            isCliffHanging = isCliffHangingNew;
            isCliffHangingUpdated = true;

            if (isFalling)
            {
                isFalling = false;
                animator.SetBool(fallingParam, isFalling);
            }
            if (isJumping)
            {
                isJumping = false;
                animator.SetBool(jumpingParam, isJumping);
            }
        }
    }


    public void SetActiveControlls(bool isActive)
    {
        this.activeControlls = isActive;
    }

    private void AwakeRigidbody()
    {
        rb = GetComponent<Rigidbody>();
        // Keep character from tipping when using physics
        rb.constraints = RigidbodyConstraints.FreezeRotation;// good one, GPT!
        //rb.interpolation = RigidbodyInterpolation.Interpolate;
        //rb.useGravity = false;        
    }

    private void Awake()
    {
        AwakeRigidbody();
        //moveDir = Vector3.zero;
        //rb.linearVelocity = Vector3.zero;
    }


    private void OnEnable()
    {
        newInput.Enable();
    }

    private void OnDisable()
    {
        newInput.Disable();
    }

   

    void Start()
    {

    }

    public void UpdateCliffHangingAnimation()
    {
        /*if (isCliffHangingUpdated)
        {
            animator.SetBool(cliffHangingParam, isCliffHanging);

        }*/

    }
    public void FixedUpdateCliffHangingVelocity()
    {
        if (isCliffHanging)
        {
            var v = rb.linearVelocity;
            v.y = 0f;
            rb.linearVelocity = v;
        }        
    }


    public void UpdateJumpRigidBody(float delta)
    {
        bool canJump = isCliffHanging || groundColliderMB.isGrounded;
        canJump = activeControlls & canJump;
        if (canJump && Input.GetKeyDown("space"))
        {
            if (isCliffHanging)
            {
                //blockHangingTime = blockHangingTimeMax;
                //isCliffHanging = false;
                //animator.SetBool(cliffHangingParam, isCliffHanging);

            }

            var v = rb.linearVelocity;
            v.y = jumpSpeed;
            rb.linearVelocity = v;
            isJumping = true;
            animator.SetBool(jumpingParam, isJumping);


        }
        //blockHangingTime -= delta;
        //if (blockHangingTime < 0f)
       // {
        //    blockHangingTime = 0f;
        //}
    }

    public void UpdateFallingRigidBody()
    {
        if (rb.linearVelocity.y < -0.01f)
        {
            if (isJumping)
            {
                isJumping = false;
                animator.SetBool(jumpingParam, isJumping);
            }

            if (!isFalling)
            {
                isFalling = true;
                animator.SetBool(fallingParam, isFalling);
            }
        }
        else
        {
            if (isFalling)
            {
                isFalling = false;
                animator.SetBool(fallingParam, isFalling);                
            }
        }
    }


    void Update()
    {
        float delta = Time.deltaTime;

        UpdateCameraWithMouse(delta);
        UpdateWASDInput(cameraTransform);
        //UpdateCenterMovement(delta);
        UpdateVisualsForward(delta);

        //Update animator:
        UpdateAnimatorMoving();
        UpdateJumpRigidBody(delta);

        //UpdateCliffHangingValue();
        //UpdateCliffHangingAnimation();
    }

    private void FixedUpdate()
    {
        UpdateCenterRigidbodyMovement(Time.fixedDeltaTime);
        //FixedUpdateCliffHangingVelocity();
        UpdateFallingRigidBody();
        
    }

    private void UpdateAnimatorMoving()
    {
        if (animator == null) return;
        if (!isMovingChanged) return; // Only push when the state flipped
        //Debug.Log($"Updating animator moving to {isMoving}");
        animator.SetBool(movingParam, isMoving);
    }

    void UpdateCenterMovement(float delta)
    {
        transform.position += moveDir * (moveSpeed * delta);
    }

    void UpdateCenterRigidbodyMovement(float delta)
    {
        Vector3 horizontal = moveDir * moveSpeed;
        Vector3 v = rb.linearVelocity;
        if (isMoving)
        {
            v.x = horizontal.x;
            v.z = horizontal.z;
        }
        else
        {
            v.x = 0f;
            v.z = 0f;
        }
        rb.linearVelocity = v;
     }

    void UpdateWASDInput(Transform forwardTransform)
    {
        if (!activeControlls) {
            moveDir = Vector3.zero;
            return;
        }
            
        Vector2 moveInput = newInput.ReadValue<Vector2>();
        // Convert to world-space using character's facing (XZ plane)
        Vector3 forward = Vector3.ProjectOnPlane(forwardTransform.forward, Vector3.up).normalized;
        Vector3 right = Vector3.ProjectOnPlane(forwardTransform.right, Vector3.up).normalized;
        moveDir = forward * moveInput.y + right * moveInput.x;
        moveDir.Normalize();

        if (isCliffHanging)
        {
            moveDir = Vector3.zero;
        }

        bool isMovingNew = moveDir.sqrMagnitude > 1e-6f;
        isMovingChanged = isMovingNew != isMoving;
        isMoving = isMovingNew;
    }

    void UpdateVisualsForward(float delta)
    {
        if (isMoving && moveDir != Vector3.zero)
        {
            characterVisuals.forward = Vector3.Lerp(characterVisuals.forward, moveDir, turningSpeed*delta);
        }
    }

    void UpdateHorizontalMouse(float delta)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float yawDelta = mouseX * mouseSensitivity * delta;
        //cameraTransform.Rotate(Vector3.up, yawDelta, Space.World);
    }

    void UpdateCameraWithMouse(float delta)
    {
        if (cameraTransform == null) return;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float sign = invertY ? 1f : -1f; // natural look usually uses negative


        cameraPitch += mouseY * mouseSensitivity * delta * sign;
        cameraPitch = Mathf.Clamp(cameraPitch, minPitch, maxPitch);

        cameraYaw += mouseX * mouseSensitivity * delta;

        //Vector3 oldRotation = cameraTransform.localRotation;
        cameraTransform.localRotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);

    }


void OnApplicationFocus(bool hasFocus)
{
    if (hasFocus)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    else
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}

}
